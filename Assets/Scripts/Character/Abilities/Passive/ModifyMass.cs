using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ModifyMass : AbilityDefinition
{
    [SerializeField] private float massFactor;
    public override AbilityInfo GetInfo()
    {
        var newInfo = new AbilityInfo()
        {
            abilityType = AbilityType.Passive,
            infoText = $"This Creature has {massFactor} times the normal weight."
        };

        return newInfo;
    }

    private void Start()
    {
        GetComponent<Rigidbody2D>().mass *= massFactor;
    }
}
