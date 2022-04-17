using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardList : MonoBehaviour
{
    public GameObject saveCtrl;
    public GameObject buttonListItem;
    private List<GameBoard> boardList;

    // Start is called before the first frame update
    void Start()
    {
        saveCtrl = GameObject.FindGameObjectWithTag("SaveController");
        boardList = saveCtrl.GetComponent<SaveController>().so.saveData;
        populateBoardList();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void populateBoardList()
    {

        Vector3 position = Vector3.zero;
        foreach (GameBoard board in boardList)
        {
            // Create a new list item for the board, set as parent to the scrollview content
            Transform scrollViewContent = GameObject.Find("Content").transform;
            GameObject boardListItem = Instantiate(buttonListItem, new Vector3(0, 0, 0), Quaternion.identity, scrollViewContent);
            // UI elements use the RectTransform component instead of Transform
            boardListItem.GetComponent<RectTransform>().localPosition = position;


            GameObject boardListItemText = boardListItem.transform.GetChild(0).gameObject;
            Debug.Log(boardListItemText);
            TMPro.TextMeshProUGUI text = boardListItemText.GetComponent<TMPro.TextMeshProUGUI>();
            text.text = board.name;

            boardListItem.SetActive(true);

            position.y -= boardListItem.GetComponent<RectTransform>().sizeDelta.y;

        }
    }
}
