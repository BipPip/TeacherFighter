using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;
using System;


public class Damage : MonoBehaviour
{

    private Animator anim;
    private PlatformerCharacter2D m_Character;
    private SimpleHealthBar playerHealthBar;
    private Cooldown blockDelay;
    private Cooldown forceStun;
    private bool blocking;
    private bool allowBlock = false;
    private bool blocked = true;
    public bool knockbacking;
    private float knockback;

    float h;
    float h2;
    float v;
    float v2;
    bool p2block;

    float playerHealth;

    private Stamina stamina;

    // Start is called before the first frame update
    void Start()
    {
        this.anim = gameObject.GetComponent<Animator>();
        this.playerHealthBar = gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>();
        this.stamina = gameObject.GetComponent<Stamina>();
        this.blockDelay = gameObject.AddComponent<Cooldown>();
        this.forceStun = gameObject.AddComponent<Cooldown>();
        this.playerHealth = gameObject.GetComponent<PlatformerCharacter2D>().playerHealth;
        this.m_Character = gameObject.GetComponent<PlatformerCharacter2D>();
        
        
        // Debug.Log(playerHealth);
        
    }

    // Update is called once per frame
    void Update()
    {
        h = CrossPlatformInputManager.GetAxis("Horizontal");
        h2 = CrossPlatformInputManager.GetAxis("Horizontal2");
        v = CrossPlatformInputManager.GetAxis("Vertical");
        v2 = CrossPlatformInputManager.GetAxis("Vertical2");

        

        if (CrossPlatformInputManager.GetButton("Vertical2")) {
            p2block = true;
        } else {
            p2block = false;
        }
        // Debug.Log(v);
        

        if(m_Character.m_Grounded)
            gameObject.GetComponent<PlayerJumpPush>().isColliding = false;

        
        if(knockbacking && knockback > 0) {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            //anim.SetTrigger("Hit");
            if (!gameObject.GetComponent<PlatformerCharacter2D>().m_FacingRight) {
                // gameObject.transform.position += new Vector3(0.5f, 0, 0);
                // m_Character.Move(100f, false, false);
                // m_Character.
                // m_Character.m_Rigidbody2D.velocity += new Vector2(0.5f, m_Character.m_Rigidbody2D.velocity.y);
                // m_Character.m_Rigidbody2D.AddForce(new Vector2(0.5f * 10000f, 0f));
                m_Character.m_Rigidbody2D.velocity = new Vector2(50f, m_Character.m_Rigidbody2D.velocity.y);
            }
            else {
                // gameObject.transform.position += new Vector3(-0.5f, 0, 0);
                m_Character.m_Rigidbody2D.velocity = new Vector2(-50f, m_Character.m_Rigidbody2D.velocity.y);
            }

            knockback--;
        }
        else {
            knockbacking = false;
            knockback = 0;
        }
        

        if(this.m_Character.m_Grounded && this.stamina.getStamina() > 0 && (gameObject.name == "CharacterTaylor" && ((v == 1 && h < 1 && h > -1) || (h < 0 && m_Character.m_FacingRight) || (h > 0 && !m_Character.m_FacingRight) || ((Input.GetKey(KeyCode.A) && m_Character.m_FacingRight) || (Input.GetKey(KeyCode.D) && !m_Character.m_FacingRight) || Input.GetKey(KeyCode.S)))
        || gameObject.name == "CharacterVonDerEhe" &&
         /*(h2 > 0 || v2 < 0))*/ (Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.RightArrow) && !m_Character.m_FacingRight) || (Input.GetKey(KeyCode.LeftArrow) && m_Character.m_FacingRight))) 
         && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) {
             
             // Block Delay
             if (blocked) {
                // Debug.Log("poggers");
                blockDelay.startCooldown(enableBlock, 0.1f);
                blocked = false;
             }
             
             if (allowBlock) {
                blocking = true;
                blocked = true;
             }
         }
         else {
             blocking = false;
             allowBlock = false;
             blocked = true;
         }

         if (forceStun.active()) {
            anim.SetTrigger("Hit");
            allowBlock = false;
            blocking = false;
            v = 0;
            h = 0;
            v2 = 0;
            h2 = 0;
            anim.ResetTrigger("Block");
        }

         if (m_Character.m_Grounded && (this.stamina.getStamina() >= 5 && ((gameObject.name == "CharacterTaylor" 
         && ((v == 1 && h < 1 && h > -1) || Input.GetKey(KeyCode.S))) 
         || gameObject.name == "CharacterVonDerEhe" && v2 < 0 && CrossPlatformInputManager.GetButton("Vertical2"))) 
        && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) {
            
            if (blocking) {
                // Debug.Log("RIP");
                anim.SetTrigger("Block");
            }
            
            // v2 = 0;
        }

        // Debug.Log(allowBlock);
       

        if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Light") && this.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Walk")) || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Run") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchingWalk") 
        || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        if((!knockbacking && this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Block")){
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                
            }

        // Checks if player is dead
        if (playerHealthBar.GetCurrentFraction <= 0) {
            
                anim.SetTrigger("Die");
                //Destroy(gameObject);
            }
    }

    public void doDamage(float damage, float knockback) {
        if (gameObject.GetComponent<PlayerJumpPush>().isColliding)
            gameObject.GetComponent<PlayerJumpPush>().isColliding = false;

        // if (gameObject.GetComponent<PlatformerCharacter2D>().m_FacingRight) {
        //         knockback = knockback * -1;
        //     }
        // gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        this.knockback = knockback;
        h = CrossPlatformInputManager.GetAxis("Horizontal");
        h2 = CrossPlatformInputManager.GetAxis("Horizontal2");
        v = CrossPlatformInputManager.GetAxis("Vertical");
        v2 = CrossPlatformInputManager.GetAxis("Vertical2");

        // todo: for when we get player select to work, we need to find a way to tell if player is using controller or not

        if(blocking) {
            
            float staminaDecreaseAmount = (float) Math.Pow(damage, 1.4f);
            if (staminaDecreaseAmount > 35)
                staminaDecreaseAmount = 35;
            this.stamina.staminaDecrease(staminaDecreaseAmount);
            // Debug.Log(this.stamina.getStamina());
            if(this.stamina.getStamina() <= 0) {
                this.playerHealthBar.UpdateBar((gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().GetCurrentFraction * playerHealth) - damage, playerHealth);
                knockbacking = true;
                blocking = false;
                allowBlock = false;
                blocked = false;
                blockDelay.startCooldown(enableBlock, 0.15f);
                forceStun.startCooldown(0.15f);
                //Debug.Log(this.stamina.getStamina());
                anim.SetTrigger("Hit");
            }
            else {
                // Debug.Log("TEEEEEEEEEEESTTT");
                if (!forceStun.active())
                    anim.SetTrigger("Block");
            }
            // Debug.Log("t");
            // Debug.Log(staminaDecreaseAmount);
            this.stamina.startCountdown(1);
            //Debug.Log("TEST");
        } else {

        this.playerHealthBar.UpdateBar((gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().GetCurrentFraction * playerHealth) - damage, playerHealth);
        if (/*!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun") &&*/ !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Lariat")) {
            anim.SetTrigger("Hit");
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            knockbacking = true;
           
            
        }
    }
    
    
}
    public void enableBlock() {
        allowBlock = true;
    }

    

}
