using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterJumping : MonoBehaviour
{
    [SerializeField] private float jumpHeight;

    public int MaxJumps
    {
        get => _maxExtraJumps;
        set
        {
            var difference = value - _maxExtraJumps;
            _maxExtraJumps = value;
            _extraJumpsLeft += difference;
        }
    }
    [SerializeField]
    private int _maxExtraJumps = 1;
    private int _extraJumpsLeft;

    [HideInInspector]
    public float jumpBufferTime;

    [SerializeField]
    private float jumpBufferTimeLimit;
    private CharacterMovement _characterMovement;
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _extraJumpsLeft = _maxExtraJumps;
        _characterMovement.OnLanding += OnLand;
    }

    private void OnEnable()
    {
        _characterMovement.OnLanding += ResetJumpsLeft;
    }

    private void OnDisable()
    {
        _characterMovement.OnLanding -= ResetJumpsLeft;
    }

    public virtual bool CanJump()
    {
        if (_characterMovement.isGrounded || _characterMovement.coyoteTime>Time.time)
        {
            return true;
        }
        else if(_extraJumpsLeft>0)
        {
            _extraJumpsLeft--;
            return true;
        }

        jumpBufferTime = Time.time + jumpBufferTimeLimit;
        return false;
      
        
    }

    private void OnLand()
    {
        if(jumpBufferTime>Time.time)
        {
            Jump();
        }
    }

    public void Jump()
    {
        _characterMovement.ResetYVelocity();
        _characterMovement.ApplyForce(Vector2.up*Mathf.Sqrt(-2*Physics2D.gravity.y*jumpHeight), false);
    }
    
    private void ResetJumpsLeft()
    {
        _extraJumpsLeft = _maxExtraJumps;
    }
}
