using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class FireBall : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        rb.velocity = new Vector2(speed * transform.localScale.x, 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if(col.gameObject.tag == "Player"){
            col.gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>().UpdateBar((Canvas.healthBarRight.GetCurrentFraction * 100) - 10, 100);

        }
        Destroy(gameObject);
        Instantiate(impactEffect, transform.position, transform.rotation);
    }

}
