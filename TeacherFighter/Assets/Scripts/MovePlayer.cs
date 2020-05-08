/*
    @author Caleb Hardy

    Controls player physics
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;


public class MovePlayer : MonoBehaviour
{


    private Cooldown movementTime;
    private PlatformerCharacter2D m_Character;
    private float velocityX;
    private float velocityY;
    private bool usingY;
    private bool movementPreviousState;


    void Awake() {
        movementTime = gameObject.AddComponent<Cooldown>();
        m_Character = GetComponent<PlatformerCharacter2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    private void FixedUpdate() {
        if(movementTime.active()) {
            m_Character.preventMovement = true;
            if (usingY) {
                m_Character.m_Rigidbody2D.velocity = new Vector2(velocityX, velocityY);
            } else {
                m_Character.m_Rigidbody2D.velocity = new Vector2(velocityX, m_Character.m_Rigidbody2D.velocity.y);
            }
        } 
    }

    public void move(float v1, float v2, float duration) {
        movementPreviousState = m_Character.preventMovement;
        movementTime.startCooldown(enableMovement, duration);
        this.velocityX = v1;
        this.velocityY = v2;
        usingY = true;

    }

    public void move(float v1, float duration) {
        movementPreviousState = m_Character.preventMovement;
        movementTime.startCooldown(enableMovement, duration);
        this.velocityX = v1;
        

    }

    public void moveFacingDirection(float v1, float v2, float duration) {

        if (!m_Character.m_FacingRight)
            v1 = v1 * -1;

        movementPreviousState = m_Character.preventMovement;
        movementTime.startCooldown(enableMovement, duration);
        this.velocityX = v1;
        this.velocityY = v2;
        usingY = true;

    }

    public void moveFacingDirection(float v1, float duration) {

        if (!m_Character.m_FacingRight)
            v1 = v1 * -1;
            
        movementPreviousState = m_Character.preventMovement;
        movementTime.startCooldown(enableMovement, duration);
        this.velocityX = v1;
        

    }

    public bool isMoving() {
        return movementTime.active();
    }

    private void enableMovement() {
        m_Character.preventMovement = false;
    }
}
