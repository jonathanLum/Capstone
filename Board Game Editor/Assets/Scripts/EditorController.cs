using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EditorController : MonoBehaviour
{
    public Vector2Int boardLimits = new Vector2Int(5,5);
    
    public Vector3 mouseLoc;
    public Vector3 snapPos;

    Vector3 dragStartLoc;
    Vector3 p1;
    Vector3 dragEndLoc;
    bool dragging = false;

    [SerializeField] GameObject tilePrefab;

    public List<GameObject> selection;
    public List<GameObject> allTiles;
    
    // Start is called before the first frame update
    void Start()
    {
        allTiles.Add(GameObject.FindGameObjectWithTag("Start"));
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

    void ChangeSelection(GameObject obj){
        if(obj.CompareTag("Start") || obj.CompareTag("Tile") || obj.CompareTag("End")){
            if(selection.Contains(obj)){
                selection.Remove(obj);
            }else{
                selection.Add(obj);
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
                    selection.Add(tile);
                }
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
        foreach(GameObject tile in allTiles){
            if(tile.transform.position == snapPos){
                return false;
            }
        }
        // Check outside board limits
        if(snapPos.x < -boardLimits.x || boardLimits.x < snapPos.x || snapPos.z < -boardLimits.y || boardLimits.y < snapPos.z)
            return false;
        
        // Check too many neighbors ignoring lastTile


        // Check if diagonal to lastTile

        

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

    void DeleteSelection(){
        foreach(GameObject tile in selection.ToArray()){
            if(!tile.CompareTag("Start")){
                selection.Remove(tile);
                allTiles.Remove(tile);
                #nullable enable
                GameObject? parent = tile.GetComponent<EditorTile>().parent;
                if(parent != null)
                    parent.GetComponent<EditorTile>().children.Remove(tile);
                #nullable disable
                Destroy(tile);
            }
        }
    }

    public void AddEffect(EffectTypeEnum.Types effect){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000, LayerMask.NameToLayer("Terrain"))){
            GameObject hitObject = hit.collider.gameObject;
            if(hitObject.CompareTag("Tile")){
                hitObject.GetComponent<EditorTile>().effect = effect;
                hitObject.GetComponent<EditorTile>().SetColor();
            }
        }
        //Debug.Log("Add Effect");
    }
}
