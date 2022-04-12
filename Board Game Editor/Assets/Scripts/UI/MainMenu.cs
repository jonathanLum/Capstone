using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button newBoard;
    public GameObject boardOptions;
    public GameObject boardSelect;
    public Slider boardID;
    public Text boardText;
    public GameObject playerSelect;

    GameObject saveCtrl;
    
    // Start is called before the first frame update
    void Start()
    {
        saveCtrl = GameObject.FindGameObjectWithTag("SaveController");
        boardID.maxValue = saveCtrl.GetComponent<SaveController>().so.saveData.Count - 1;
        boardID.onValueChanged.AddListener(SetBoardID);
        SetBoardID(boardID.value);
        newBoard.onClick.AddListener(saveCtrl.GetComponent<SaveController>().ResetBoardId);
    }

    void SetBoardID(System.Single id){
        saveCtrl.GetComponent<SaveController>().currBoardID = (int)id;
        boardText.text = saveCtrl.GetComponent<SaveController>().so.saveData[(int)id].name;
    }


    public void OpenNewBoard(){
        SceneManager.LoadScene("BoardEditor", LoadSceneMode.Single);
       
    }

    public void OpenBoardOptions(){
        boardOptions.SetActive(true);
    }

    public void OpenBoardSelect(){
        boardSelect.SetActive(true);
    }

    public void OpenPlayerSelect(){
        playerSelect.SetActive(true);
    }

    public void BackButton(){
        boardOptions.SetActive(false);
        boardSelect.SetActive(false);
        playerSelect.SetActive(false);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
