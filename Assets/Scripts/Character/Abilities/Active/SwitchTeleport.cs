using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwitchTeleport : ActiveAbility
{
    [SerializeField] private LayerMask characterLayer;
    
    public override void CastAbility(Vector2 inputPosition)
    {
        var ray = Camera.main.ScreenPointToRay(inputPosition);
        var hitResult = Physics2D.Raycast(ray.origin, ray.direction, 100f, characterLayer);

        if (hitResult.collider == null)
            return;

        var otherCharacter = hitResult.transform;
        var otherPosition = otherCharacter.position;
        gameObject.SetActive(false);
        otherCharacter.position = transform.position;
        transform.position = otherPosition;
        gameObject.SetActive(true);
    }
}
