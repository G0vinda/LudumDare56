using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterJumping))]
public class CharacterMovement : MonoBehaviour
{
    public float movementSpeed;
    public Transform groundChecker;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    
    public float HorizontalInput {get; set;}

    public float frictionStrength;
    public event Action OnLanding;
    
    private CharacterJumping _characterJumping;
    private Rigidbody2D _rb;
    private Vector2 _extraVelocity;
    private Vector2 _inputVelocity;
    private bool _isGrounded;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _characterJumping = GetComponent<CharacterJumping>();
    }
    private void Update()
    {
        _inputVelocity = new Vector2(HorizontalInput*movementSpeed, 0);
    }

    public void OnJumpInput()
    {
        if(_characterJumping.CanJump())
            _characterJumping.Jump();
    }

    private void FixedUpdate()
    {

        ApplyGravity();
     //   ApplyFriction();
        Vector2 movementAmount= (_inputVelocity + _extraVelocity)*Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position+movementAmount);
    }

    private void ApplyGravity()
    {
        CheckIfIsGrounded();
        if (_isGrounded)
        {
            if (_extraVelocity.y < 0)
            {
                _extraVelocity = new Vector2(_extraVelocity.x, 0);

            }

        }
        else
        {

            _extraVelocity +=Vector2.up*Physics2D.gravity.y*_rb.mass*Time.fixedDeltaTime;
            
        }
    }

    private void CheckIfIsGrounded()
    {
        var isNowGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, groundLayer) != null;

        if (isNowGrounded && !_isGrounded)
            OnLanding?.Invoke();
        
        _isGrounded = isNowGrounded;
    }

    public void ApplyForce(Vector2 forceAmount, bool isImpulse)
    {
        float multiplier = isImpulse ? 50 : 1;
        _extraVelocity += forceAmount * multiplier;
    }

    private void ApplyFriction()
    {
        _extraVelocity = _extraVelocity - _extraVelocity.normalized * frictionStrength;

        if (_extraVelocity.magnitude < 0.05f)
        {
            _extraVelocity = Vector2.zero;
        }
    }
}
