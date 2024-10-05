using UnityEngine;

public class DashTowardsMouseAbility : ActiveAbility
{
    [SerializeField] private LayerMask _inputLayer;
    [SerializeField] private float dashForce;
    private CharacterMovement _characterMovement;
    
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }
    
    public override AbilityInfo GetInfo()
    {
        var info = new AbilityInfo
        {
            abilityType = AbilityType.Active,
            infoText = "You can left-click with your mouse to perform a dash into that direction."
        };

        return info;
    }

    public override void CastAbility(Vector2 inputPosition)
    {
        var ray = Camera.main.ScreenPointToRay(inputPosition);
        var hitResult = Physics2D.Raycast(ray.origin, ray.direction, 100f, _inputLayer);
        
        var dashDirection = (hitResult.point - (Vector2)transform.position).normalized;
        _characterMovement.ApplyForce(dashDirection * dashForce, false);
    }
}
