using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterReward : MonoBehaviour
{
    public CharacterInput characterPrefab;
    public Button selectButton;
    
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] private Image characterImage;

    public void Initialize(CharacterInput characterPrefab)
    {
        this.characterPrefab = characterPrefab;
        characterName.text = characterPrefab.gameObject.name;
        characterImage.sprite = characterPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
    }
}
