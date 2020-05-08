/*
    @author Caleb Hardy

    Loads selected characters into level
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerLoad : MonoBehaviour
{

    public CharacterManager characterManager;
    public Transform spawn1, spawn2;
    public GameObject player1, player2, barLeft, barRight;
  
    void Awake()
    {
        characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
        player1 = Instantiate(characterManager.players[0].playerprefab, spawn1.position, spawn1.rotation);
        player2 = Instantiate(characterManager.players[1].playerprefab, spawn2.position, spawn2.rotation);

        // Player 1 Setup
        PlatformerCharacter2D m_Character1 = player1.GetComponent<PlatformerCharacter2D>();
        m_Character1.Bar = barLeft;
        m_Character1.healthBarObject = barLeft.transform.Find("healthBarLeft").gameObject;
        m_Character1.staminaBarObject = barLeft.transform.Find("StaminaLeft").gameObject;
        m_Character1.cooldownUI = GameObject.Find("CooldownLeft");
        foreach(CharacterBase character in characterManager.characterList) {
            if (character.prefab == characterManager.players[0].playerprefab) {
                barLeft.transform.Find("Head").gameObject.GetComponent<SpriteRenderer>().sprite = character.displayIcon.GetComponent<SpriteRenderer>().sprite;
                
              
                
            }

        }

        // Player 2 Setup
        PlatformerCharacter2D m_Character2 = player2.GetComponent<PlatformerCharacter2D>();
        m_Character2.Bar = barRight;
        m_Character2.healthBarObject = barRight.transform.Find("healthBarRight").gameObject;
        m_Character2.staminaBarObject = barRight.transform.Find("StaminaRight").gameObject;
        m_Character2.cooldownUI = GameObject.Find("CooldownRight");
        m_Character2.m_FacingRight = false;
        if (player1.name == player2.name)
            player2.transform.localScale = new Vector3(player2.transform.localScale.x * -1, player2.transform.localScale.y, player2.transform.localScale.z);


        foreach(CharacterBase character in characterManager.characterList) {
            if (character.prefab == characterManager.players[1].playerprefab) {
                barRight.transform.Find("Head").gameObject.GetComponent<SpriteRenderer>().sprite = character.displayIcon.GetComponent<SpriteRenderer>().sprite;
                
              
                
            }

        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
