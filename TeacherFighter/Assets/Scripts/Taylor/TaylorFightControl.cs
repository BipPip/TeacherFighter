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

        private Cooldown fireCooldown;
        private Cooldown lightCooldown;
        private Cooldown mediumCooldown;
        private Cooldown heavyCooldown;
        private Cooldown damageWait;
        private bool mediumActive, heavyActive;
        
        private SimpleHealthBar playerHealthBar;
        private SimpleHealthBar staminaBar;
        private Stamina stamina;
        private GameObject cooldownUI;
        public float speed = 20f;
        public Transform basicAttackPoint;
        public float basicAttackRange = 0.5f;
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
            //cooldown  = new Cooldown();
            heavyCooldown = gameObject.AddComponent<Cooldown>();
            mediumCooldown = gameObject.AddComponent<Cooldown>();
            lightCooldown = gameObject.AddComponent<Cooldown>();
            fireCooldown = gameObject.AddComponent<Cooldown>();
            damageWait = gameObject.AddComponent<Cooldown>();
            
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
            

            
            // Handle Inputs

            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stun")) {

            if(Input.GetButtonDown("Taylor_Fire") || Input.GetAxis("Axis 10") != 0 && !heavyActive){
                if (stamina.getStamina() >= 20f) {
                    if (!fireCooldown.active()) {
                        Shoot();
                        stamina.startCountdown(1f);
                        fireCooldown.startCooldown(0.2f);
                    }
                }
            }
            else if (Input.GetButtonDown("Taylor_Light") && !heavyActive) {
                if (!lightCooldown.active()) {
                    Light();
                    lightCooldown.startCooldown(0.2f);
                }
            }
            else if (Input.GetButtonDown("Taylor_Medium") && !heavyActive) {
                if (!mediumCooldown.active()) {
                    Medium();
                    mediumCooldown.startCooldown(0.5f);
                }
            }
            else if (Input.GetButtonDown("Taylor_Heavy")) {
                if (!heavyCooldown.active()) {
                    Heavy();
                    heavyCooldown.startCooldown(0.8f);
                }
            }
        }


            // Detect current active attack
            heavyActive = false;
            mediumActive = false;
            if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Taylor_Heavy"))
                heavyActive = true;
            if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Taylor_Medium"))
                mediumActive = true;
           
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
            .gameObject.transform.GetChild(0).GetComponent<SimpleHealthBar>().UpdateBar(fireCooldown.getCurrentTime(), 0.2f);

            // Freeze constraints after doing basic moves

            if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Taylor_Light") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Taylor_Medium") 
            || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Taylor_Heavy") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Taylor_Fire")) {
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }


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

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Damage>().doDamage(1.5f, 0.5f);

        }
    }
    

    void Medium() 
    {
        
        anim.SetTrigger("Medium");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Damage>().doDamage(4f, 0.5f);

        }
    }

    void Heavy() 
    {
        if(damageWait.isInitial()) {
            anim.SetTrigger("Heavy");
            damageWait.startCooldown(Heavy, 0.5f);
        }
      
        if(!damageWait.isInitial()) {
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);
            foreach(Collider2D enemy in hitEnemies)
            {   
                enemy.GetComponent<Damage>().doDamage(8f, 0.5f);

            }
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
