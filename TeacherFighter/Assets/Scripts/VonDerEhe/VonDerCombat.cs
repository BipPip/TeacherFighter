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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            Attack();
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");

       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

       foreach(Collider2D enemy in hitEnemies)
       {
           Debug.Log("Hit enemy" + enemy.name );
           enemy.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().UpdateBar((Canvas.healthBarLeft.GetCurrentFraction * 100) - 10, 100);
       }
    }

    void OnDrawGizmosSelected()
    {

        if(attackPoint == null)
        return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
