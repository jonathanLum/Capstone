using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EditorController : MonoBehaviour
{
    public Vector2Int boardLimits = new Vector2Int(7,7);
    public Renderer grid;
    
    public Vector3 mouseLoc;
    public Vector3 snapPos;

    Vector3 dragStartLoc;
    Vector3 p1;
    Vector3 dragEndLoc;
    bool dragging = false;

    [SerializeField] GameObject tilePrefab;

    public List<GameObject> selection;
    public List<GameObject> allTiles;


    private void Start() {
        grid.material.SetVector("_Scale", new Vector4(boardLimits.x, boardLimits.y, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000, LayerMask.NameToLayer("Terrain")))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            //Debug.Log(hit.point);
            mouseLoc.x = hit.point.x;
            mouseLoc.z = hit.point.z;
            snapPos.x = Mathf.RoundToInt(mouseLoc.x - 0.5f); 
            snapPos.z = Mathf.RoundToInt(mouseLoc.z - 0.5f);
            //Debug.Log(snapPos);

            if(Input.GetMouseButtonDown(1)){
                if(hitObject.tag == "Floor"){ 
                    selection.Clear();
                    if(!dragging){
                        dragStartLoc.x = hit.point.x;
                        dragStartLoc.z = hit.point.z;
                        p1 = Input.mousePosition;
                        dragging = true;
                    }
                }else{
                    ChangeSelection(hitObject);
                }
            }
            if(Input.GetMouseButtonUp(1)){
                if(dragging){
                    dragEndLoc.x = hit.point.x;
                    dragEndLoc.z = hit.point.z;
                    dragging = false;
                    SelectArea();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Delete)){
            DeleteSelection();
        }
    }

    void AddToSelection(GameObject tile){
        foreach(GameObject child in tile.GetComponent<EditorTile>().children){
            AddToSelection(child);
        }
        if(!selection.Contains(tile))
            selection.Add(tile);
    }

    void RemoveFromSelection(GameObject tile){
        if(tile.GetComponent<EditorTile>().parent)
            RemoveFromSelection(tile.GetComponent<EditorTile>().parent);
        selection.Remove(tile);
    }

    void ChangeSelection(GameObject obj){
        if(obj.CompareTag("Start") || obj.CompareTag("Tile") || obj.CompareTag("End")){
            if(selection.Contains(obj)){
                RemoveFromSelection(obj);
            }else{
                AddToSelection(obj);
            }
        }
    }

    void SelectArea(){
        var minX = Mathf.Min(dragStartLoc.x, dragEndLoc.x);
        var maxX = Mathf.Max(dragStartLoc.x, dragEndLoc.x);
        var minZ = Mathf.Min(dragStartLoc.z, dragEndLoc.z);
        var maxZ = Mathf.Max(dragStartLoc.z, dragEndLoc.z);
        
        foreach(GameObject tile in allTiles){
            if(minX < tile.transform.position.x + 0.5 && tile.transform.position.x + 0.5 < maxX){
                if(minZ < tile.transform.position.z + 0.5 && tile.transform.position.z + 0.5 < maxZ){
                    AddToSelection(tile);
                }
            }
        }
    }

    void DeleteSelection(){
        foreach(GameObject tile in selection.ToArray()){
                if(tile.GetComponent<EditorTile>().effect != EffectTypeEnum.Types.Start){
                    selection.Remove(tile);
                    allTiles.Remove(tile);
                    GameObject parent = tile.GetComponent<EditorTile>().parent;
                    parent.GetComponent<EditorTile>().children.Remove(tile);
                    Destroy(tile);
                }
        }
    }

    private void OnGUI(){
        if(dragging){
            var rect = SelectionBox.GetScreenRect(p1, Input.mousePosition);
            SelectionBox.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            SelectionBox.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }


    public GameObject lastTile;
    bool DrawProtectionCheck(){
        // Check occupied space
        if(isOccupied(snapPos))
            return false;

        // Check outside board limits
        if(snapPos.x < -boardLimits.x || boardLimits.x-1 < snapPos.x || snapPos.z < -boardLimits.y || boardLimits.y-1 < snapPos.z)
            return false;
        
        // Check too many neighbors ignoring lastTile
        if(checkSurroundings(snapPos) == false)
            return false;

        // only allows tile to be drawn 1 away from last tile (prevents diagonally created tiles)
        if((lastTile.transform.position-snapPos).magnitude > 1)
            return false;
        
        return true;
    }
    
    bool isOccupied(Vector3 pos){
        //check Occupied space
        foreach(GameObject tile in allTiles){
            if(tile.transform.position == pos)
                return true;
        }
        return false;
    }

    bool checkSurroundings(Vector3 pos){
        //check for tiles surrounding the new tile
        var right = snapPos + new Vector3 (1,0,0);
        var left = snapPos + new Vector3 (-1, 0, 0);
        var up = snapPos + new Vector3 (0, 0, 1);
        var down = snapPos + new Vector3 (0, 0, -1);
        if(isOccupied(right) && lastTile.transform.position != right)
            return false;
        if(isOccupied(left) && lastTile.transform.position != left)
            return false;
        if(isOccupied(up) && lastTile.transform.position != up)
            return false;
        if(isOccupied(down) && lastTile.transform.position != down)
            return false;
        return true;
    }
    
    public void Draw(){
        // Draw Protections
        if(DrawProtectionCheck() == false)
            return;
        
        // Spawn tile
        GameObject newTile = Instantiate(tilePrefab, snapPos, Quaternion.identity);
        allTiles.Add(newTile);
        newTile.GetComponent<EditorTile>().parent = lastTile;
        lastTile.GetComponent<EditorTile>().children.Add(newTile);
        lastTile = newTile; 
    }

    public void AddEffect(EffectTypeEnum.Types effect){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000, LayerMask.NameToLayer("Terrain"))){
            GameObject hitObject = hit.collider.gameObject;
            if(hitObject.CompareTag("Tile") && hitObject.GetComponent<EditorTile>().effect != EffectTypeEnum.Types.Start){
                if(hitObject.GetComponent<EditorTile>().children.Count > 0){
                    hitObject.GetComponent<EditorTile>().effect = effect;
                }
            }
        }
    }
}
