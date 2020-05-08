/*
    @author Tia Flores-Carr

    This makes an object blink its lit
    rally only used for the blinking
    "press any key" thing in the main
    menu 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StartMenuBlink : MonoBehaviour
{

  
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
