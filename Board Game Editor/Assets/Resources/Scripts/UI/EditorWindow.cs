using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorWindow : MonoBehaviour
{
    public TMP_InputField boardName;
    public Button saveButton;
    public Button reloadButton;
    public Button quitButton;

    GameObject saveCtrl;
    
    // Start is called before the first frame update
    void Start()
    {
        saveCtrl = GameObject.FindGameObjectWithTag("SaveController");

        saveButton.onClick.AddListener(delegate{saveCtrl.GetComponent<SaveController>().SaveBoard(boardName.text);});
        reloadButton.onClick.AddListener(saveCtrl.GetComponent<SaveController>().LoadBoard);
        quitButton.onClick.AddListener(PromptQuit);

        boardName.text = saveCtrl.GetComponent<SaveController>().GetCurrentBoard().name;
    }

    void PromptQuit(){
        Debug.Log("Unsaved progress will be lost");
        GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().GoToMainMenu();
    }
}
