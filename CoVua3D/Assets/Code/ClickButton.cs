using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    private AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnMouseDown()
    {
        audioManager.PlayVFX(audioManager.VFXclickClips);
        Destroy(gameObject);
    }
}

