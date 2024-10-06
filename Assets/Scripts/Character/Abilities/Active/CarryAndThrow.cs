using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(CharacterMovement))]
public class CarryAndThrow : ActiveAbility
{
    [SerializeField] private LayerMask inputLayer;
    [SerializeField] private float grabDistance;
    [SerializeField] private Transform carryPoint;
    [SerializeField] private float throwForce;
    
    private CharacterMovement _grabbedCharacter;
    
    public override void CastAbility(Vector2 inputPosition)
    {
        if (_grabbedCharacter != null)
        {
            StartCoroutine(Throw());
        }
        else
        {
            if (TryGetOtherCharacterInRange(inputPosition, out var otherCharacter))
                GrabCharacter(otherCharacter);
        }
    }

    private bool TryGetOtherCharacterInRange(Vector2 inputPosition, out CharacterMovement otherCharacter)
    {
        var ray = Camera.main.ScreenPointToRay(inputPosition);
        var hitResult = Physics2D.Raycast(ray.origin, ray.direction, 100f, inputLayer);
        
        var grabDirection = (hitResult.point - (Vector2)transform.position).normalized;
        var grabTargets = Physics2D.RaycastAll(transform.position, grabDirection, grabDistance);
        var self = GetComponent<CharacterMovement>();
        foreach (var grabTarget in grabTargets)
        {
            if (grabTarget.transform.TryGetComponent<CharacterMovement>(out var characterMovement))
            {
                if(characterMovement == self)
                    continue;

                otherCharacter = characterMovement;
                return true;
            }
        }

        otherCharacter = null;
        return false;
    }

    private void GrabCharacter(CharacterMovement characterToGrab)
    {
        characterToGrab.ResetYVelocity();
        characterToGrab.ResetXVelocity();
        characterToGrab.enabled = false;
        characterToGrab.GetComponent<Collider2D>().enabled = false;
        characterToGrab.GetComponent<Rigidbody2D>().isKinematic = true;

        characterToGrab.transform.position = carryPoint.position;
        characterToGrab.transform.rotation = carryPoint.rotation;
        characterToGrab.transform.SetParent(carryPoint);

        _grabbedCharacter = characterToGrab;
    }

    private IEnumerator Throw()
    {
        _grabbedCharacter.enabled = true;
        _grabbedCharacter.GetComponent<Rigidbody2D>().isKinematic = false;
        _grabbedCharacter.ApplyForce(carryPoint.right * throwForce, false);
        _grabbedCharacter.transform.SetParent(null);
        
        var characterToRelease = _grabbedCharacter;
        _grabbedCharacter = null;
        yield return new WaitForSeconds(0.3f);
        characterToRelease.transform.rotation = Quaternion.identity;
        characterToRelease.GetComponent<Collider2D>().enabled = true;
    }
}
