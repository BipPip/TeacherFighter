/*
    @author Caleb Hardy

    Controls player stamina
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Stamina : MonoBehaviour
{

    private SimpleHealthBar staminaBar;
    
    private bool regenTimerActive;                 //Is this timer active?
    private bool regenActive = false;

    private float regenTimer;                 //Time left on timer, can be used at 0
    private float regen;
    private float regenInc;

    // Start is called before the first frame update
    void Start()
    {
        this.staminaBar = gameObject.GetComponent<PlatformerCharacter2D>().staminaBarObject.GetComponent<SimpleHealthBar>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Before regen starts
        if(this.regenTimerActive)
            this.regenTimer -= Time.deltaTime;    //Subtract the time since last frame from the timer.
        if (this.regenTimer < 0) 
        {
            this.regenTimer = 0;                  //If timer is less than 0, reset it to 0 as we don't want it to be negative
            this.regenTimerActive = false;
            this.startRegen();
        }

        // While regen is active
        if(this.regenActive) 
        {
            this.regen -= Time.deltaTime;  //Subtract the time since last frame from the timer.
        }  

        if (this.regen < 0) 
        {
            this. regen = 0;                  //If timer is less than 0, reset it to 0 as we don't want it to be negative
            this.staminaBar.UpdateBar(getStamina() + regenInc, 100);
            this.regenInc += 1;
            if(this.getStamina() == 100f) 
            {
                this.regenActive = false;
            }
            else 
            {
                this.regen = 0.5f;
            }
        }
    }

    public float getStamina() 
    {
        return this.staminaBar.GetCurrentFraction * 100;
    }
    public void startCountdown(float regenCooldown) 
    {
        this.regenTimerActive = true;
        this.regenActive = false;
        this.regenTimer = regenCooldown;
        this.regenActive = false;
        
    }

    public void startRegen() 
    {
        this.regenActive = true;
        this.regen = 0.5f;
        this.regenInc = 5;
    }

    public void staminaDecrease(float decrease) 
    {
        this.staminaBar.UpdateBar(getStamina() - decrease, 100);
    }
}
