/*
    @author Tia Flores-Carr

    Controls the start menu.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 
public class MenuScript: MonoBehaviour {
 
    void Update()
    {
        if(Input.anyKey){
            SceneManager.LoadScene("CharacterSelect");
        }
    }
}