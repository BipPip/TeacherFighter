using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;

public class Damage : MonoBehaviour
{

    private Animator anim;
    private SimpleHealthBar playerHealthBar;

    float h;
    float h2;
    float v;
    float v2;
    bool p2block;

    private Stamina stamina;

    // Start is called before the first frame update
    void Start()
    {
        this.anim = gameObject.GetComponent<Animator>();
        this.playerHealthBar = gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>();
        this.stamina = gameObject.GetComponent<Stamina>();
        
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
        
        if (this.stamina.getStamina() >= 5 && ((gameObject.GetComponent<PlatformerCharacter2D>().m_FacingRight && v == 1 && h < 1 && h > -1) || !gameObject.GetComponent<PlatformerCharacter2D>().m_FacingRight && v2 < 0 && CrossPlatformInputManager.GetButton("Vertical2")) && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) {
            anim.SetTrigger("Block");
            // v2 = 0;
        }
        Debug.Log(p2block);

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Run") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchingWalk") 
        || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Block")){
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                
            }

        // Checks if player is dead
        if (playerHealthBar.GetCurrentFraction <= 0) {
            
                anim.SetTrigger("Die");
                //Destroy(gameObject);
            }
    }

    public void doDamage(float damage, float knockback) {
        if (gameObject.GetComponent<PlatformerCharacter2D>().m_FacingRight) {
                knockback = knockback * -1;
            }
        h = CrossPlatformInputManager.GetAxis("Horizontal");
        h2 = CrossPlatformInputManager.GetAxis("Horizontal2");
        v = CrossPlatformInputManager.GetAxis("Vertical");
        v2 = CrossPlatformInputManager.GetAxis("Vertical2");
        if(this.stamina.getStamina() >= 5 && (gameObject.GetComponent<PlatformerCharacter2D>().m_FacingRight && (h < 0 || v > 0) || !gameObject.GetComponent<PlatformerCharacter2D>().m_FacingRight && (h2 > 0 || v2 < 0)) && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) {
            anim.SetTrigger("Block");
            this.stamina.staminaDecrease(5f);
            this.stamina.startCountdown(1);
        } else {

        this.playerHealthBar.UpdateBar((gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().GetCurrentFraction * 100) - damage, 100);
        if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) {
            anim.SetTrigger("Hit");
            
        }
    }
    gameObject.transform.position += new Vector3(knockback, 0, 0);
}

    

}
