using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Abilities/Ability Info")]
public class AbilityInfoSO : ScriptableObject
{
    public AbilityDefinition.AbilityType abilityType;
    public string abilityDescription;
}
