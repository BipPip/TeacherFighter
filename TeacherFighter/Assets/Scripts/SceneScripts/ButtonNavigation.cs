/*
    @author Tia Flores-Carr

    I dont think this was used or even finished
    its better off deleted but Im not sure if
    it was used or not so
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNavigation : MonoBehaviour
{
    int index = 0;
    public int PauseOptions = 2;
    public float yOffset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("s"))
        {
            if(index < PauseOptions -1)
            {
                index++;
                Vector2 position = transform.position;
                position.y -= yOffset;
                transform.position = position;
            }
        }
        if(Input.GetKeyDown("w"))
        {
            if(index < 0)
            {
                index--;
                Vector2 position = transform.position;
                position.y += yOffset;
                transform.position = position;
            }
        }
    }
}
