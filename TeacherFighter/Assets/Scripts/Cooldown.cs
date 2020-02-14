using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour
{

    private delegate void Delegate(); // This defines what type of method you're going to call.
    private Delegate m_methodToCall; // This is the variable holding the method you're going to call.

    private bool cooldownTimerActive;                 //Is this timer active?
    //private float regenCooldown;              //How often this cooldown may be used
    private float cooldownTimer;                 //Time left on timer, can be used at 0


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(this.cooldownTimerActive)
            this.cooldownTimer -= Time.deltaTime;    //Subtract the time since last frame from the timer.
        if (this.cooldownTimer < 0) {
            this.cooldownTimer = 0;                  //If timer is less than 0, reset it to 0 as we don't want it to be negative
            this.cooldownTimerActive = false;
            //this.m_methodToCall();
        }
        
    }

    public void startCooldown(/*Delegate method,*/ float cooldown) {
        this.cooldownTimer = cooldown;
        this.cooldownTimerActive = true;
        //this.m_methodToCall = method;
    }

    public bool active() {
        return this.cooldownTimerActive;
    }
}
