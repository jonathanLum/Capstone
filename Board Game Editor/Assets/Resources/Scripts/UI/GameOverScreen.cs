using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text text;

    public void Display(int id){
        text.text = "Player " + id + " Wins!";
        gameObject.SetActive(true);
    }
}
