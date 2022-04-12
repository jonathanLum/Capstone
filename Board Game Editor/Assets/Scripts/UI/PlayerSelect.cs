using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public Text playerCount;
    public List<GameObject> pieceSelectors;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeDisplay(int value){
        switch(value){
            case 2:
                pieceSelectors[2].SetActive(false);
                pieceSelectors[3].SetActive(false);
                break;
            case 3:
                pieceSelectors[2].SetActive(true);
                pieceSelectors[3].SetActive(false);
                break;
            case 4:
                pieceSelectors[2].SetActive(true);
                pieceSelectors[3].SetActive(true);
                break;
        }
            
    }

    public void UpdateCount(System.Single value){
        playerCount.text = value.ToString();
        ChangeDisplay((int)value);
    }
}
