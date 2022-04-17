using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button newBoard;
    // public GameObject boardOptions;
    // public GameObject boardSelect;
    public Slider boardID;
    public Text boardText;
    // public GameObject playerSelect;

    public Button buttonEditorOptions;
    public Button buttonLaunchEditor;
    public GameObject menuTop;
    public GameObject menuEditorOptions;
    public GameObject menuEditorLoadBoard;
    public GameObject menuPlayerSelect;
    public GameObject menuBoardSelect;

    public Stack<GameObject> menuStack = new Stack<GameObject>();
    public SaveObject saveObject;
    public GameObject saveCtrl;
    public SceneController sceneCtrl;
    private List<GameBoard> boardList;




    // Start is called before the first frame update
    void Start()
    {
        saveCtrl = GameObject.FindGameObjectWithTag("SaveController");
        sceneCtrl = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();

        // boardID.maxValue = saveCtrl.GetComponent<SaveController>().so.saveData.Count - 1;
        // boardID.onValueChanged.AddListener(SetBoardID);
        // SetBoardID(boardID.value);
        // newBoard.onClick.AddListener(saveCtrl.GetComponent<SaveController>().ResetBoardId);
        initMenus();
    }

    private void initMenus()
    {
        menuEditorOptions.SetActive(false);
        menuEditorLoadBoard.SetActive(false);
        menuPlayerSelect.SetActive(false);
        menuBoardSelect.SetActive(false);
        PushMenu(menuTop);
    }

    public void PushMenu(GameObject newMenu)
    {
        if (menuStack.Count > 0)
        {
            menuStack.Peek().SetActive(false);
        }

        menuStack.Push(newMenu);
        menuStack.Peek().SetActive(true);
    }

    public void PopMenu()
    {
        menuStack.Pop().SetActive(false);
        menuStack.Peek().SetActive(true);
    }

    public void OpenMenuEditorOptions()
    {
        PushMenu(menuEditorOptions);
    }

    public void OpenMenuEditorLoadBoard()
    {
        PushMenu(menuEditorLoadBoard);
    }

    public void OpenEditorNewBoard()
    {
        saveCtrl.GetComponent<SaveController>().ResetBoardId();
        sceneCtrl.GoToEditor();
    }

    public void OpenEditorExistingBoard()
    {
        sceneCtrl.GoToEditor();
    }

    public void OpenMenuPlayerSelect()
    {
        PushMenu(menuPlayerSelect);
    }

    public void OpenMenuBoardSelect()
    {
        PushMenu(menuBoardSelect);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        sceneCtrl.GoToPlayGame();
    }
}
