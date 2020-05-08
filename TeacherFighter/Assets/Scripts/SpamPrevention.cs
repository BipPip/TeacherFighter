/*
    @author Caleb Hardy

    Can be used to prevent moves from being spammed (used for triple light jab)
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpamPrevention : MonoBehaviour
{
    // Start is called before the first frame update


    private int moveCount;
    private int moveCountMax;
    private float spamCooldownDuration;
    private Cooldown moveSpamCooldown;
    private float moveCooldownAmount;
    private bool isLast;

   
    void Start()
    {
        moveSpamCooldown = gameObject.AddComponent<Cooldown>();
        moveCount = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!moveSpamCooldown.active())
                moveCount = 0;
        
    }

    public void init(int max, float duration) {
        this.moveCountMax = max;
        this.spamCooldownDuration = duration;
    }

    public void run() {
        moveCount++;
        if (moveCount < moveCountMax) {
        moveSpamCooldown.startCooldown(spamCooldownDuration);
        isLast = false;
        }
        else {
        moveCount = 0;
        isLast = true;
                       
         }
    }

    public int getCount() {
        return moveCount;
    }

    public bool beforeLast() {
        return (moveCount == moveCountMax - 1);
    }
  

    public bool notLast() {
        return !isLast;
    }
}
