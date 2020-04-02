using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player1;
    public GameObject player2;
    private bool player1Win, player2Win;
    private Animator player1Anim, player2Anim;
    

    private void Awake() {
        player1Anim = player1.GetComponent<Animator>(); 
        player2Anim = player2.GetComponent<Animator>();    
    }
    void Start()
    {
        player1Win = false;
        player2Win = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player1Anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
            if (!player2Win)
                player2Anim.SetTrigger("Win");
            player2Win = true;

        }
        if (player2Anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
            if (!player1Win)
                player1Anim.SetTrigger("Win");
            player1Win = true;

        }


    }
}
