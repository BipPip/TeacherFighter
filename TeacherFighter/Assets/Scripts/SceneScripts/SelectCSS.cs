using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SelectCSS : MonoBehaviour
{

    public List<Character> characters = new List<Character>();
    public GameObject charCellPrefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Character character in characters)
        {
            SpawnCharacterCell(character);
        }
    }

    
    private void SpawnCharacterCell(Character character)
    {
        GameObject charCell = Instantiate(charCellPrefab, transform);

        Image artwork = charCell.transform.Find("artwork").GetComponent<Image>();
        TextMeshProUGUI name = charCell.transform.Find("nameRect").GetComponentInChildren<TextMeshProUGUI>();

        artwork.sprite = character.CharacterSprite;
        name.text = character.characterName;
    }
}
