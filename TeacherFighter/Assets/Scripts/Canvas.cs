using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    public GameObject leftHealthBarObject;
    public GameObject rightHealthBarObject;
    // Start is called before the first frame update
    public static SimpleHealthBar healthBarLeft;
    public static SimpleHealthBar healthBarRight;
    void Start()
    {
        healthBarLeft = leftHealthBarObject.GetComponent<SimpleHealthBar>();
        healthBarRight = rightHealthBarObject.GetComponent<SimpleHealthBar>();
        // print(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
