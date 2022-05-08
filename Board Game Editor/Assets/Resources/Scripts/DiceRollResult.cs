using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollResult : MonoBehaviour
{
    public Dice dice;
    public int number;
    
    private void OnTriggerStay(Collider other) {
        switch(other.gameObject.name){
            case "1":
                number = 6;
                break;
            case "2":
                number = 5;
                break;
            case "3":
                number = 4;
                break;
            case "4":
                number = 3;
                break;
            case "5":
                number = 2;
                break;
            case "6":
                number = 1;
                break;
        }
    }
}
