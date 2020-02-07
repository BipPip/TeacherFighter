using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject fireBallPrefab;
    Animator anim;


    private void Start()
    {
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
