using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardList : MonoBehaviour
{
    public SaveController saveCtrl;
    public GameObject buttonListItem;
    private List<GameBoard> boardList;
    public bool boardSelected = false;
    public int currIndex;

    // Start is called before the first frame update
    void Start()
    {
        saveCtrl = GameObject.FindGameObjectWithTag("SaveController").GetComponent<SaveController>();
        boardList = saveCtrl.so.saveData;
        populateBoardList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void populateBoardList()
    {

        // Create a new list item for the board, set as parent to the scrollview content
        Transform scrollViewContent = GameObject.Find("Content").transform;

        Vector3 position = Vector3.zero;

        foreach (GameBoard board in boardList)
        {


            GameObject boardListItem = Instantiate(buttonListItem, new Vector3(0, 0, 0), Quaternion.identity, scrollViewContent);

            // UI elements use the RectTransform component instead of Transform
            boardListItem.GetComponent<RectTransform>().localPosition = position;

            // Component containing the text data to update is TMPro.TextMeshProGUI
            // https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/index.html
            GameObject boardListItemText = boardListItem.transform.GetChild(0).gameObject;
            TMPro.TextMeshProUGUI text = boardListItemText.GetComponent<TMPro.TextMeshProUGUI>();
            text.text = board.name;

            boardListItem.GetComponent<Button>().onClick.AddListener(() => { updateCurrentBoard(boardListItem); });
            boardListItem.SetActive(true);
            position.y -= boardListItem.GetComponent<RectTransform>().sizeDelta.y;
        }
    }

    public void updateCurrentBoard(GameObject boardListItem)
    {

        // determine index of the list based on calculating the difference
        // between this button and the distance from top of the container.
        RectTransform rt = boardListItem.GetComponent<RectTransform>();
        Transform scrollViewContent = GameObject.Find("Content").transform;
        float spacing = scrollViewContent.GetComponent<VerticalLayoutGroup>().spacing;

        currIndex = (int)Math.Abs(rt.anchoredPosition.y / (rt.sizeDelta.y + spacing));
        Debug.Log(currIndex);
        saveCtrl.SetBoardID(currIndex);

        boardSelected = true;
    }
}
