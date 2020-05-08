/*
    @author Caleb Hardy

    Controls the game time
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using System;

public class GameTime : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject timeObject;
    public GameObject mainCamera;
    private string timeText;
    private Cooldown timeCooldown;


    void Awake() {
        timeCooldown = gameObject.AddComponent<Cooldown>();
    }
    void Start()
    {
        timeText = timeObject.GetComponent<UnityEngine.UI.Text>().text;
        timeCooldown.startCooldown(float.Parse(timeText));
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera.GetComponent<PlayerWin>().gameOver) {
            timeCooldown.cancel();
            return;
        }
        
        timeObject.GetComponent<UnityEngine.UI.Text>().text = Convert.ToInt32(timeCooldown.getCurrentTime()).ToString();
        if (!timeCooldown.active()) {
            mainCamera.GetComponent<PlayerWin>().timeoutWin();
        }
    }
}
