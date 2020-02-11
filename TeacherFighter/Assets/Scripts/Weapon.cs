using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject fireBallPrefab;
    Animator anim;

    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public LayerMask enemyLayers;


    private void Start()
    {
        rb.velocity = new Vector2(speed * transform.localScale.x, 0);
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();

        }
    }

    void Shoot()
    {
        GameObject ballClone = Instantiate(fireBallPrefab, firePoint.position, firePoint.rotation);
        ballClone.transform.localScale = transform.localScale;
        anim.SetTrigger("Attack");
    }

    
}
