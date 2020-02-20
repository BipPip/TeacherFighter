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

        private Vector3 startPosition;
        
        private Cooldown lightCooldown;
        private Cooldown mediumCooldown;
        private Cooldown heavyCooldown;
        private Cooldown moveActive; // Used because there is a slight delay between anim.trigger and the actual animation returning active
        private Cooldown damageWait;
        private bool mediumActive, heavyActive, jumpActive;

        private SimpleHealthBar playerHealthBar;
        private SimpleHealthBar staminaBar;
        private Stamina stamina;

        public float lariatCooldown = 0.5f;              //How often this cooldown may be used
        public float lariatTimer;
        public float attackRange = 0.5f;

        private Vector3 startPosition;


        public Animator anim;
        public Transform attackPoint;
        public LayerMask enemyLayers;
    
        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            startPosition = transform.position;
            playerHealthBar = gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>();

            staminaBar = gameObject.GetComponent<PlatformerCharacter2D>().staminaBarObject.GetComponent<SimpleHealthBar>();
            stamina = gameObject.GetComponent<Stamina>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump2");
            }
            
            
            if(lariatActive)
              lariatTimer -= Time.deltaTime;    //Subtract the time since last frame from the timer.
            if (lariatTimer < 0) {
             lariatTimer = 0;                  //If timer is less than 0, reset it to 0 as we don't want it to be negative
                lariatActive = false;
            }


            // Lariat Attack input
            if(Input.GetButtonDown("Vonder_Lariat"))
            {
                if (stamina.getStamina() >= 45f && gameObject.GetComponent<PlatformerCharacter2D>().m_Grounded) {
                    if(!lariatActive) 
                    {
                        LariatAttack();
                        lariatActive = true;
                        lariatTimer = lariatCooldown;
                    }
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
        
        void Light() 
        {
            anim.SetTrigger("Light");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                AudioSource.PlayClipAtPoint(audioData[0].clip, gameObject.transform.position);
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
                    AudioSource.PlayClipAtPoint(audioData[2].clip, gameObject.transform.position);
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
        
}
