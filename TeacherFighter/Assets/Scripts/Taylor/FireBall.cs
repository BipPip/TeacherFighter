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
    private AudioSource[] audioData;
    private Component[] audioArray;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioArray = gameObject.GetComponents(typeof(AudioSource));
        audioData = new AudioSource[audioArray.Length];
        
        for(int i = 0; i < audioArray.Length; i++) 
        {
            audioData[i] = (AudioSource) audioArray[i];
        }

        AudioSource.PlayClipAtPoint(audioData[1].clip, gameObject.transform.position);
    }

    void Update()
    {
        rb.velocity = new Vector2(speed * transform.localScale.x, 0);
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        

        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Damage>().doDamage(5f, 0.5f);
        }
        AudioSource.PlayClipAtPoint(audioData[0].clip, gameObject.transform.position);
        Destroy(gameObject);
        Instantiate(impactEffect, transform.position, transform.rotation);
    }

}
