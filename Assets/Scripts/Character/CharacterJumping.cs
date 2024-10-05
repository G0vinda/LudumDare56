using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterJumping : MonoBehaviour
{
    [SerializeField] private float jumpHeight;
    
    private CharacterMovement _characterMovement;
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    public virtual bool CanJump()
    {
        return _characterMovement.IsGrounded();
    }

    public void Jump()
    {
        _characterMovement.ApplyForce(Vector2.up*Mathf.Sqrt(-2*Physics2D.gravity.y*jumpHeight), false);
    }
}
