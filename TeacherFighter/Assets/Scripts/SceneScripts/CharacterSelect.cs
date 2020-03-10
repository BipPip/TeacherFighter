using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterSelect : MonoBehaviour
{
    
    static string charName;

    public GameObject vonderehedisplay;
    public GameObject taylordisplay;

    GameObject TSprite;
    GameObject VSprite;


    public void playerSelected(string character)
    {
        charName = character;
        Debug.Log("variable " + charName);
        
        Debug.Log("From getPlayer" + getPlayer1());


        if(charName.Equals("CharacterTaylor")){
            Destroy(VSprite);
            GameObject TSprite = Instantiate(taylordisplay,new Vector3(-7,2.5f,0), Quaternion.identity);
            TSprite.transform.localScale = new Vector3(5,5,1); 
            TSprite.SetActive(true);
        }else if(charName.Equals("CharacterVonDerEhe")){
            Destroy(TSprite);
            GameObject VSprite = Instantiate(vonderehedisplay,new Vector3(-7,2.5f,0), Quaternion.identity);
            VSprite.transform.localScale = new Vector3(4,4,1); 
            VSprite.SetActive(true);
        }
    }

    

    
    public string getPlayer1(){
       return charName;
    }

}
