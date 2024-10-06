using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterJumping))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float coyoteTimeLimit = 0.1f;
    [SerializeField] private float inputMovementAirMultiplier = 0.5f;
    
    public float movementSpeed;
    public Transform groundChecker, ceilingChecker;
    public float groundCheckRadius, ceilingCheckerRadius;
    public LayerMask groundLayer;
    public float frictionStrength;
    public bool isGrounded;
    public ParticleSystem dustParticles;
    
    public float HorizontalInput {get; set;}
    public float CoyoteTime { get; set; }
    
    public event Action OnLanding;
    public event Action<float> OnRotated;

    private CharacterJumping _characterJumping;
    private Rigidbody2D _rb;
    private Vector2 _extraVelocity;
    private Vector2 _inputVelocity;
    private CharacterInput _characterInput;
    private List<Collider2D> _hitGroundColliders = new();
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _characterJumping = GetComponent<CharacterJumping>();
        _characterInput = GetComponent<CharacterInput>();
    }
    
    private void Update()
    {
        if (!_characterInput.enabled) 
            return;
        
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        _inputVelocity = new Vector2(HorizontalInput * movementSpeed, 0);

        if (HorizontalInput > 0)
        {
            var yRotation = 0;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRotation, transform.localEulerAngles.z);
            OnRotated?.Invoke(yRotation);
        }
        else if (HorizontalInput < 0)
        {
            var yRotation = 180;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRotation, transform.localEulerAngles.z);
            OnRotated?.Invoke(yRotation);
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
            _inputVelocity = Vector2.zero;

        Vector2 movementAmount;
        if (isGrounded)
        {
             movementAmount = (_inputVelocity + _extraVelocity) * Time.fixedDeltaTime;
        }
        else
        {
             movementAmount = (_inputVelocity*inputMovementAirMultiplier + _extraVelocity) * Time.fixedDeltaTime;
        }
        
        _rb.MovePosition(_rb.position + movementAmount);
    }
    
    private void CheckIfHittingCeiling()
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
        if (isGrounded)
        {
            CoyoteTime = Time.time+coyoteTimeLimit;
        }
        else
        {
            _extraVelocity += Vector2.up * Physics2D.gravity.y * _rb.mass * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (LayerIsInLayerMask(other.gameObject.layer, groundLayer))
        {
            var collisionNormal = other.contacts[0].normal;
            if (collisionNormal == Vector2.up)
            {
                ResetYVelocity();
                _hitGroundColliders.Add(other.collider);

                if (!isGrounded)
                {
                    isGrounded = true;
                    OnLanding?.Invoke();    
                }
            }
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (_hitGroundColliders.Contains(other.collider))
        {
            _hitGroundColliders.Remove(other.collider);
            if (_hitGroundColliders.Count == 0)
                isGrounded = false;
        }
    }

    private bool LayerIsInLayerMask(int layer, LayerMask layerMask)
    {
        return ((1 << layer) & layerMask) != 0;
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
        else if(_extraVelocity.x < 0)
        {
            direction = -1;
        }

        _extraVelocity -= Vector2.right * (direction*frictionStrength) * Time.fixedDeltaTime;

        if (Mathf.Abs(_extraVelocity.x) < 0.05f)
        {
            _extraVelocity = new Vector2(0, _extraVelocity.y);
        }
    }
}
