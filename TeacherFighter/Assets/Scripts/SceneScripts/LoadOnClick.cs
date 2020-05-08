/*
    @author Tia Flores-Carr

    Used in the level selection screen
    if you click on a level then it goes
    to it.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadOnClick : MonoBehaviour
{
    
    public void LoadScene(string level)
    {
        SceneManager.LoadScene(level);
    }

}
