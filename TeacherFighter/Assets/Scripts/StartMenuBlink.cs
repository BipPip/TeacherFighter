﻿using System.Collections;
using System.Collections.Generic;
 using UnityEngine.UI;
using UnityEngine;

public class StartMenuBlink : MonoBehaviour
{
    
    // this is the UI.Text or other UI element you want to toggle
    public MaskableGraphic imageToToggle;
 
    public float interval = 1f;
    public float startDelay = 0.5f;
    public bool currentState = true;
    public bool defaultState = true;
    bool isBlinking = false;
 
 
    void Start()
    {
        imageToToggle.enabled = defaultState;
        StartBlink();
    }
 
    public void StartBlink()
    {
        // do not invoke the blink twice - needed if you need to start the blink from an external object
        if (isBlinking)
            return;
 
        if (imageToToggle !=null)
        {
            isBlinking = true;
            InvokeRepeating("ToggleState", startDelay, interval);
        }
    }
 
    public void ToggleState()
    {
        imageToToggle.enabled = !imageToToggle.enabled;
 
    }
}
