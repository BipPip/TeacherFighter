using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;

public class PlayerWin : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player1;
    public GameObject player2;
    private bool player1Win, player2Win;
    public bool gameOver;
    private Animator player1Anim, player2Anim;
    private Cooldown exitLevelCountdown;

    public static int match = 0;
    public static bool player1Won;
    public static bool player2Won;

    public GameObject round1Text;
    public GameObject round2Text;
    public GameObject knockoutText;
    public GameObject player1WinText;
    public GameObject player2WinText;
    

    private void Awake() {
        player1Anim = player1.GetComponent<Animator>(); 
        player2Anim = player2.GetComponent<Animator>();    
        exitLevelCountdown = gameObject.AddComponent<Cooldown>();
    }
    void Start()
    {
        player1Win = false;
        player2Win = false;
        
        if (!player2Won && !player1Won) {
            round1Text.GetComponent<TextFlash>().turnOn(2);
        } else {
            round2Text.GetComponent<TextFlash>().turnOn(2);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (gameOver && !exitLevelCountdown.active()) {
            match++;
            exitLevelCountdown.startCooldown(exitLevel, 3f);
            if (!player2Won && !player1Won) knockoutText.GetComponent<TextFlash>().turnOn(2);
            if (player1Won && !player2Won) player1WinText.GetComponent<TextFlash>().turnOn(2);
            if (player2Won && !player1Won) player2WinText.GetComponent<TextFlash>().turnOn(2);
        }


        if (player1Anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
            if (!player2Win)
                player2Anim.SetTrigger("Win");
            if (player2Won)
                player2Anim.SetBool("Won", true);
            player2Win = true;
            gameOver = true;
            player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        }
        if (player2Anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
            if (!player1Win)
                player1Anim.SetTrigger("Win");
            if (player1Won)
                player1Anim.SetBool("Won", true);
            player1Win = true;
            gameOver = true;
            player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        }


    }

    public void timeoutWin() {
        if (player1.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().GetCurrentFraction 
        < player2.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().GetCurrentFraction) {
            if (!player2Win && player2Won)
                player2Anim.SetTrigger("Win");
            if (player2Won)
                player2Anim.SetBool("Won", true);
            player2Win = true;
            gameOver = true;
            player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            player1.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().UpdateBar(0, player1.GetComponent<PlatformerCharacter2D>().playerHealth);
        } else {
            if (!player1Win && player1Won)
                player1Anim.SetTrigger("Win");
            if (player1Won)
                player1Anim.SetBool("Won", true);
            player1Win = true;
            gameOver = true;
            player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            player2.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().UpdateBar(0, player2.GetComponent<PlatformerCharacter2D>().playerHealth);
        }
    }

    public void exitLevel() {
        if (player1Won || player2Won) {
            player1Won = false;
            player2Won = false;
            SceneManager.LoadScene("LevelSelect");
        } else {
            player1Won = player1Win;
            player2Won = player2Win;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }
        
        
        
        

    
    }
    
}
