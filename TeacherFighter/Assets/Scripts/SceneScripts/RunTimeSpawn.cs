using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RunTimeSpawn : CharacterSelect
{
    // Start is called before the first frame update

    string prefabName;

    void Start()
    {

        prefabName = getPlayer1();

        //charsel = GameObject.FindObjectOfType<CharacterSelect>();
        //string charselected = charsel.charName;
       


        //charsel = gm.GetComponent<CharacterSelect>(); 
        //string charselected = gameObject.GetComponent<CharacterSelect>().charName;



        Debug.Log();
        //Debug.Log(charsel.charName);





        //char1 = (GameObject)Resources.Load("prefabs/" + charName, typeof(GameObject));
        //Instantiate(char1, new Vector3(0,0,0), Quaternion.identity);
    }

}
