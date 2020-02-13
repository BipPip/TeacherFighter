using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Damage : MonoBehaviour
{

    private Animator anim;
    private SimpleHealthBar playerHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        this.anim = gameObject.GetComponent<Animator>();
        this.playerHealthBar = gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")){
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void doDamage(float damage, float knockback) {
        this.playerHealthBar.UpdateBar((gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().GetCurrentFraction * 100) - damage, 100);
        anim.SetTrigger("Hit");
        if (gameObject.GetComponent<PlatformerCharacter2D>().m_FacingRight) {
            knockback = knockback * -1;
        }
        gameObject.transform.position += new Vector3(knockback, 0, 0);
        
    }

}
