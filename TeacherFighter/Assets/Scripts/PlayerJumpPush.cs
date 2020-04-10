using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using System;

public class PlayerJumpPush : MonoBehaviour
{

    // public bool preventMovement;
    public bool startedWait;
    public bool isColliding;
    public bool nonTriggerCollide = false;
    private bool firstEntry = true;
    private bool enteredRight;
    private Cooldown wait;
    private Cooldown movementWait;
    private static int count;
    public float velocity = 10f;
    private Vector3 pushVelocity;
    private Collider2D other;
    
    private PlatformerCharacter2D m_Character;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Test");
        m_Character = GetComponent<PlatformerCharacter2D>();
        Debug.Log(m_Character.name);
        wait = gameObject.AddComponent<Cooldown>();
        movementWait = gameObject.AddComponent<Cooldown>();
        // other = null;
        
        // if (!m_Character.m_FacingRight) velocity = velocity * -1;
        // if (!m_Character.m_FacingRight) {
        //         velocity = velocity * -1;
        //     }
    }


    // Update is called once per frame
    void FixedUpdate()
    {  
        // Debug.Log(other.name);
        // Debug.Log(Math.Abs(other.transform.position.x - m_Character.transform.position.x));
        // if (!isColliding && other != null) {
        //     if (!other.GetComponent<PlatformerCharacter2D>().m_Grounded) {
        //         if (other.transform.position.x - m_Character.transform.position.x < 1)
        //             other.GetComponent<MovePlayer>().moveFacingDirection(50, 0.5f);

        //     }
        // }
        // Debug.Log(count);
        // if (!m_Character.m_Grounded)
        //     m_Character.m_Rigidbody2D.velocity = new Vector2(m_Character.m_Rigidbody2D.velocity.x, m_Character.m_Rigidbody2D.velocity.y + 1);




        if (isColliding) {
            if (!firstEntry) {
            m_Character = other.GetComponent<PlatformerCharacter2D>();
            }  else {
                m_Character = GetComponent<PlatformerCharacter2D>();
            }

            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            // if (!m_Character.m_FacingRight) {
            //     velocity = velocity * -1;
            // }
            // if(velocity < 0) velocity = velocity * -1;
            if (enteredRight) {
                m_Character.m_Rigidbody2D.velocity = new Vector2(velocity, m_Character.m_Rigidbody2D.velocity.y);
            } else {
                m_Character.m_Rigidbody2D.velocity = new Vector2(velocity * -1, m_Character.m_Rigidbody2D.velocity.y);
            }
        }

        if (nonTriggerCollide) {
            // m_Character.m_Rigidbody2D.AddForce(new Vector2(pushVelocity.x * -1, 0f));
            
            m_Character.preventMovement = true;
            movementWait.startCooldown(enableMovement, 0.2f);
            // Debug.Log("test");
            // velocity = 0;
            m_Character.m_Rigidbody2D.velocity = new Vector2(0, m_Character.m_Rigidbody2D.velocity.y - 0.5f);
            if (m_Character.m_Grounded) {
                nonTriggerCollide = false;
                m_Character.preventMovement = false;
            }
        }

        if (!wait.active() && startedWait == true) {
           m_Character.preventMovement = false;
           startedWait = false;
        //    if(velocity < 0) velocity = velocity * -1;
        //    if (!m_Character.m_FacingRight) velocity = velocity * -1;
           isColliding = false;
       }

    if (movementWait.active())
        m_Character.preventMovement = true;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        // this.other = other;
        if (other.gameObject.tag != "Player")
            return;

        if (other.GetComponent<PlatformerCharacter2D>().nearWall) {
            nonTriggerCollide = true;
            return;
        } 
        
        // Debug.Log("bRUH");
        // // if(velocity < 0) velocity = velocity * -1;
        // if (!m_Character.m_FacingRight) velocity = velocity * -1;
        if(isColliding || this.gameObject.transform.position.y < other.gameObject.transform.position.y) return;
        
        
        if (!firstEntry) {
            m_Character = other.GetComponent<PlatformerCharacter2D>();
        }  else {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }
        // firstEntry = false;
        // Debug.Log(m_Character.m_Rigidbody2D.velocity.x);
        enteredRight = m_Character.m_FacingRight;
        isColliding = true;
    // if(count == 0) m_Character = GetComponent<PlatformerCharacter2D>();
     // Rest of the code
        if (other.tag == "Player" && other.gameObject != gameObject) {
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
            // Debug.Log(m_Character.name);
            //Debug.Log(other.name);
            
        }

    

    }

    private void OnTriggerExit2D(Collider2D other) {
        // this.other = other;
        
        if(this.gameObject.transform.position.y < other.gameObject.transform.position.y) return;
        if (!wait.active())
            m_Character.preventMovement = false;
        nonTriggerCollide = false;
        // velocity = velocity * -1;
        // count = 0;
        // m_Character.preventMovement = false;
        isColliding = false;
        
    }

    private void OnTriggerStay2D(Collider2D other) {
      
        // if (m_Character.m_Grounded) return;
     
        // if (other.GetComponent<PlatformerCharacter2D>().nearWall) {
        //     nonTriggerCollide = true;
        //     // other.GetComponent<PlatformerCharacter2D>().preventMovement = true;
        // }
        if (other.tag == "Player") {
            if (nonTriggerCollide)
                m_Character.preventMovement = false;
                
            
        }

        // if (other.tag == "Player") {
        // if (nonTriggerCollide) {
        //     if (!m_Character.m_Grounded) {
        //     Debug.Log("Test");

        //         if (other.transform.position.x - m_Character.transform.position.x < 1) {
        //             if (!GetComponent<MovePlayer>().isMoving())
        //                 GetComponent<MovePlayer>().moveFacingDirection(5, 1f);
                    
        //         }
        //     }
        // }
        // }
    }

    private void enableMovement() {
        m_Character.preventMovement = false;
    }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     if (other.collider.isTrigger)
    //         Debug.Log("test");

    //     if(other.collider.offset.y == 1.25f || other.collider.offset.y == 1.19f) {
    //         Vector3 pushVelocity = other.relativeVelocity;
    //         Debug.Log(pushVelocity.x);
    //         // other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    //         m_Character.preventMovement = true;
    //         m_Character.m_Rigidbody2D.velocity = new Vector2(0, m_Character.m_Rigidbody2D.velocity.y);
    //         nonTriggerCollide = true;

    //     }
    // }

    // private void OnCollisionExit2D(Collision2D other) {
    //     if(other.collider.offset.y == 1.25f || other.collider.offset.y == 1.19f) {
    //         nonTriggerCollide = false;
    //         m_Character.preventMovement = false;
           
    //     }
    // }


}
