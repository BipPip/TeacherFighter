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
        private bool mediumActive, heavyActive, jumpActive, lightActive;

        public Animator anim;
        public Transform basicAttackPoint;
        public Transform longAttackPoint;
        public Transform shortAttackPoint;
        public float basicAttackRange = 0.5f;
        public LayerMask enemyLayers;

        private AudioSource[] audioData;
        private Component[] audioArray;
    
        private void Awake()
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
            moveActive = gameObject.AddComponent<Cooldown>();
            tripleJab = gameObject.AddComponent<SpamPrevention>();
            tripleJab.init(3, 0.5f);
        }


        private void Update()
        {
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

            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun") && !moveActive.active() && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Block")) {
            if(Input.GetButtonDown("Vonder_Lariat"))
            {
                if (stamina.getStamina() >= 45f && gameObject.GetComponent<PlatformerCharacter2D>().m_Grounded) {
                    if(!lariatCooldown.active()) 
                    {
                        LariatAttack();
                        lariatCooldown.startCooldown(1f);
                        // lariatActive = true;
                        // lariatTimer = lariatCooldown;
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
                    moveActive.startCooldown(0.2f);
                }
            }
            else if (Input.GetButtonDown("Vonder_Heavy")) {
                if (!heavyCooldown.active()) {
                    Heavy();
                    heavyCooldown.startCooldown(0.8f);
                    moveActive.startCooldown(0.3f);
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
                if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun") && !gameObject.GetComponent<Damage>().knockbacking)
                    gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            // else {
            //     disableMove = false;
            // }
        // Debug.Log(gameObject.GetComponent<Rigidbody2D>().constraints);
            
            
        }

            
        

        private void FixedUpdate()
        {
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
            if (!gameObject.GetComponent<Damage>().knockbacking)
                m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
            m_Dodge = false;
            
        }

        void LariatAttack()
        {
        
            anim.SetTrigger("LariatAttack");
            stamina.startCountdown(1f);
            stamina.staminaDecrease(45f);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Damage>().doDamage(20f, 5f);

            }
        }
        void Light() 
        {

        

            
            if(damageWait.isInitial()) 
            {
                if (tripleJab.beforeLast()) {
                anim.SetTrigger("Light");
                anim.speed = 0.1f;
                
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
                // AudioSource.PlayClipAtPoint(audioData[0].clip, gameObject.transform.position);
                if (!tripleJab.notLast()) {
                    enemy.GetComponent<Damage>().doDamage(2.5f, 4f);
                } else {
                    enemy.GetComponent<Damage>().doDamage(1.85f, 2f);
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
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(longAttackPoint.position, basicAttackRange, enemyLayers);

                foreach(Collider2D enemy in hitEnemies)
                {
                    //AudioSource.PlayClipAtPoint(audioData[2].clip, gameObject.transform.position);
                    enemy.GetComponent<Damage>().doDamage(5f, 3f);

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
                // AudioSource.PlayClipAtPoint(audioData[1].clip, gameObject.transform.position);
                enemy.GetComponent<Damage>().doDamage(10f, 3.5f);

            }
        }
        
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