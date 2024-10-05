using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterInput : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    private ActiveAbility _activeAbility;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _activeAbility = GetComponent<ActiveAbility>();
    }

    private void Update()
    {
        _characterMovement.HorizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _characterMovement.OnJumpInput();
        }

        if (Input.GetMouseButtonDown(0) && _activeAbility != null)
        {
            _activeAbility.CastAbility(Input.mousePosition);
        }
    }
}
