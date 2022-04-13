using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

public class SaveController : MonoBehaviour
{
    public SaveObject so;
    //public GameBoard gameBoard;
    public int currBoardID = 0;
    
    public EditorController ctrl;
    public SceneController sceneCtrl;


    [SerializeField] GameObject editorTilePrefab;
    [SerializeField] GameObject playTilePrefab;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        sceneCtrl = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        SaveStruct.Load(so);
        currBoardID = so.saveData.Count;
        //SaveStruct.Save(so);
        SceneManager.activeSceneChanged += ChangeScene;
    }

    public void ResetBoardId(){
        currBoardID = so.saveData.Count;
    }

    void ChangeScene(Scene current, Scene next){
        if(next == SceneManager.GetSceneByName("BoardEditor")){
            ctrl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<EditorController>();
            
            if(currBoardID == so.saveData.Count){
                // New board
                GameBoard newBoard = new GameBoard();
                so.saveData.Add(newBoard);
                //Debug.Log(so.saveData.Count);
                SaveBoard("default");
                //Debug.Log("add board");
            }else{
                // Load board
                LoadBoard();
            }
        }else if(next == SceneManager.GetSceneByName("PlayGame")){
            LoadBoard();
        }
    }

    public void SaveBoard(string name){
        BoardToSO(name);
        SaveStruct.Save(so);
    }

    void BoardToSO(string name){
        Debug.Log(so.saveData.Count);
        so.saveData[currBoardID].board.Clear();

        so.saveData[currBoardID].name = name;
        
        foreach(GameObject tile in ctrl.allTiles){
            DataTransferObject dto = new DataTransferObject();
            
            dto.position = tile.transform.position;
            dto.effect = tile.GetComponent<EditorTile>().effect;
            dto.effectValue = tile.GetComponent<EditorTile>().effectValue;
            #nullable enable
            GameObject? parentObj = tile.GetComponent<EditorTile>().parent;
            #nullable disable
            if(parentObj != null){
                dto.parent = ctrl.allTiles.IndexOf(parentObj);
            }else{
                dto.parent = -1;
            }
            
            /*void AddChildren(DataTransferObject dto){
                var childList = tile.GetComponent<EditorTile>().children;
                foreach(GameObject child in childList){
                dto.children.Add(ctrl.allTiles.IndexOf(child));
                }
            }
            AddChildren(dto);*/
            
            so.saveData[currBoardID].board.Add(dto);
            //Debug.Log("tile added to board");
        }
        
    }

    
    public void LoadBoard(){
        SaveStruct.Load(so);
        
        if(SceneManager.GetActiveScene().name == "BoardEditor"){
            if(so.saveData[currBoardID] != null){
                SOToEditBoard();
            }
        }else if(SceneManager.GetActiveScene().name == "PlayGame"){
            SOToPlayBoard();
        }
    }

    void SOToEditBoard(){
        foreach(GameObject tile in ctrl.allTiles.ToArray()){
            ctrl.allTiles.Remove(tile);
            Destroy(tile);
        }

        foreach(DataTransferObject tile in so.saveData[currBoardID].board){
            GameObject newTile = Instantiate(editorTilePrefab, tile.position, Quaternion.identity);
            newTile.GetComponent<EditorTile>().effect = tile.effect;
            newTile.GetComponent<EditorTile>().effectValue = tile.effectValue;
            if(tile.parent == -1){
                newTile.GetComponent<EditorTile>().parent = null;
            }else{
                newTile.GetComponent<EditorTile>().parent = ctrl.allTiles[tile.parent];
            }
            // children
            ctrl.allTiles.Add(newTile);
        }
    }

    void SOToPlayBoard(){
        foreach(DataTransferObject tile in so.saveData[currBoardID].board){
            // make play tile and set values
        }
    }
}
