using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//This is the script that Mr Taylor Uses for movement

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class VonDerEheFightControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool m_Dodge;
        public bool lariatActive;                 //Is this timer active?

        private SimpleHealthBar playerHealthBar;
        private SimpleHealthBar staminaBar;
        private Stamina stamina;
        private GameObject cooldownUI;

        // public float lariatCooldown = 0.5f;              //How often this cooldown may be used
        // public float lariatTimer;
      

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
        }


        private void Update()
        {
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

            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun") && !moveActive.active()) {
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
                    lightCooldown.startCooldown(0.2f);
                    moveActive.startCooldown(0.1f);
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
                    moveActive.startCooldown(0.5f);
                }
        }


        // Updates Cooldown UI
            cooldownUI = m_Character.cooldownUI;

           
            // Light
            cooldownUI.transform.GetChild(0).gameObject.transform.GetChild(0)
            .gameObject.transform.GetChild(0).GetComponent<SimpleHealthBar>().UpdateBar(lightCooldown.getCurrentTime(), 0.2f);
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

            if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Light") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Medium") 
            || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Lariat")) {
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }


            }
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
                enemy.GetComponent<Damage>().doDamage(20f, 0.5f);

            }
        }
        void Light() 
        {
            anim.SetTrigger("Light");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                //AudioSource.PlayClipAtPoint(audioData[0].clip, gameObject.transform.position);
                enemy.GetComponent<Damage>().doDamage(1.5f, 0.5f);

            }
        }


        void Medium() 
        {
            if(damageWait.isInitial()) {
             anim.SetTrigger("Medium");
             damageWait.startCooldown(Medium, 0.2f);
            }
            if(!damageWait.isInitial()) {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);

                foreach(Collider2D enemy in hitEnemies)
                {
                    //AudioSource.PlayClipAtPoint(audioData[2].clip, gameObject.transform.position);
                    enemy.GetComponent<Damage>().doDamage(4f, 0.5f);

                }
            }
        }

        void Heavy() 
        {

            if(damageWait.isInitial()) {
                anim.SetTrigger("Heavy");
                damageWait.startCooldown(Heavy, 0.2f);
            }
        }   

        void OnDrawGizmosSelected()
        {

            if(basicAttackPoint == null)
            return;

            Gizmos.DrawWireSphere(basicAttackPoint.position, basicAttackRange);
        }
    }
        
}