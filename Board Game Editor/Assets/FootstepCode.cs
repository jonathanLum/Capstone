using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepCode : MonoBehaviour
{
    public void playFootstep()
    {
        SFXController.SFXInstance.Audio.PlayOneShot(SFXController.SFXInstance.playerStep);
    }
}
