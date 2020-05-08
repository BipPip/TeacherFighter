/*
    @author Tia Flores-Carr

    Not sure if this file is currently used
    however, its for particles for the projectiles
    so that they'll be deleted once the life time has
    been reached.  
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    public float lifeTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if(lifeTime <0)
        {
            Destroy(gameObject);
        }
    }
}
