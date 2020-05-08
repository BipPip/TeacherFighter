/*
    @author Tia Flores-Carr

    Stores all information for the prefabs
    and players. This is also used to spawn
    players in the level selected. This should
    always be in player levels as a transform 
    or event system
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int numberOfUsers;

    public List<PlayerBase> players = new List<PlayerBase>();
    public List<CharacterBase> characterList = new List<CharacterBase>();

    public CharacterBase returnCharacterWithID(string id)
    {
        CharacterBase retVal = null;

        for(int i = 0; i < characterList.Count; i++)
        {
            if(string.Equals(characterList[i].CharID, id))
            {
                retVal = characterList[i];
            }
            
        }
        return retVal;
    }

    
    public static CharacterManager instance;
    public static CharacterManager getInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}

[System.Serializable]
public class CharacterBase
{
    public string CharID;
    public GameObject prefab;

    public GameObject displayIcon;
}

[System.Serializable]
public class PlayerBase
{
    public string playerId;
    public string inputId;
    public bool hasCharacter;
    public GameObject playerprefab;

    
    //public int score; // not sure if well need this later but here it is

}

