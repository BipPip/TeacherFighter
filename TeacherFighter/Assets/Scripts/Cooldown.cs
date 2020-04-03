using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour
{

    public delegate void Delegate(); // This defines what type of method you're going to call.
    private Delegate m_methodToCall; // This is the variable holding the method you're going to call.

    private bool usingDelegate;
    private bool intial = true;
    private bool methodCalled = false;
    private bool cooldownTimerActive;                 //Is this timer active?

    private float cooldownTimer = 0;                 //Time left on timer, can be used at 0


    // Update is called once per frame
    void FixedUpdate()
    {

        if(this.cooldownTimerActive)
            this.cooldownTimer -= Time.fixedDeltaTime;    //Subtract the time since last frame from the timer.
        if (this.cooldownTimer < 0) 
        {
            this.cooldownTimer = 0;                  //If timer is less than 0, reset it to 0 as we don't want it to be negative
            this.cooldownTimerActive = false;

            if (this.usingDelegate && !this.methodCalled) 
            {
                this.intial = false;
                this.methodCalled = true;
                this.m_methodToCall();
            }
            
            
        }

        if (!this.intial && this.methodCalled) 
        {
            this.intial = true;
            this.methodCalled = false;
            this.usingDelegate = false;
        }
        
    }

    public void startCooldown(/*Delegate method,*/ float cooldown) 
    {
        this.cooldownTimer = cooldown;
        this.cooldownTimerActive = true;
        this.usingDelegate = false;
    }
    
    public void startCooldown(Delegate method, float cooldown) 
    {
        this.cooldownTimer = cooldown;
        this.cooldownTimerActive = true;
        this.m_methodToCall = method;
        this.usingDelegate = true;
        this.intial = true;

    }

    public bool active()
    {
        return this.cooldownTimerActive;
    }

    public float getCurrentTime() 
    {
        return this.cooldownTimer;
    }

    public bool isInitial() 
    {
        return this.intial;
    }

    public void cancel() {
        this.usingDelegate = false;
        this.cooldownTimerActive = false;
        this.cooldownTimer = 0;
    }

}
