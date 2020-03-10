using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject display;
    public bool isOver = false;
    GameObject displaySprite;

    void Start(){
        displaySprite = Instantiate(display,new Vector3(-7,2.5f,0), Quaternion.identity);
        displaySprite.transform.localScale = new Vector3(5,5,1); 
        displaySprite.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
            Debug.Log("Taylor Enter");
            displaySprite.SetActive(true);
        

        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
            Debug.Log("Taylor Exit");
            displaySprite.SetActive(false);
        


        isOver = false;
    }

}
