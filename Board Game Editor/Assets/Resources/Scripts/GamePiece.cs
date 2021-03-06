using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public bool moving = true;
    [SerializeField] private GameObject model;
    public Animator anim;


    void Update()
    {
        if(moving){
            // animate
            //model.transform.localRotation = Quaternion.Euler(0f, 0f, distance * Mathf.Sin(Time.time * speed));
            anim.SetBool("Walking", true);
            
        }else{
            // lerp to zero
            //model.transform.localRotation = Quaternion.RotateTowards(model.transform.localRotation, Quaternion.identity, rotateSpeed * Time.deltaTime);
            anim.SetBool("Walking", false);
        }
    }
}
