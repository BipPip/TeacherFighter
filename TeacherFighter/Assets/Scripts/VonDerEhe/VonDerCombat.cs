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

    // Lariat Timer

    public bool lariatActive;                 //Is this timer active?
    public float lariatCooldown = 1;              //How often this cooldown may be used
    public float lariatTimer;                 //Time left on timer, can be used at 0

    // Update is called once per frame
    void Update()
    {

        if(lariatActive)
            lariatTimer -= Time.deltaTime;    //Subtract the time since last frame from the timer.
        if (lariatTimer < 0) {
            lariatTimer = 0;                  //If timer is less than 0, reset it to 0 as we don't want it to be negative
            lariatActive = false;
        }

        if(Input.GetButtonDown("Fire2"))
            {
                if(!lariatActive) {
                    LariatAttack();
                    lariatActive = true;
                    lariatTimer = lariatCooldown;
                }
            }
    }

    void LariatAttack()
    {
        
        anim.SetTrigger("LariatAttack");

       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

       foreach(Collider2D enemy in hitEnemies)
       {
           Debug.Log("Hit enemy" + enemy.name );
           SimpleHealthBar enemyHealth = enemy.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>();
           enemyHealth.UpdateBar((enemyHealth.GetCurrentFraction * 100) - 10, 100);
       }
    }

    void OnDrawGizmosSelected()
    {

        if(attackPoint == null)
        return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
