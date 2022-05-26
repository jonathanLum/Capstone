using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    public void Clicked()
    {
        SFXController.SFXInstance.Audio.PlayOneShot(SFXController.SFXInstance.Click);
    }
}
