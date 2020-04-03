using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CharacterSelect : MonoBehaviour
{
    

    public int numberOfPlayers = 2;

    public List<PlayerInterfaces> plInterfaces = new List<PlayerInterfaces>();
    public PortraitInfo[] portraitPrefabs;
    public int maxX;
    public int maxY;
    PortraitInfo[,] charGrid;

    public GameObject portraitCanvas;
    public bool bothPlayersSelected;

    CharacterManager charManager;

    void Start()
    {
        charManager = CharacterManager.getInstance();
        numberOfPlayers = charManager.numberOfUsers;

        charGrid = new PortraitInfo[maxX,maxY];

        int x = 0;
        int y = 0;

        portraitPrefabs = portraitCanvas.GetComponentsInChildren<PortraitInfo>();

        //All the portrait information

        for(int i =0; i < portraitPrefabs.Length; i++)
        {
            //assign grid positions
            portraitPrefabs[i].posX += x;
            portraitPrefabs[i].posY += y;

            charGrid[x,y] = portraitPrefabs[i];

            if(x < maxX -1)
            {
                x++;
            }else
            {
                x = 0;
                y++;
            }
        }
    } 

    void Update()
    {
        for(int i = 0; i < plInterfaces.Count; i++)
        {
            if(i < numberOfPlayers)
            {

                if(Input.GetButtonDown("Cancel" + charManager.players[i].inputId))
                {
                    plInterfaces[i].playerBase.hasCharacter = false;
                }

                if(!charManager.players[i].hasCharacter)
                {
                    plInterfaces[i].playerBase = charManager.players[i];

                    HandleSelectorPosition(plInterfaces[i]);
                    HandleSelectScreenInput(plInterfaces[i], charManager.players[i].inputId);
                    HandleCharacterPreview(plInterfaces[i]);
                }
            }
            else
            {
                charManager.players[i].hasCharacter = true;
            }
        }

        if(bothPlayersSelected)
        {
            Debug.Log("Both Players have been selected");

            SceneManager.LoadScene("LevelSelect");
        }
        else
        {
            if(charManager.players[0].hasCharacter && charManager.players[1].hasCharacter)
            {
                bothPlayersSelected = true;
            }
        }

    }

    void HandleSelectorPosition(PlayerInterfaces pl)
    {
        pl.selector.SetActive(true);

        pl.activePortrait = charGrid[pl.activeX, pl.activeY];

        Vector2 selectorPosition = pl.activePortrait.transform.localPosition;
        selectorPosition = selectorPosition + new Vector2(portraitCanvas.transform.localPosition.x
            , portraitCanvas.transform.localPosition.y);
        
        pl.selector.transform.localPosition = selectorPosition;
    }

    void HandleSelectScreenInput(PlayerInterfaces pl, string playerId)
    {
        float vertical = Input.GetAxis("Vertical" + playerId);

        if(vertical != 0)
        {
            if(!pl.hitInputOnce)
            {
                if(vertical > 0)
                {
                    pl.activeY = (pl.activeY > 0) ? pl.activeY -1 : maxY -1;
                }
                else
                {
                    pl.activeY = (pl.activeY < maxY - 1) ? pl.activeY + 1 : 0;
                }

                pl.hitInputOnce = true;
            }
        }

        float horizontal = Input.GetAxis("Horizontal" + playerId);

        if(horizontal != 0)
        {
            if(!pl.hitInputOnce)
            {
                if(horizontal > 0)
                {
                    pl.activeX = (pl.activeX > 0) ? pl.activeX -1 : maxX -1;
                }
                else
                {
                    pl.activeX = (pl.activeX < maxX - 1) ? pl.activeX + 1 : 0;
                }

                pl.hitInputOnce = true;
            }
        }

        if(vertical == 0 && horizontal == 0)
        {
            pl.hitInputOnce = false;
        }


        // Actual Character Selection

        if(Input.GetButtonDown("Submit" + playerId))
        {
            Debug.Log("player" + playerId + " has selected a character");
           //Passes to Character Manager
           pl.playerBase.playerprefab = charManager.returnCharacterWithID(pl.activePortrait.CharacterId).prefab;

           pl.playerBase.hasCharacter = true;
        }

        

    }

    void HandleCharacterPreview(PlayerInterfaces pl)
    {
        if(pl.previewPortrait != pl.activePortrait)
        {
            if(pl.createdCharacter != null)
            {
                Destroy(pl.createdCharacter);
            }

            GameObject go = Instantiate(
                CharacterManager.getInstance().returnCharacterWithID(pl.activePortrait.CharacterId).displayIcon,
                pl.charVisPos.position,
                Quaternion.identity) as GameObject;
            
            pl.createdCharacter = go;

            pl.previewPortrait = pl.activePortrait;

        }
    }


    [System.Serializable]
    public class PlayerInterfaces{
        public PortraitInfo activePortrait;
        public PortraitInfo previewPortrait;
        public GameObject selector;
        public Transform charVisPos;
        public GameObject createdCharacter;

        public int activeX;
        public int activeY;

        public bool hitInputOnce;
        public float timerToReset;

        public PlayerBase playerBase;
    }
}
