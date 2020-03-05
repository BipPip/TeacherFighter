using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RunTimeSpawn : CharacterSelect
{
    // Start is called before the first frame update

    string prefabName;
    GameObject char1;

    void Start()
    {

        prefabName = getPlayer1();

        Debug.Log(prefabName);
       
        char1 = (GameObject)Resources.Load("prefabs/" + prefabName, typeof(GameObject));
        //Instantiate(char1, new Vector3(0,0,0), Quaternion.identity);
    }

}
