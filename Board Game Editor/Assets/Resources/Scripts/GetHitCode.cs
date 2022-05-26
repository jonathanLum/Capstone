using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitCode : MonoBehaviour
{
    public void playGetHit()
    {
        SFXController.SFXInstance.Audio.PlayOneShot(SFXController.SFXInstance.playerHit);
    }
}
