using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterJumping : MonoBehaviour
{
    [SerializeField] private float jumpHeight;

    public int MaxJumps
    {
        get => _maxJumps;
        set
        {
            var difference = value - _maxJumps;
            _maxJumps = value;
            _jumpsLeft += difference;
        }
    }

    private int _maxJumps = 2;
    private int _jumpsLeft;
    
    private CharacterMovement _characterMovement;
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _jumpsLeft = _maxJumps;
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
        return _jumpsLeft > 0;
    }

    public void Jump()
    {
        _jumpsLeft--;
        _characterMovement.ApplyForce(Vector2.up*Mathf.Sqrt(-2*Physics2D.gravity.y*jumpHeight), false);
    }
    
    private void ResetJumpsLeft()
    {
        _jumpsLeft = _maxJumps;
    }
}
