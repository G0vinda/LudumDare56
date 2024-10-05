using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityDefinition : MonoBehaviour
{
    [field: SerializeField] public AbilityInfoSO Info { get; private set; }

    public enum AbilityType
    {
        Active,
        Passive
    }
}
