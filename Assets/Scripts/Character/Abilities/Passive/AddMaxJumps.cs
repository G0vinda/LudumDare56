using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterJumping))]
public class AddMaxJumps : AbilityDefinition
{
    private void Start()
    {
        GetComponent<CharacterJumping>().MaxExtraJumps = 1;
    }
}
