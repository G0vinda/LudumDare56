using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : AbilityDefinition
{
    public abstract void CastAbility(Vector2 inputPosition);
}
