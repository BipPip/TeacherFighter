using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class VonDerCombat : MonoBehaviour
{
    
    public Animator anim;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private SimpleHealthBar staminaBar;
    private Stamina stamina;

    // Lariat Timer

    public bool lariatActive;                 //Is this timer active?
    public float lariatCooldown = 0.5f;              //How often this cooldown may be used
    public float lariatTimer;                 //Time left on timer, can be used at 0


    void Awake() {
        staminaBar = gameObject.GetComponent<PlatformerCharacter2D>().staminaBarObject.GetComponent<SimpleHealthBar>();
        stamina = gameObject.GetComponent<Stamina>();
    }


    // Update is called once per frame
    void Update()
    {

        if(lariatActive)
            lariatTimer -= Time.deltaTime;    //Subtract the time since last frame from the timer.
        if (lariatTimer < 0) {
            lariatTimer = 0;                  //If timer is less than 0, reset it to 0 as we don't want it to be negative
            lariatActive = false;
        }


        // Lariat Attack input
        if(Input.GetButtonDown("Fire2"))
            {
                if (stamina.getStamina() >= 45f && gameObject.GetComponent<PlatformerCharacter2D>().m_Grounded) {
                if(!lariatActive) {
                    LariatAttack();
                    lariatActive = true;
                    lariatTimer = lariatCooldown;
                }
            }
        }
    }

    void LariatAttack()
    {
        
        anim.SetTrigger("LariatAttack");
        stamina.startCountdown(1f);
        stamina.staminaDecrease(45f);

       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

       foreach(Collider2D enemy in hitEnemies)
       {
           //Debug.Log("Hit enemy" + enemy.name );
           enemy.GetComponent<Damage>().doDamage(20f, 0.5f);

       }
    }

    void OnDrawGizmosSelected()
    {

        if(attackPoint == null)
        return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
