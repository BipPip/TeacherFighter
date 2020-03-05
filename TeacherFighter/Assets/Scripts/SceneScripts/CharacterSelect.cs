using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    
    static string charName;

    public void playerSelected(string character)
    {
        charName = character;
        Debug.Log("variable " + charName);
        
        Debug.Log("From getPlayer" + getPlayer1());
    }

    public string getPlayer1(){
       return charName;
    }

}
