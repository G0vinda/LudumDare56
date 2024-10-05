using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : MonoBehaviour
{
    public abstract void CastAbility(Vector2 inputPosition);
}
