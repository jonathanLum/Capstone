using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public bool moving = true;
    [SerializeField] private GameObject model;
    [SerializeField]private float distance = 20;
    [SerializeField]private float speed = 2;
    [SerializeField]private float rotateSpeed = 20;
    void Update()
    {
        if(moving){
            // animate
            model.transform.localRotation = Quaternion.Euler(0f, 0f, distance * Mathf.Sin(Time.time * speed));
            
        }else{
            // lerp to zero
            model.transform.localRotation = Quaternion.RotateTowards(model.transform.localRotation, Quaternion.identity, rotateSpeed * Time.deltaTime);
        }
    }
}
