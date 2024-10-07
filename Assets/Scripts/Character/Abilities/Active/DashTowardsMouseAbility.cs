using UnityEngine;
using UnityEngine.Serialization;

public class DashTowardsMouseAbility : ActiveAbility
{
    [SerializeField] private LayerMask inputLayer;
    [SerializeField] private float dashForce;
    private CharacterMovement _characterMovement;
    
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    public override void CastAbility(Vector2 inputPosition)
    {
        var ray = Camera.main.ScreenPointToRay(inputPosition);
        var hitResult = Physics2D.Raycast(ray.origin, ray.direction, 100f, inputLayer);
        
        var dashDirection = (hitResult.point - (Vector2)transform.position).normalized;
        _characterMovement.ApplyForce(dashDirection * dashForce, false);
        
        AudioPlayer.instance.PlayAudio(SoundID.Dash);
    }
}
