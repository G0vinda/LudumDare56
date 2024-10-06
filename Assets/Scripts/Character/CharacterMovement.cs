using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterJumping))]
public class CharacterMovement : MonoBehaviour
{
    public float movementSpeed;
    public Transform groundChecker, ceilingChecker;
    public float groundCheckRadius, ceilingCheckerRadius;
    public LayerMask groundLayer;

    public float HorizontalInput { get; set; }

    public float frictionStrength;
    public event Action OnLanding;

    private CharacterJumping _characterJumping;
    private Rigidbody2D _rb;
    private Vector2 _extraVelocity;
    private Vector2 _inputVelocity;
    public bool isGrounded;

    [HideInInspector]
    public float coyoteTime;

    private CharacterInput _characterInput;

    [SerializeField]
    private float coyoteTimeLimit = 0.1f;


    [SerializeField]
    private float inputMovementAirMultiplier = 0.5f;


    public ParticleSystem dustParticles;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _characterJumping = GetComponent<CharacterJumping>();
        _characterInput = GetComponent<CharacterInput>();
    }
    private void Update()
    {
        if (!_characterInput.enabled) return;
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        _inputVelocity = new Vector2(HorizontalInput * movementSpeed, 0);

        if (HorizontalInput > 0)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        }
        else if (HorizontalInput < 0)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 180, transform.localEulerAngles.z);

        }

        if (Math.Abs(HorizontalInput) > 0.1f)
        {
            if (!dustParticles.isPlaying && isGrounded)
            {
                dustParticles.Play();

            }
        }
        else
        {
            if (dustParticles.isPlaying)
            {
                dustParticles.Stop();
            }
        }

        //   CheckIfIsGrounded();
    }


    public void OnJumpInput()
    {
        if (_characterJumping.CanJump())
            _characterJumping.Jump();
    }

    public float GetGroundBouncyFactor()
    {
        var hitList = Physics2D.OverlapCircleAll(groundChecker.position, groundCheckRadius);
        foreach (var item in hitList)
        {
            if (item.TryGetComponent<BeingBouncy>(out var bouncy))
            {
                return bouncy.BounceFactor;
            }
        }

        return 1f;
    }

    private void FixedUpdate()
    {

        ApplyGravity();
        ApplyFriction();
        CheckIfHittingCeiling();
        if (!_characterInput.enabled)
        {
            _inputVelocity = Vector2.zero;
        }

        Vector2 movementAmount = Vector2.zero;

        if (isGrounded)
        {
            movementAmount = (_inputVelocity + _extraVelocity) * Time.fixedDeltaTime;

        }
        else
        {
            movementAmount = (_inputVelocity * inputMovementAirMultiplier + _extraVelocity) * Time.fixedDeltaTime;

        }

        _rb.MovePosition(_rb.position + movementAmount);
    }



    public void CheckIfHittingCeiling()
    {
        var hitlist= Physics2D.OverlapCircleAll(ceilingChecker.position, ceilingCheckerRadius, groundLayer);
        bool hitCeiling = false;
        foreach (var hit in hitlist)
        {
            if (hit.gameObject != this.gameObject)
            {
                hitCeiling = true;
            }
        }
        if (hitCeiling)
        {
            _extraVelocity.y = -2;
        }
    }
    private void ApplyGravity()
    {
        CheckIfIsGrounded();
        if (isGrounded)
        {

        }
        else
        {
            _extraVelocity += Vector2.up * Physics2D.gravity.y * _rb.mass * Time.fixedDeltaTime;
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
            coyoteTime = Time.time + coyoteTimeLimit;
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

    public void ResetXVelocity()
    {
        _extraVelocity.x = 0;
    }

    private void ApplyFriction()
    {
        int direction = 0;
        if (_extraVelocity.x > 0)
        {
            direction = 1;
        }
        else if (_extraVelocity.x < 0)
        {
            direction = -1;
        }

        _extraVelocity -= Vector2.right * (direction * frictionStrength) * Time.fixedDeltaTime;

        if (_extraVelocity.x < 0.05f)
        {
            _extraVelocity = new Vector2(0, _extraVelocity.y);
        }
    }
}
