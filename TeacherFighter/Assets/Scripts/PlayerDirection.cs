using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerDirection : MonoBehaviour
{


    public GameObject player1;
    public GameObject player2;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(player1.transform.position.x);
        Debug.Log(player1.transform.position.x > player2.transform.position.x);
        if (player1.transform.position.x > player2.transform.position.x) {
            player1.GetComponent<PlatformerCharacter2D>().m_FacingRight = false;
            player1.transform.scale.x = player1.transform.scale.x * -1;
        }

        
    }
}
