using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class CarryAndThrow : ActiveAbility
{
    [SerializeField] private LayerMask inputLayer;
    
    public override void CastAbility(Vector2 inputPosition)
    {
        var ray = Camera.main.ScreenPointToRay(inputPosition);
        var hitResult = Physics2D.Raycast(ray.origin, ray.direction, 100f, inputLayer);

        var carryDirection = (hitResult.point - (Vector2)transform.position).normalized;
    }
}
