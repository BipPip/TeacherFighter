/*
    @author Caleb Hardy & Tia Flores-Carr

    Taylors player class. Controls all movements
    and fighting controls. 
*/
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//This is the script that Mr Taylor Uses for movement

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class TaylorFightControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;

        private bool m_Jump;
        private bool m_Dodge;
        private bool m_FacingRight = true;
        private bool lightActive, mediumActive, heavyActive, jumpActive;


        private float lightCooldownAmount;

        private SpamPrevention tripleJab;

        private Cooldown fireCooldown;
        private Cooldown lightCooldown;
        private Cooldown mediumCooldown;
        private Cooldown heavyCooldown;
        private Cooldown moveActive; // Used because there is a slight delay between anim.trigger and the actual animation returning active
        private Cooldown damageWait;
        private Cooldown animEnd;
        public bool startedWait;
        public bool isColliding;
        private Cooldown wait;

        public Transform firePoint;
        public Transform respawnPoint;
        public Transform basicAttackPoint;
        public  Transform shortAttackPoint;
        
        public GameObject fireBallPrefab;
        private GameObject cooldownUI;

        public float basicAttackRange = 0.5f;
        
        private SimpleHealthBar playerHealthBar;
        private SimpleHealthBar staminaBar;
        private Stamina stamina;

        private Vector3 startPosition;
        private Animator anim;
        public LayerMask enemyLayers;
        private Component[] audioArray;
        private AudioSource[] audioData;

        private GameObject player1, player2;

        private void Start()
        {


            player1 = GameObject.Find("Main Camera").GetComponent<PlayerLoad>().player1;
            player2 = GameObject.Find("Main Camera").GetComponent<PlayerLoad>().player2;

            anim = gameObject.GetComponent<Animator>();
            stamina = gameObject.GetComponent<Stamina>();
           
     
            m_Character = GetComponent<PlatformerCharacter2D>();
            startPosition = transform.position;
            anim = gameObject.GetComponent<Animator>();
            playerHealthBar = gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>();
            staminaBar = gameObject.GetComponent<PlatformerCharacter2D>().staminaBarObject.GetComponent<SimpleHealthBar>();
            
            audioArray = gameObject.GetComponents(typeof(AudioSource));
            audioData = new AudioSource[audioArray.Length];
            
            for(int i = 0; i < audioArray.Length; i++) 
            {
                audioData[i] = (AudioSource) audioArray[i];
            }

            heavyCooldown = gameObject.AddComponent<Cooldown>();
            mediumCooldown = gameObject.AddComponent<Cooldown>();
            lightCooldown = gameObject.AddComponent<Cooldown>();
            fireCooldown = gameObject.AddComponent<Cooldown>();
            damageWait = gameObject.AddComponent<Cooldown>();
            moveActive = gameObject.AddComponent<Cooldown>();
            animEnd = gameObject.AddComponent<Cooldown>();
            tripleJab = gameObject.AddComponent<SpamPrevention>();
            tripleJab.init(3, 0.5f);
            wait = gameObject.AddComponent<Cooldown>();
            
        }


        private void Update()
        {
            if (anim.GetBool("Won") || anim.GetCurrentAnimatorStateInfo(0).IsName("Win") || anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) return;

            float distanceToLeft = GameObject.Find("/PlatformLeft").transform.position.x;
            distanceToLeft = distanceToLeft - (gameObject.transform.position.x + gameObject.GetComponent<CapsuleCollider2D>().size.x);

            float distanceToRight = GameObject.Find("/PlatformRight").transform.position.x;
            distanceToRight = distanceToRight - (gameObject.transform.position.x + gameObject.GetComponent<CapsuleCollider2D>().size.x);

             if ((distanceToLeft > -4.57 && m_Character.m_FacingRight) || (distanceToRight <= 10 && !m_Character.m_FacingRight)) {
                m_Character.nearWall = true;
                // gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true;
                gameObject.GetComponents<BoxCollider2D>()[1].offset = new Vector2(0f, gameObject.GetComponents<BoxCollider2D>()[1].offset.y);
                if (m_Character.m_Rigidbody2D.velocity.y > -21f && !m_Character.m_Grounded && ((distanceToLeft > -4.57 && !m_Character.m_FacingRight) || (distanceToRight <= 10 && m_Character.m_FacingRight)))
                    m_Character.m_Rigidbody2D.velocity = new Vector2(m_Character.m_Rigidbody2D.velocity.x, m_Character.m_Rigidbody2D.velocity.y - 0.5f);
            }
            else {
                m_Character.nearWall = false;
                // gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true;
                gameObject.GetComponents<BoxCollider2D>()[1].offset = new Vector2(-0.1f, gameObject.GetComponents<BoxCollider2D>()[1].offset.y);
            }

            // Debug.Log(distanceToRight);
            // if (!wait.active() && startedWait == true) {
            //     m_Character.preventMovement = false;
            //     startedWait = false;
            //     isColliding = false;
            // }
            // Debug.Log(m_Character.m_Rigidbody2D.velocity.x);
            if(!moveActive.active()) {
                anim.speed = 1f;
            }

            if (!m_Jump) { 
                if (gameObject == player1) m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                if (gameObject == player2) m_Jump = CrossPlatformInputManager.GetButtonDown("Jump2");

            }
            if (!m_Dodge)
            
                m_Dodge = CrossPlatformInputManager.GetButtonDown("Taylor_Dodge");
            if (m_Character.m_Grounded) 
                jumpActive = false;
            else 
                jumpActive = true;
            // Detect current active attack
            
            heavyActive = this.anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy");
            mediumActive = this.anim.GetCurrentAnimatorStateInfo(0).IsName("Medium");
            lightActive = this.anim.GetCurrentAnimatorStateInfo(0).IsName("Light");


                
            // Handle Inputs

            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun") && !moveActive.active() &&
            !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Block") && !gameObject.GetComponent<PlayerJumpPush>().isColliding) 
            {
                
                if(((Input.GetButtonDown("Taylor_Fire") || Input.GetAxis("Axis 10") != 0) && gameObject == player1
                 || (Input.GetButtonDown("Taylor_Fire 2") || Input.GetAxis("Axis 10 2") != 0) && gameObject == player2) && !heavyActive)
                {
                    if (stamina.getStamina() >= 20f) 
                    {
                        if (!fireCooldown.active()) 
                        {
                        Shoot();
                        stamina.startCountdown(1f);
                        fireCooldown.startCooldown(0.5f);
                        }
                    }
                }
                else if ((Input.GetButtonDown("Taylor_Light") && gameObject == player1) || (Input.GetButtonDown("Taylor_Light 2") && gameObject == player2) && !heavyActive) 
                {
                    if (!lightCooldown.active()) 
                    {
                        Light();
                        tripleJab.run();
                        if(tripleJab.notLast()) {
                            lightCooldown.startCooldown(0.2f);
                            lightCooldownAmount = 0.2f;
                        }
                        else {
                            lightCooldown.startCooldown(0.8f);
                            lightCooldownAmount = 0.8f;
                            moveActive.startCooldown(0.45f);
                        }
                      
                    }
                }
                else if ((Input.GetButtonDown("Taylor_Medium") && gameObject == player1) || (Input.GetButtonDown("Taylor_Medium 2") && gameObject == player2) && !heavyActive) 
                {
                    if (!mediumCooldown.active()) 
                    {
                        Medium();
                        mediumCooldown.startCooldown(0.5f);
                        moveActive.startCooldown(0.2f);
                    }
                }
                else if ((Input.GetButtonDown("Taylor_Heavy") && gameObject == player1) || (Input.GetButtonDown("Taylor_Heavy 2") && gameObject == player2)) 
                {
                    if (!heavyCooldown.active()) 
                    {
                        Heavy();
                        heavyCooldown.startCooldown(0.8f);
                        moveActive.startCooldown(0.5f);
                    }
                }
            }

            // Updates Cooldown UI
            cooldownUI = m_Character.cooldownUI;

            // Light
            cooldownUI.transform.GetChild(0).gameObject.transform.GetChild(0)
                .gameObject.transform.GetChild(0).GetComponent<SimpleHealthBar>().UpdateBar(lightCooldown.getCurrentTime(), lightCooldownAmount);
            // Medium
            cooldownUI.transform.GetChild(1).gameObject.transform.GetChild(0)
                .gameObject.transform.GetChild(0).GetComponent<SimpleHealthBar>().UpdateBar(mediumCooldown.getCurrentTime(), 0.5f);
            // Heavy
            cooldownUI.transform.GetChild(2).gameObject.transform.GetChild(0)
                .gameObject.transform.GetChild(0).GetComponent<SimpleHealthBar>().UpdateBar(heavyCooldown.getCurrentTime(), 0.8f);
            // Special
            cooldownUI.transform.GetChild(3).gameObject.transform.GetChild(0)
                .gameObject.transform.GetChild(0).GetComponent<SimpleHealthBar>().UpdateBar(fireCooldown.getCurrentTime(), 0.2f);

            // Freeze constraints after doing basic moves

            if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Light") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Medium") 
            || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Fire")) {
                if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun") && !gameObject.GetComponent<Damage>().knockbacking && !gameObject.GetComponent<PlayerJumpPush>().isColliding)
                    gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }

            if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) damageWait.cancel();
          
        }

        private void FixedUpdate()
        {
            if (anim.GetBool("Won") || anim.GetCurrentAnimatorStateInfo(0).IsName("Win") || anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) return;
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = 0;
            if (gameObject == player1) h = CrossPlatformInputManager.GetAxis("Horizontal");
            if (gameObject == player2) h = CrossPlatformInputManager.GetAxis("Horizontal2");
            
        
            // if(m_Dodge) 
            // {
            //     if(h > 0) 
            //         h = 5;
            //     else if (h < 0) 
            //         h = (-5);
            // }

            // Pass all parameters to the character control script.
            if (!gameObject.GetComponent<Damage>().knockbacking && !m_Character.preventMovement)
                m_Character.Move(h, crouch, m_Jump);

            if(m_Jump && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping") && !audioData[3].isPlaying && !jumpActive)
                audioData[3].Play();

            m_Jump = false;
            m_Dodge = false;

            
        }

        private void Flip()
        {
            m_FacingRight = !m_FacingRight;
            transform.Rotate(0f, 180f, 0f);
        }

 
        void Shoot()
        {

            if(damageWait.isInitial()) 
            {
                anim.SetTrigger("Fire");
                damageWait.startCooldown(Shoot, 0.2f);
            }

            if(!damageWait.isInitial()) 
            {

            GameObject ballClone = Instantiate(fireBallPrefab, firePoint.position, firePoint.rotation);
            ballClone.transform.localScale = transform.localScale;
            stamina.staminaDecrease(20f);
            }
        }

        void Light() 
        {

            // gameObject.GetComponent<MovePlayer>().moveFacingDirection(50, 0.5f);

            
            if(damageWait.isInitial()) 
            {
                if (tripleJab.beforeLast()) {
                anim.SetTrigger("Light");
                
                if (anim.GetNextAnimatorStateInfo(0).IsName("Idle") || anim.GetNextAnimatorStateInfo(0).IsName("Walk") || anim.GetNextAnimatorStateInfo(0).IsName("Run")) { 
                    animEnd.startCooldown(slowAnimSpeed, anim.GetCurrentAnimatorStateInfo(0).length);
                } else {
                    anim.speed = 0.1f;
                }
                
                damageWait.startCooldown(Light, 0.1f);
                } else {
                    anim.SetTrigger("Light");
                    damageWait.startCooldown(Light, 0.01f);
                }
            }

            if(!damageWait.isInitial()) 
            {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                if (!enemy.isTrigger && enemy.offset.y != 1.19f) {
                AudioSource.PlayClipAtPoint(audioData[0].clip, gameObject.transform.position);
                if (!tripleJab.notLast()) {
                    enemy.GetComponent<Damage>().doDamage(2.5f, 4f);
                } else {
                    enemy.GetComponent<Damage>().doDamage(1.5f, 1f);
                }
            }
            }
            }
        }
    

        void Medium() 
        {
            if(damageWait.isInitial()) 
            {
                anim.SetTrigger("Medium");
                damageWait.startCooldown(Medium, 0.2f);
            }

            if(!damageWait.isInitial()) 
            {
                if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) return;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(shortAttackPoint.position, basicAttackRange, enemyLayers);

                foreach(Collider2D enemy in hitEnemies)
                {
                    if (!enemy.isTrigger && enemy.offset.y != 1.19f) {
                    AudioSource.PlayClipAtPoint(audioData[2].clip, gameObject.transform.position);
                    enemy.GetComponent<Damage>().doDamage(4f, 2.5f);
                    }
                }
            }
        }

        void Heavy() 
        {
            
            if(damageWait.isInitial())
            {
                anim.SetTrigger("Heavy");
                damageWait.startCooldown(Heavy, 0.2f);
            }
      
            if(!damageWait.isInitial()) 
            {
                if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) return;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(shortAttackPoint.position, basicAttackRange, enemyLayers);
                foreach(Collider2D enemy in hitEnemies)
                {   
                    if (!enemy.isTrigger && enemy.offset.y != 1.19f) {
                    AudioSource.PlayClipAtPoint(audioData[1].clip, gameObject.transform.position);
                    enemy.GetComponent<Damage>().doDamage(8f, 4f);
                    }
                }
            }
        
        }

        private void setAnimSpeed(float x) {
            anim.speed = x;
        }
        private void slowAnimSpeed() {
            anim.speed = 0.1f;
        }

    //     private void OnTriggerEnter2D(Collider2D other) {
    //     if(isColliding) return;
    //         isColliding = true;
    //  // Rest of the code
    //     if (other.tag == "Player" && other.name != gameObject.name) {
    //         // Debug.Log("EPIC");
    //         // m_Character = other.GetComponentInParent<PlatformerCharacter2D>();
    //         m_Character.preventMovement = true;
    //         wait.startCooldown(0.5f);
    //         startedWait = true;
    //         float velocity = 10f;
    //         if (!m_Character.m_FacingRight) {
    //             velocity = velocity * -1;
    //         }
    //         m_Character.m_Rigidbody2D.velocity = new Vector2(velocity, m_Character.m_Rigidbody2D.velocity.y);
    //         Debug.Log(other.name);
            
    //     }
    // }

        void OnDrawGizmosSelected()
        {
            if(basicAttackPoint == null)
            return;
            Gizmos.DrawWireSphere(basicAttackPoint.position, basicAttackRange);
            Gizmos.DrawWireSphere(shortAttackPoint.position, basicAttackRange);
        }
    }        
}
