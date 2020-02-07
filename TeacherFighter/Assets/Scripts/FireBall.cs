using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(speed * transform.localScale.x, 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);

        Instantiate(impactEffect, transform.position, transform.rotation);
    }

}
