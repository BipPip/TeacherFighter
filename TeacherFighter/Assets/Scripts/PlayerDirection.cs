using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerDirection : MonoBehaviour
{


    public GameObject player1;
    public GameObject player2;
    private Vector3 player1Scale;
    private Vector3 player2Scale;
    private PlatformerCharacter2D player1Platformer;
    private PlatformerCharacter2D player2Platformer;
    // Start is called before the first frame update
    void Start()
    {
        
        player1Platformer = player1.GetComponent<PlatformerCharacter2D>();
        player2Platformer = player2.GetComponent<PlatformerCharacter2D>();


        
    }

    // Update is called once per frame
    void Update()
    {
        player1Scale = player1.transform.localScale;
        player2Scale = player2.transform.localScale;
        // Debug.Log(player1.transform.position.x);
        // Debug.Log(player1.transform.position.x > player2.transform.position.x);
        // Debug.Log(player1.transform.localScale.x);
        if ((player1.transform.position.x > player2.transform.position.x) && player1Platformer.m_FacingRight) {
            player1Platformer.m_FacingRight = false;
            player1.transform.localScale = new Vector3(player1Scale.x * -1, player1Scale.y, player1Scale.z);

            player2Platformer.m_FacingRight = true;
            player2.transform.localScale = new Vector3(player2Scale.x * -1, player2Scale.y, player2Scale.z);

        } else if ((player1.transform.position.x < player2.transform.position.x) && !player1Platformer.m_FacingRight) {
            player1Platformer.m_FacingRight = true;
            player1.transform.localScale = new Vector3(player1Scale.x * -1, player1Scale.y, player1Scale.z);

            player2Platformer.m_FacingRight = false;
            player2.transform.localScale = new Vector3(player2Scale.x * -1, player2Scale.y, player2Scale.z);
        }
        

        
    }
}
