using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public AudioSource MusicaudioSource, VFXaudioSource;
    public AudioClip MusicClips, VFXclickClips;

    void Start()
    {
        //PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (!FireBase.isLoginSignupPage)
        {
            MusicaudioSource.clip = MusicClips;
            MusicaudioSource.Play();
        }
    }

    public void PlayVFX(AudioClip clickButton)
    {
        VFXaudioSource.clip = clickButton;
        VFXaudioSource.PlayOneShot(clickButton);
    }
}


