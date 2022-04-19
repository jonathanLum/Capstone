using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

public class SaveController : MonoBehaviour
{
    public SaveObject so;
    public int currBoardID = 0;

    public EditorController ctrl;
    public GameManager gameManager;
    public SceneController sceneCtrl;


    [SerializeField] GameObject editorTilePrefab;
    [SerializeField] GameObject playTilePrefab;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        sceneCtrl = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        SaveStruct.Load(so);
        currBoardID = so.saveData.Count;
        SceneManager.activeSceneChanged += ChangeScene;
    }

    public void ResetBoardId()
    {
        currBoardID = so.saveData.Count;
    }

    void ChangeScene(Scene current, Scene next)
    {
        if (next == SceneManager.GetSceneByName("BoardEditor"))
        {
            ctrl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<EditorController>();

            if (currBoardID == so.saveData.Count)
            {
                // New board
                GameBoard newBoard = new GameBoard();
                newBoard.name = "Default";
                var startTile = new DataTransferObject();
                startTile.position = new Vector3(0, 0, 0);
                startTile.effect = EffectTypeEnum.Types.Start;
                startTile.parent = -1;
                newBoard.board.Add(startTile);
                so.saveData.Add(newBoard);
                SaveStruct.Save(so);
            }
            LoadBoard();
        }
        else if (next == SceneManager.GetSceneByName("PlayGame"))
        {
            gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
            LoadBoard();
        }
    }

    public void SaveBoard(string name)
    {
        BoardToSO(name);
        SaveStruct.Save(so);
    }

    void BoardToSO(string name)
    {
        so.saveData[currBoardID].board.Clear();

        so.saveData[currBoardID].name = name;

        foreach (GameObject tile in ctrl.allTiles)
        {
            DataTransferObject dto = new DataTransferObject();
            var childList = tile.GetComponent<EditorTile>().children;
            dto.position = tile.transform.position;
            dto.effect = tile.GetComponent<EditorTile>().effect;
            dto.effectValue = tile.GetComponent<EditorTile>().effectValue;
#nullable enable
            GameObject? parentObj = tile.GetComponent<EditorTile>().parent;
#nullable disable
            if (parentObj != null)
            {
                dto.parent = ctrl.allTiles.IndexOf(parentObj);
            }
            else
            {
                dto.parent = -1;
            }

            foreach(GameObject child in childList){
                dto.children.Add(ctrl.allTiles.IndexOf(child));
            }

            so.saveData[currBoardID].board.Add(dto);
        }

    }


    public void LoadBoard()
    {
        SaveStruct.Load(so);

        if (SceneManager.GetActiveScene().name == "BoardEditor")
        {
            SOToEditBoard();
        }
        else if (SceneManager.GetActiveScene().name == "PlayGame")
        {
            SOToPlayBoard();
        }
    }

    void SOToEditBoard()
    {
        foreach (GameObject tile in ctrl.allTiles.ToArray())
        {
            ctrl.allTiles.Remove(tile);
            Destroy(tile);
        }

        foreach (DataTransferObject tile in so.saveData[currBoardID].board)
        {
            GameObject newTile = Instantiate(editorTilePrefab, tile.position, Quaternion.identity);
            newTile.GetComponent<EditorTile>().effect = tile.effect;
            newTile.GetComponent<EditorTile>().effectValue = tile.effectValue;
            if (tile.parent == -1)
            {
                newTile.GetComponent<EditorTile>().parent = null;
            }
            else
            {
                newTile.GetComponent<EditorTile>().parent = ctrl.allTiles[tile.parent];
            }

            ctrl.allTiles.Add(newTile);
        }
        int i = 0;
        foreach (DataTransferObject tile in so.saveData[currBoardID].board)
        {
            var childList = tile.children;
            foreach(int child in childList){
                ctrl.allTiles[i].GetComponent<EditorTile>().children.Add(ctrl.allTiles[child]);
            }
            i++;
        }
    }


    void SOToPlayBoard()
    {
        foreach (DataTransferObject tile in so.saveData[currBoardID].board)
        {
            GameObject newTile = Instantiate(playTilePrefab, tile.position, Quaternion.identity);
            gameManager.allTiles.Add(newTile);
            newTile.GetComponent<Tile>().effectValue = tile.effectValue;
            switch(tile.effect)
            {
                case EffectTypeEnum.Types.None:
                    newTile.GetComponent<Tile>().effect = null;
                    newTile.GetComponent<Tile>().icon = null;
                    break;
                case EffectTypeEnum.Types.Start:
                    newTile.GetComponent<Tile>().effect = null;
                    newTile.GetComponent<Tile>().icon = (Texture2D)Resources.Load("Textures/StartIcon");
                    break;
                case EffectTypeEnum.Types.End:
                    newTile.GetComponent<Tile>().effect = null;
                    newTile.GetComponent<Tile>().icon = (Texture2D)Resources.Load("Textures/FinishIcon");
                    break;
                case EffectTypeEnum.Types.Move:
                    newTile.GetComponent<Tile>().effect = ScriptableObject.CreateInstance<MoveEffect>();
                    newTile.GetComponent<Tile>().icon = (Texture2D)Resources.Load("Textures/MoveIcon");
                    break;
                case EffectTypeEnum.Types.MoveRandom:
                    newTile.GetComponent<Tile>().effect = ScriptableObject.CreateInstance<MoveRandomEffect>();
                    newTile.GetComponent<Tile>().icon = (Texture2D)Resources.Load("Textures/RandomIcon");
                    break;
                case EffectTypeEnum.Types.Trap:
                    newTile.GetComponent<Tile>().effect = ScriptableObject.CreateInstance<TrapEffect>();
                    newTile.GetComponent<Tile>().icon = (Texture2D)Resources.Load("Textures/TrapIcon");
                    break;
                case EffectTypeEnum.Types.Attack:
                    newTile.GetComponent<Tile>().effect = ScriptableObject.CreateInstance<AttackEffect>();
                    newTile.GetComponent<Tile>().icon = (Texture2D)Resources.Load("Textures/AttackIcon");
                    break;
            }
            if (tile.parent == -1)
            {
                newTile.GetComponent<Tile>().parent = null;
            }
            else
            {
                newTile.GetComponent<Tile>().parent = gameManager.allTiles[tile.parent];
            } 
        }
        int i = 0;
        foreach (DataTransferObject tile in so.saveData[currBoardID].board)
        {
            var childList = tile.children;
            foreach(int child in childList){
                gameManager.allTiles[i].GetComponent<Tile>().children.Add(gameManager.allTiles[child]);
            }
            i++;
        }
    }

    public void SetBoardID(System.Single id)
    {
        currBoardID = (int)id;
    }


    public GameBoard GetCurrentBoard(){
        return so.saveData[currBoardID];
    }
}
