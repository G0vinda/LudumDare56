using UnityEngine;

public class DashTowardsMouseAbility : ActiveAbility
{
    [SerializeField] private float dashForce;
    private CharacterMovement _characterMovement;
    
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    public override void CastAbility(Vector2 inputPosition)
    {
        var dashDirection = (inputPosition - (Vector2)transform.position).normalized;
        _characterMovement.ApplyForce(dashDirection * dashForce, false);
    }
}
