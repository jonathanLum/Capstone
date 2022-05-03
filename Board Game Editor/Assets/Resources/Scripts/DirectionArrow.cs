using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DirectionArrow : MonoBehaviour
{
    public GameManager gameManager;
    public UnityEvent clicked;


    private void OnMouseDown() {
        clicked.Invoke();
        GetComponent<Renderer>().material.SetInt("_Hovering", 0);
        //Debug.Log("clicked"); 
    }

    private void OnMouseEnter() {
        GetComponent<Renderer>().material.SetInt("_Hovering", 1);
        //Debug.Log("hover"); 
    }

    private void OnMouseExit() {
        GetComponent<Renderer>().material.SetInt("_Hovering", 0);
        //Debug.Log("no hover"); 
    }
}
