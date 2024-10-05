using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityDefinition : MonoBehaviour
{
    public abstract AbilityInfo GetInfo();
    
    public struct AbilityInfo
    {
        public AbilityType abilityType; 
        public string infoText;
    }

    public enum AbilityType
    {
        Active,
        Passive
    }
}
