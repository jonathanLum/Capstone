using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textPlayerCount;
    public int playerCount;
    public List<GameObject> pieceSelectors;
    public GameObject playerCountSelect;

    // Start is called before the first frame update
    void Start()
    {
        playerCount = 4;
        SetPlayerCount(playerCount);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ChangeDisplay(int value)
    {
        switch (value)
        {
            case 1:
                pieceSelectors[0].SetActive(true);
                pieceSelectors[1].SetActive(false);
                pieceSelectors[2].SetActive(false);
                pieceSelectors[3].SetActive(false);
                break;
            case 2:
                pieceSelectors[0].SetActive(true);
                pieceSelectors[1].SetActive(true);
                pieceSelectors[2].SetActive(false);
                pieceSelectors[3].SetActive(false);
                break;
            case 3:
                pieceSelectors[0].SetActive(true);
                pieceSelectors[1].SetActive(true);
                pieceSelectors[2].SetActive(true);
                pieceSelectors[3].SetActive(false);
                break;
            case 4:
                pieceSelectors[0].SetActive(true);
                pieceSelectors[1].SetActive(true);
                pieceSelectors[2].SetActive(true);
                pieceSelectors[3].SetActive(true);
                break;
        }

    }

    public void DecrementPlayerCount()
    {
        if (playerCount > 1)
        {
            SetPlayerCount(playerCount - 1);
        }
    }

    public void IncrementPlayerCount()
    {
        if (playerCount < 4)
        {
            SetPlayerCount(playerCount + 1);
        }
    }

    public void SetPlayerCount(int value)
    {
        playerCount = value;
        textPlayerCount.text = value.ToString();
        ChangeDisplay((int)value);
    }
}
