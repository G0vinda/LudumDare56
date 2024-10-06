using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterJumping : MonoBehaviour
{
    [SerializeField] private float jumpHeight;
    [SerializeField] private int maxExtraJumps;
    [SerializeField] private float jumpBufferTimeLimit;

    public int MaxExtraJumps
    {
        get => maxExtraJumps;
        set
        {
            var difference = value - maxExtraJumps;
            maxExtraJumps = value;
            _extraJumpsLeft += difference;
        }
    }
    
    public float JumpBufferTime { get; set; }
    
    private int _extraJumpsLeft;
    private CharacterMovement _characterMovement;
    
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _extraJumpsLeft = maxExtraJumps;
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

        JumpBufferTime = Time.time + jumpBufferTimeLimit;
        return false; 
    }

    private void OnLand()
    {
        if(JumpBufferTime>Time.time)
        {

            Jump();
        }
    }

    public void Jump()
    {
        _characterMovement.ResetYVelocity();
        var jumpForce = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight) * Physics2D.gravity.normalized.y;
        var bouncyFactor = _characterMovement.GetGroundBouncyFactor();
        _characterMovement.ApplyForce(Vector2.up*jumpForce*bouncyFactor, false);
        _characterMovement.ApplyForce(Vector2.up * Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight) * Physics2D.gravity.normalized.y, false);

    }

    private void ResetJumpsLeft()
    {
        _extraJumpsLeft = maxExtraJumps;
    }
}
