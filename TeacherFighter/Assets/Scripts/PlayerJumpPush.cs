using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerJumpPush : MonoBehaviour
{

    // public bool preventMovement;
    public bool startedWait;
    public bool isColliding;
    private Cooldown wait;
    private static int count;
    public float velocity = 10f;
    private Collider2D other;
    
    static PlatformerCharacter2D m_Character;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Test");
        wait = gameObject.AddComponent<Cooldown>();
        m_Character = GetComponent<PlatformerCharacter2D>();
        if (!m_Character.m_FacingRight) velocity = velocity * -1;
        // if (!m_Character.m_FacingRight) {
        //         velocity = velocity * -1;
        //     }
    }


    // Update is called once per frame
    void Update()
    {  
        
        // Debug.Log(count);
        
            
        if (isColliding) {
            // if (!m_Character.m_FacingRight) {
            //     velocity = velocity * -1;
            // }
            // if(velocity < 0) velocity = velocity * -1;
            
            m_Character.m_Rigidbody2D.velocity = new Vector2(velocity, m_Character.m_Rigidbody2D.velocity.y);
        }
        if (!wait.active() && startedWait == true) {
           m_Character.preventMovement = false;
           startedWait = false;
        //    if(velocity < 0) velocity = velocity * -1;
        //    if (!m_Character.m_FacingRight) velocity = velocity * -1;
        //    isColliding = false;
       }


    }
    private void OnTriggerEnter2D(Collider2D other) {
        this.other = other;
        // Debug.Log("bRUH");
        if(isColliding || this.gameObject.transform.position.y < this.other.gameObject.transform.position.y) return;
        isColliding = true;
    // if(count == 0) m_Character = GetComponent<PlatformerCharacter2D>();
     // Rest of the code
        if (other.tag == "Player" && other.name != gameObject.name) {
            // Debug.Log("EPIC");
            // m_Character = other.GetComponentInParent<PlatformerCharacter2D>();
            m_Character.preventMovement = true;
            wait.startCooldown(0.5f);
            startedWait = true;
            
            // float velocity = 10f;
            // if (!m_Character.m_FacingRight) {
            //     velocity = velocity * -1;
            // }
            // m_Character.m_Rigidbody2D.velocity = new Vector2(velocity, m_Character.m_Rigidbody2D.velocity.y + 2);
            Debug.Log(other.name);
            
        }
        count++;
    }

    private void OnTriggerExit2D(Collider2D other) {
        this.other = other;
        if(this.gameObject.transform.position.y < this.other.gameObject.transform.position.y) return;
        velocity = velocity * -1;
        count = 0;
        isColliding = false;
        
    }

}
