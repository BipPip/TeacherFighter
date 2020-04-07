using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using System;
 public class FPSTarget : MonoBehaviour
 {
     
     public int target = 60;
     
     void Awake()
     {
         QualitySettings.vSyncCount = 1;
         Application.targetFrameRate = target;
     }
     
     void FixedUpdate()
     {
         if(Application.targetFrameRate != target)
             Application.targetFrameRate = target;
     }
 }
