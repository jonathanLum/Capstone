using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject saveCtrl;
    public SceneController sceneCtrl;
    public GameObject menuTop;
    public GameObject menuEditorOptions;
    public GameObject menuEditorLoadBoard;
    public GameObject menuPlayerSelect;
    public GameObject menuBoardSelect;

    public BoardList boardListEdit;
    public BoardList boardListPlay;

    public Stack<GameObject> menuStack = new Stack<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        saveCtrl = GameObject.FindGameObjectWithTag("SaveController");
        sceneCtrl = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();

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
        if(boardListEdit.boardSelected)
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
        if(boardListPlay.boardSelected)
            sceneCtrl.GoToPlayGame();
    }
}
