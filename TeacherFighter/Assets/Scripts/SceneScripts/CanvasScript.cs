using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VariablesCanva{
    public class CanvasScript : MonoBehaviour
    {
        public GameObject leftHealthBarObject;
        public GameObject rightHealthBarObject;

        public GameObject leftCooldown;
        public static GameObject CooldownLeft;
        public GameObject rightCooldown;
        public static GameObject CooldownRight;
        public static SimpleHealthBar healthBarLeft;
        public static SimpleHealthBar healthBarRight;
        void Start()
        {
            // healthBarLeft = leftHealthBarObject.GetComponent<SimpleHealthBar>();
            // healthBarRight = rightHealthBarObject.GetComponent<SimpleHealthBar>();
        
            // CooldownRight = rightCooldown;
            // CooldownLeft = leftCooldown;
        }

    }
}
