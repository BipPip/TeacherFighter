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

        private Vector3 startPosition;
        private Animator anim;
        public Transform firePoint;
        public GameObject fireBallPrefab;

        private SimpleHealthBar playerHealthBar;
        private SimpleHealthBar staminaBar;
        private Stamina stamina;
        public float speed = 20f;
         public LayerMask enemyLayers;

        private void Start()
         {
           anim = gameObject.GetComponent<Animator>();
           stamina = gameObject.GetComponent<Stamina>();
        }
      
     
        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            startPosition = transform.position;
            anim = gameObject.GetComponent<Animator>();
            playerHealthBar = gameObject.GetComponent<PlatformerCharacter2D>().healthBarObject.GetComponent<SimpleHealthBar>();
            staminaBar = gameObject.GetComponent<PlatformerCharacter2D>().staminaBarObject.GetComponent<SimpleHealthBar>();
            
        }


        private void Update()
        {
            

            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (!m_Dodge)
            {
                // Read the dodge input in Update so button presses aren't missed.
                m_Dodge = CrossPlatformInputManager.GetButtonDown("Taylor_Dodge");
            }
            //Canvas.healthBarLeft.UpdateBar((Canvas.healthBarLeft.GetCurrentFraction * 100) - 10, 100);
            //Debug.Log(Canvas.healthBarLeft.GetCurrentFraction * 100);
            
            // if (playerHealthBar.GetCurrentFraction <= 0) {
            //     //Debug.Log("EE RERER");
            //     gameObject.GetComponent<Damage>();
            //     anim.SetTrigger("Die");
            //     //Destroy(gameObject);
            // }

            if(Input.GetButtonDown("Taylor_Fire")){
                if (stamina.getStamina() >= 20f) {
                    Shoot();
                    stamina.startCountdown(1f);
                }
            }
            else if (Input.GetButtonDown("Taylor_Light")) {
                Light();
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }



            // UnFreeze constraints after doing basic moves


        }


        private void FixedUpdate()
        {
            

            

            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            //Debug.Log(h);
            //Debug.Log(m_Character.transform.);
            if(m_Dodge) {
                if(h > 0) {
                    h = 5;
                }
                else if (h < 0) {
                    h = (-5);
                }
                //Canvas.healthBarLeft.UpdateBar((Canvas.healthBarLeft.GetCurrentFraction * 100) - 10, 100);
            }

            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
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
        GameObject ballClone = Instantiate(fireBallPrefab, firePoint.position, firePoint.rotation);
        ballClone.transform.localScale = transform.localScale;
        anim.SetTrigger("Fire");
        stamina.staminaDecrease(20f);
    }

    void Light() 
    {
        anim.SetTrigger("Light");
    }

    }


    
        
}
