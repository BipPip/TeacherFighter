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

    public GameObject rightBar;
    public GameObject leftBar;

    public GameObject leftCooldown;
    public GameObject rightCooldown;



    void Start()
    {

        prefabName = getPlayer1();

        Debug.Log("RunTimeSpawn Script" + prefabName);
       
        GameObject p1Health = Instantiate(rightBar,new Vector3(0,0,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        //p1Health.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);

        if(prefabName.Equals("CharacterTaylor")){
            //Instantiate(CTaylor);

        }else if(prefabName.Equals("CharacterVonDerEhe")){
            //Instantiate(CVonderehe);
        }
    }

}
