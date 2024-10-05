using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ModifyMass : AbilityDefinition
{
    [SerializeField] private float massFactor;
    
    private void Start()
    {
        GetComponent<Rigidbody2D>().mass *= massFactor;
    }
}
