using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TextTimer : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private char characterSpliter = ':';

    [Header("Logic")]
    private float timer;
    private bool isActive;
    #region Singleton implementation
    public static TextTimer Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    public void Update()
    {
        if(timer<0)
        {
            isActive=!isActive;
            Debug.Log("het gio");
            
        }
        else if (isActive)
        {
            timer -= Time.deltaTime;
            UpdateText();
        }
    }
    private void UpdateText()
    {
        // Get the amount time since start
        float seconds = (timer % 60);
        float minutes = ((int)(timer / 60) % 60);
        float hours = (int)(timer / 3600);
        text.text =minutes.ToString("00") + characterSpliter + seconds.ToString("00");
    }

    //public void StartTimer()
    //{
    //    StartTimer(0);
    //}
    public void StartTimer(float seconds)
    {
        isActive = true;
        timer = seconds;
        UpdateText();
    }

    public bool Stop()
    {
        if(timer>0)
        {
            return false;
        }
        return true;
    }
    public void AddTime(float seconds)
    {
        timer += seconds;
        UpdateText();
    }
    public void PauseTimer()
    {
        isActive = !isActive;
    }
    public void ResetTimer()
    {
        timer = 90;
        UpdateText();
    }

    public float GetTimeSinceStart()
    {
        return timer;
    }
}
