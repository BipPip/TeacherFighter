using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;
using System;


public class RunTimeSpawn : MonoBehaviour
{
   
    /*
        I believe everything can be accessed by the character select script 
        keep in mind it does not have to be added to the scene it gets carried over
        when it goes from chracter select to the level so there should be no visibility 
        issues. Some uses can be found in the character manager script.
    */

   public Transform player1spawn;
   public Transform player2spawn;

   CharacterManager charManager;

   //public List<PlayerInterfaces> plInterfaces = new List<PlayerInterfaces>();

    void Start()
    {
        // everything gets instantiated in here
    }

}
