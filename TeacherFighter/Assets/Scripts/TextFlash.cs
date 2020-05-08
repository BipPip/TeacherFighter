/*
    @author Caleb Hardy

    Added to text object to make it flash
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFlash : MonoBehaviour
{

    private Cooldown timer, offCooldown;
    public bool on = false;
    public float speed = 0.3f;
    private bool visible, usingCount, originalState;
    private int count;
    void Awake() {
        timer = gameObject.AddComponent<Cooldown>();
        offCooldown = gameObject.AddComponent<Cooldown>();
        visible = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (on) {

            if (visible) {
           gameObject.GetComponent<UnityEngine.UI.Text>().enabled = true;
            } else {
                gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
            }

            if (!timer.active())
                timer.startCooldown(toggle, speed);
            
        } else {
            timer.cancel();
           
        }

       
       


    }

    private void toggle() {
        if (visible) {
            visible = false;
            
        } else if (!visible) {
            visible = true;
        }
        if (usingCount)
           
        if (count == 0 && usingCount) {
            turnOf();
        }
         count--;
    }

    public void turnOn() {
        originalState = gameObject.GetComponent<UnityEngine.UI.Text>().enabled;
        on = true;
    }
    public void turnOn(int count) {
        // offCooldown.startCooldown(turnOf, x);
        originalState = gameObject.GetComponent<UnityEngine.UI.Text>().enabled;
        on = true;
        usingCount = true;
        this.count = count * 2;
      
    }

    public void turnOf() {
        on = false;
        gameObject.GetComponent<UnityEngine.UI.Text>().enabled = originalState;
    }
}
