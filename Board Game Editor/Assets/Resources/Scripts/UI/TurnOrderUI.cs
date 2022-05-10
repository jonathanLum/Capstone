using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TurnOrderUI : MonoBehaviour
{
    public GameManager gameManager;
    GameDataController gameData;
    List<Player> players;

    public Transform listPanel;
    public GameObject listToken;

    List<GameObject> buttons = new List<GameObject>();

    public float offset = 6f;
    public float startY = -50.3f;

    private static int maxDisplay = 6;

    int startIndex = 0;

    void Start()
    {
        gameData = GameObject.FindGameObjectWithTag("GameDataController").GetComponent<GameDataController>();
        players = new List<Player>(gameManager.players);
        UpdateDisplay();
    }

    void UpdateDisplay(){
        // Clear old display
        foreach(GameObject button in buttons){
            Destroy(button);
        }
        buttons.Clear();

        // Make new display
        Vector3 position = new Vector3(0,startY,0);
        for(int i = startIndex; i < maxDisplay+startIndex; i++){
            // spawn buttons
            int buttonID = i%gameManager.numberOfPlayers;
            GameObject listTokenItem = Instantiate(listToken, new Vector3(0, 0, 0), Quaternion.identity, listPanel);
            buttons.Add(listTokenItem);

            listTokenItem.GetComponent<RectTransform>().anchoredPosition = position;

            ColorBlock cb = listTokenItem.GetComponent<Button>().colors;
            Color color = gameData.pieceColors[buttonID].color;
            cb.highlightedColor = color;
            float H, S, V;
            Color.RGBToHSV(color, out H, out S, out V);
            S = 0.2f;
            color = Color.HSVToRGB(H, S, V);
            cb.normalColor = color;
            listTokenItem.GetComponent<Button>().colors = cb;

            GameObject listTokenItemText = listTokenItem.transform.GetChild(0).gameObject;
            TMPro.TextMeshProUGUI text = listTokenItemText.GetComponent<TMPro.TextMeshProUGUI>();
            text.text = "Player " + (players[buttonID].ID+1);

            position.y -= listTokenItem.GetComponent<RectTransform>().sizeDelta.y + offset;

            // allow attacking players with button press
            GameObject listTokenItemX = listTokenItem.transform.GetChild(1).gameObject;
            if(gameManager.players[buttonID].skipNextTurn && (i - startIndex) < gameManager.numberOfPlayers)
                listTokenItemX.SetActive(true);
            listTokenItem.GetComponent<Button>().onClick.AddListener(() => { if(gameManager.attacking && gameManager.currentTurn != buttonID)
                                                                                {gameManager.players[buttonID].skipNextTurn = true; 
                                                                                listTokenItemX.SetActive(true); 
                                                                                gameManager.attacking = false;}
                                                                            EventSystem.current.SetSelectedGameObject(null); });
        }
    }

    public void ShiftList(){
        startIndex += 1;
        UpdateDisplay();
    }

    public void Disable(){
        gameObject.SetActive(false);
    }
}
