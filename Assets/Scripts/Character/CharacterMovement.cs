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
    public bool isGrounded;

    [HideInInspector]
    public float coyoteTime;


    [SerializeField]
    private float coyoteTimeLimit = 0.1f;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _characterJumping = GetComponent<CharacterJumping>();
    }
    private void Update()
    {
        _inputVelocity = new Vector2(HorizontalInput*movementSpeed, 0);
     //   CheckIfIsGrounded();
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
        if (isGrounded)
        {
          
        }
        else
        {
            _extraVelocity +=Vector2.up*Physics2D.gravity.y*_rb.mass*Time.fixedDeltaTime;
        }
    }

    private void CheckIfIsGrounded()
    {
       var hitList = Physics2D.OverlapCircleAll(groundChecker.position, groundCheckRadius, groundLayer);

        bool isNowGrounded = false;

        foreach (var hit in hitList)
        {
            if (hit.gameObject != this.gameObject)
            {
                isNowGrounded = true;
            }
        }



        if (isNowGrounded && !isGrounded)
        {
            ResetYVelocity();
            OnLanding?.Invoke();

        }


        if (isNowGrounded)
        {
            coyoteTime = Time.time+coyoteTimeLimit;
        }
        isGrounded = isNowGrounded;
    }

    public void ApplyForce(Vector2 forceAmount, bool isImpulse)
    {
        float multiplier = isImpulse ? 50 : 1;
        _extraVelocity += forceAmount * multiplier;
    }

    public void ResetYVelocity()
    {
        _extraVelocity.y = 0;
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
