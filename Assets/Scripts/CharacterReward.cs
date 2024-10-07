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
        characterName.name = characterPrefab.name;
        characterImage.sprite = characterPrefab.GetComponent<SpriteRenderer>().sprite;
    }
}
