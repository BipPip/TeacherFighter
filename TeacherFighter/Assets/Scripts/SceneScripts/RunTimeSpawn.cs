using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class RunTimeSpawn : CharacterSelect
{
    // Start is called before the first frame update

    string prefabName;

    public GameObject CTaylor;
    public GameObject CVonderehe;


    void Start()
    {

        prefabName = getPlayer1();

        Debug.Log("RunTimeSpawn Script" + prefabName);
       
        if(prefabName.Equals("CharacterTaylor")){
            Instantiate(CTaylor);
        }else if(prefabName.Equals("CharacterVonDerEhe")){
            Instantiate(CVonderehe);
        }
    }

}
