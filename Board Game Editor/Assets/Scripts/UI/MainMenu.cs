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
    public GameObject menuTop;
    public GameObject menuEditorOptions;
    public GameObject menuEditorLoadBoard;

    public Stack<GameObject> menuStack = new Stack<GameObject>();

    GameObject saveCtrl;



    // Start is called before the first frame update
    void Start()
    {
        saveCtrl = GameObject.FindGameObjectWithTag("SaveController");
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
    void SetBoardID(System.Single id)
    {
        saveCtrl.GetComponent<SaveController>().currBoardID = (int)id;
        boardText.text = saveCtrl.GetComponent<SaveController>().so.saveData[(int)id].name;
    }

    public void OpenMenuEditorOptions()
    {
        PushMenu(menuEditorOptions);
    }

    public void OpenMenuEditorLoadBoard()
    {
        PushMenu(menuEditorLoadBoard);
    }

    public void OpenNewBoard()
    {
        saveCtrl.GetComponent<SaveController>().ResetBoardId();
        SceneManager.LoadScene("BoardEditor", LoadSceneMode.Single);

    }

    // public void OpenBoardOptions(){
    //     boardOptions.SetActive(true);
    // }

    // public void OpenBoardSelect(){
    //     boardSelect.SetActive(true);
    // }

    // public void OpenPlayerSelect(){
    //     playerSelect.SetActive(true);
    // }

    // public void BackButton(){
    //     boardOptions.SetActive(false);
    //     boardSelect.SetActive(false);
    //     playerSelect.SetActive(false);
    // }

    public void QuitGame()
    {
        Application.Quit();
    }
}
