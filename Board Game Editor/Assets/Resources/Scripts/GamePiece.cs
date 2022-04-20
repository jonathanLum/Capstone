using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public bool moving = true;
    [SerializeField] private GameObject model;
    [SerializeField]private float distance = 20;
    [SerializeField]private float speed = 2;
    void Update()
    {
        if(moving){
            // animate
            transform.rotation = Quaternion.Euler(0f, 0f, distance * Mathf.Sin(Time.time * speed));
            
        }
    }
}
