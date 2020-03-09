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

    public GameObject CTaylor;
    public GameObject CVonderehe;

    public GameObject rightBar;
    public GameObject leftBar;

    public GameObject leftCooldown;
    public GameObject rightCooldown;

    private PlatformerCharacter2D player1;



    void Start()
    {

        prefabName = getPlayer1();

        Debug.Log("RunTimeSpawn Script" + prefabName);
       
        GameObject p1Health = Instantiate(leftBar,new Vector3(-10,10.8f,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        GameObject p1Cooldown = Instantiate(leftCooldown,new Vector3(-3.42f,7.25f,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);

        GameObject p2Health = Instantiate(rightBar,new Vector3(10,10.8f,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        GameObject p2Cooldown = Instantiate(rightCooldown,new Vector3(3.42f,7.25f,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);

       // if(prefabName.Equals("CharacterTaylor")){
        player1 = GetComponent<PlatformerCharacter2D>();

        Instantiate(CTaylor);
        player1.setUI(rightBar, rightCooldown);
        

        //}else if(prefabName.Equals("CharacterVonDerEhe")){
            //Instantiate(CVonderehe);
       // }
    }

}
