using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAbilityOverview : MonoBehaviour
{
    [SerializeField] private AbilityInfoUI abilityInfoUIPrefab;
    [SerializeField] private CharacterManager characterManager;

    private void OnEnable()
    {
        characterManager.OnCharacterChanged += UpdateAbilityInfo;
    }
    
    private void OnDisable()
    {
        characterManager.OnCharacterChanged -= UpdateAbilityInfo;
    }
    
    private void UpdateAbilityInfo(CharacterInput character)
    {
        var abilities = character.GetComponents<AbilityDefinition>();
        var abilityInfos = abilities.Select(abilityDefinition => abilityDefinition.Info).ToArray();
        
        DisplayAbilityInfo(abilityInfos);
    }

    private void DisplayAbilityInfo(AbilityInfoSO[] infosToDisplay)
    {
        DeleteCurrentInfo();
        
        foreach (var info in infosToDisplay)
        {
            var abilityInfoUI = Instantiate(abilityInfoUIPrefab, transform);
            abilityInfoUI.Initialize(info);
        }
    }

    private void DeleteCurrentInfo()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
