using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;
using System;


public class RunTimeSpawn : CharacterSelect
{
    // Start is called before the first frame update

    string prefabName;

    public GameObject LeftCTaylor;
    //public GameObject LeftCVonderehe;

    public GameObject rightBar, rightHealth, rightStamina, rightCooldown;
    public GameObject leftBar, leftHealth, leftStamina, leftCooldown;

    private PlatformerCharacter2D player1;




    void Start()
    {

        prefabName = getPlayer1();

        Debug.Log("RunTimeSpawn Script" + prefabName);
       
        //GameObject p1Health = Instantiate(leftBar,new Vector3(-10,10.8f,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        //GameObject p1Cooldown = Instantiate(leftCooldown,new Vector3(-3.42f,7.25f,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);

        //GameObject p2Health = Instantiate(rightBar,new Vector3(10,10.8f,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        //GameObject p2Cooldown = Instantiate(rightCooldown,new Vector3(3.42f,7.25f,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);

        player1 = GetComponent<PlatformerCharacter2D>();
        player1.setUI(rightHealth,rightStamina, rightCooldown);
        Instantiate(LeftCTaylor); //maybe in the constructor say what character it is

        //the issue is now that when setting the ui of the character its not saying who it is.
        
    }

}
