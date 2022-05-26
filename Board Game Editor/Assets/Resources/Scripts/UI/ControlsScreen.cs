using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsScreen : MonoBehaviour
{
    public void ToggleDisplay(){
        if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }else{
            gameObject.SetActive(true);
        }
    }
}
