using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public AudioSource Audio;
    public AudioClip Click;
    public AudioClip diceShake;
    public AudioClip diceCollision;
    public AudioClip playerStep;
    public AudioClip playerHit;

    public static SFXController SFXInstance;

    private void Awake()
    {
        if (SFXInstance != null && SFXInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        SFXInstance = this;
        DontDestroyOnLoad(this);
    }

    public void playClick()
    {
        SFXInstance.Audio.PlayOneShot(Click);
    }

    
    public void playFootstep()
    {
        SFXInstance.Audio.PlayOneShot(playerStep);
    }

        public void playGetHit()
    {
        SFXInstance.Audio.PlayOneShot(playerHit);
    }
}
