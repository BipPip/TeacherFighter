using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class VonDerEheFightControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool m_Dodge;
        public bool lariatActive;                 //Is this timer active?
        public bool disableMove;

        public bool startedWait;
        public bool isColliding;
        private Cooldown wait;

        private SimpleHealthBar playerHealthBar;
        private SimpleHealthBar staminaBar;
        private Stamina stamina;
        private GameObject cooldownUI;

        // public float lariatCooldown = 0.5f;              //How often this cooldown may be used
        // public float lariatTimer;
        private float lightCooldownAmount;
        private SpamPrevention tripleJab;

        private Vector3 startPosition;

        private Cooldown lariatCooldown;
        private Cooldown lightCooldown;
        private Cooldown mediumCooldown;
        private Cooldown heavyCooldown;
        private Cooldown moveActive; // Used because there is a slight delay between anim.trigger and the actual animation returning active
        private Cooldown damageWait;
        private Cooldown animEnd;
        private bool mediumActive, heavyActive, jumpActive, lightActive;

        public Animator anim;
        public Transform basicAttackPoint;
        public Transform longAttackPoint;
        public Transform shortAttackPoint;
        public float basicAttackRange = 0.5f;
        public LayerMask enemyLayers;

        private AudioSource[] audioData;
        private Component[] audioArray;
       
    
        private void Start()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            startPosition = transform.position;
            playerHealthBar = gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>();

            staminaBar = gameObject.GetComponent<PlatformerCharacter2D>().staminaBarObject.GetComponent<SimpleHealthBar>();
            stamina = gameObject.GetComponent<Stamina>();

            audioArray = gameObject.GetComponents(typeof(AudioSource));
            audioData = new AudioSource[audioArray.Length];
            
            for(int i = 0; i < audioArray.Length; i++) {
                audioData[i] = (AudioSource) audioArray[i];
            }

            heavyCooldown = gameObject.AddComponent<Cooldown>();
            mediumCooldown = gameObject.AddComponent<Cooldown>();
            lightCooldown = gameObject.AddComponent<Cooldown>();
            lariatCooldown = gameObject.AddComponent<Cooldown>();
            damageWait = gameObject.AddComponent<Cooldown>();
            animEnd = gameObject.AddComponent<Cooldown>();
            moveActive = gameObject.AddComponent<Cooldown>();
            tripleJab = gameObject.AddComponent<SpamPrevention>();
            tripleJab.init(3, 0.5f);
            wait = gameObject.AddComponent<Cooldown>();
         
        }


        private void Update()
        {
            
            if (anim.GetBool("Won") || anim.GetCurrentAnimatorStateInfo(0).IsName("Win") || anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) return;
                

            // Debug.Log(m_Character.m_Rigidbody2D.velocity.y);
            float distanceToLeft = GameObject.Find("/PlatformLeft").transform.position.x;
            distanceToLeft = distanceToLeft - (gameObject.transform.position.x + gameObject.GetComponent<CapsuleCollider2D>().size.x);

            float distanceToRight = GameObject.Find("/PlatformRight").transform.position.x;
            distanceToRight = distanceToRight - (gameObject.transform.position.x + gameObject.GetComponent<CapsuleCollider2D>().size.x);

            if ((distanceToLeft > -4.57 && m_Character.m_FacingRight) || (distanceToRight <= 10 && !m_Character.m_FacingRight)) {
                m_Character.nearWall = true;
                // gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = false;
                gameObject.GetComponents<BoxCollider2D>()[1].offset = new Vector2(0f, 1.19f);
                if (m_Character.m_Rigidbody2D.velocity.y > -21f && !m_Character.m_Grounded && ((distanceToLeft > -4.57 && !m_Character.m_FacingRight) || (distanceToRight <= 10 && m_Character.m_FacingRight))) 
                        m_Character.m_Rigidbody2D.velocity = new Vector2(m_Character.m_Rigidbody2D.velocity.x, m_Character.m_Rigidbody2D.velocity.y - 0.5f);
               
            }
            else {
                m_Character.nearWall = false;
                // gameObject.GetComponents<BoxCollider2D>()[1].isTrigger = true;
                gameObject.GetComponents<BoxCollider2D>()[1].offset = new Vector2(-0.1f, 1.19f);
            }

            // if (!wait.active() && startedWait == true) {
            //     m_Character.preventMovement = false;
            //     startedWait = false;
            //     isColliding = false;
            // }
            // Debug.Log(m_Character.m_Rigidbody2D.velocity.x);
            if(!moveActive.active()) {
                anim.speed = 1f;
            }

            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump2");
            }

            if (m_Character.m_Grounded) {
                jumpActive = false;
            }
            else {
                jumpActive = true;
            }
            
            
            // if(lariatActive)
            //   lariatTimer -= Time.deltaTime;    //Subtract the time since last frame from the timer.
            // if (lariatTimer < 0) {
            //  lariatTimer = 0;                  //If timer is less than 0, reset it to 0 as we don't want it to be negative
            //     lariatActive = false;
            // }


            heavyActive = this.anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy");
    
            mediumActive = this.anim.GetCurrentAnimatorStateInfo(0).IsName("Medium");

            lightActive = this.anim.GetCurrentAnimatorStateInfo(0).IsName("Light");


            // Handle Inputs
            // Debug.Log(gameObject.GetComponent<PlayerJumpPush>().isColliding);
            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun") && !moveActive.active() && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Block") && !gameObject.GetComponent<PlayerJumpPush>().isColliding) {
            
            if(Input.GetButtonDown("Vonder_Lariat"))
            {
                if (stamina.getStamina() >= 45f && gameObject.GetComponent<PlatformerCharacter2D>().m_Grounded) {
                    if(!lariatCooldown.active()) 
                    {
                        LariatAttack();
                        lariatCooldown.startCooldown(1f);
                        // lariatActive = true;
                        // lariatTimer = lariatCooldown;
                        moveActive.startCooldown(0.4f);
                    }
                }
            }
            else if (Input.GetButtonDown("Vonder_Light") && !heavyActive && !mediumActive) {
                if (!lightCooldown.active()) {


                    Light();
                        tripleJab.run();
                        if(tripleJab.notLast()) {
                            lightCooldown.startCooldown(0.2f);
                            lightCooldownAmount = 0.2f;
                            moveActive.startCooldown(0.1f);
                        }
                        else {
                            lightCooldown.startCooldown(0.8f);
                            lightCooldownAmount = 0.8f;
                            moveActive.startCooldown(0.3f);
                        }

                    
                }
            }
            else if (Input.GetButtonDown("Vonder_Medium") && !heavyActive && !lightActive) {
                if (!mediumCooldown.active()) {
                    Medium();
                    mediumCooldown.startCooldown(0.5f);
                    moveActive.startCooldown(0.5f);
                }
            }
            else if (Input.GetButtonDown("Vonder_Heavy")) {
                if (!heavyCooldown.active()) {
                    Heavy();
                    heavyCooldown.startCooldown(0.8f);
                    moveActive.startCooldown(0.8f);
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
            .gameObject.transform.GetChild(0).GetComponent<SimpleHealthBar>().UpdateBar(lariatCooldown.getCurrentTime(), 1f);

            // Freeze constraints after doing basic moves

            // if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Light") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Medium") 
            // || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Lariat")) {
            //     gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            // }

            // Freeze constraints after doing basic moves

            if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Light") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Medium") 
            || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Lariat")) {
                // disableMove = true;
                if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun") && !gameObject.GetComponent<Damage>().knockbacking && !gameObject.GetComponent<PlayerJumpPush>().isColliding)
                    gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            // else {
            //     disableMove = false;
            // }
        // Debug.Log(gameObject.GetComponent<Rigidbody2D>().constraints);
            if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) damageWait.cancel();
            
        }

            
        

        private void FixedUpdate()
        {
            if (anim.GetBool("Won") || anim.GetCurrentAnimatorStateInfo(0).IsName("Win") || anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) return;

            if(Input.GetKey("o"))
            {
                Debug.Log("wow you found a secret thats so cool");
            }

            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.RightControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal2");
            
            if(m_Dodge) 
            {
                if(h > 0) 
                    h = 5;
                else if (h < 0) 
                    h = (-5);
            }

            // Pass all parameters to the character control script.
            if (!gameObject.GetComponent<Damage>().knockbacking && !m_Character.preventMovement)
                m_Character.Move(h, crouch, m_Jump);

            if(m_Jump && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping") && !audioData[3].isPlaying && !jumpActive)
                audioData[3].Play();
            
            m_Jump = false;
            m_Dodge = false;
            
            // if (m_Character.m_Rigidbody2D.velocity.x != 0)
            //     Debug.Log(m_Character.m_Rigidbody2D.velocity.x);
            //   if (m_Character.preventMovement)
            //     Debug.Log(m_Character.preventMovement);
        }

        void LariatAttack()
        {
        
            anim.SetTrigger("LariatAttack");
            stamina.startCountdown(1f);
            stamina.staminaDecrease(45f);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                if (!enemy.isTrigger && enemy.offset.y != 1.25f) {
                AudioSource.PlayClipAtPoint(audioData[4].clip, gameObject.transform.position);
                enemy.GetComponent<Damage>().doDamage(20f, 5f);
                }
            }
        }
        void Light() 
        {

        

            
            if(damageWait.isInitial()) 
            {
                if (tripleJab.beforeLast()) {
                anim.SetTrigger("Light");
                if (anim.GetNextAnimatorStateInfo(0).IsName("Idle") || anim.GetNextAnimatorStateInfo(0).IsName("Walk") || anim.GetNextAnimatorStateInfo(0).IsName("Run")) { 
                    animEnd.startCooldown(slowAnimSpeed, anim.GetCurrentAnimatorStateInfo(0).length);
                } else {
                    anim.speed = 0.1f;
                }
                
                damageWait.startCooldown(Light, 0.15f);
                } else {
                    anim.SetTrigger("Light");
                    damageWait.startCooldown(Light, 0.01f);
                }
            }

            if(!damageWait.isInitial()) 
            {
            if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) return;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                if (!enemy.isTrigger && enemy.offset.y != 1.25f) {
                AudioSource.PlayClipAtPoint(audioData[0].clip, gameObject.transform.position);
                if (!tripleJab.notLast()) {
                    enemy.GetComponent<Damage>().doDamage(2.5f, 4f);
                } else {
                    enemy.GetComponent<Damage>().doDamage(1.85f, 2f);
                }
            }
            }
            }
        }


        void Medium() 
        {
            if(damageWait.isInitial()) {
             anim.SetTrigger("Medium");
             damageWait.startCooldown(Medium, 0.2f);
            }
            if(!damageWait.isInitial()) {
                if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) return;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(longAttackPoint.position, basicAttackRange, enemyLayers);

                foreach(Collider2D enemy in hitEnemies)
                {
                    if (!enemy.isTrigger && enemy.offset.y != 1.25f) {
                    AudioSource.PlayClipAtPoint(audioData[1].clip, gameObject.transform.position);
                    enemy.GetComponent<Damage>().doDamage(5f, 3f);
                    }
                }
            }
        }

        void Heavy() 
    {
        
        if(damageWait.isInitial()) {
            anim.SetTrigger("Heavy");
            damageWait.startCooldown(Heavy, 0.5f);
        }
      
        if(!damageWait.isInitial()) {
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(shortAttackPoint.position, basicAttackRange, enemyLayers);
            foreach(Collider2D enemy in hitEnemies)
            {   
                if (!enemy.isTrigger && enemy.offset.y != 1.25f) {
                AudioSource.PlayClipAtPoint(audioData[2].clip, gameObject.transform.position);
                enemy.GetComponent<Damage>().doDamage(10f, 3.5f);
                }

            }
        }
        
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

        private void setAnimSpeed(float x) {
            anim.speed = x;
        }
        private void slowAnimSpeed() {
            anim.speed = 0.1f;
        }
        void OnDrawGizmosSelected()
        {

            if(basicAttackPoint == null)
            return;

            Gizmos.DrawWireSphere(basicAttackPoint.position, basicAttackRange);
            Gizmos.DrawWireSphere(longAttackPoint.position, basicAttackRange);
            Gizmos.DrawWireSphere(shortAttackPoint.position, basicAttackRange);
        }
    }
        
}