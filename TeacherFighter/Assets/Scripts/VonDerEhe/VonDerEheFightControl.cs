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

        void LariatAttack()
        {
        
            anim.SetTrigger("LariatAttack");
            stamina.startCountdown(1f);
            stamina.staminaDecrease(45f);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Damage>().doDamage(20f, 0.5f);

            }
        }   

        void OnDrawGizmosSelected()
        {

            if(attackPoint == null)
            return;

            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
        
}
