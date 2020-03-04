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