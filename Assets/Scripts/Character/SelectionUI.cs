using UnityEngine;

public class SelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject selectionArrow;
    [SerializeField] private GameObject qHint;
    [SerializeField] private GameObject eHint;
    
    private CharacterMovement _characterMovement;

    private void Awake()
    {
        _characterMovement = GetComponentInParent<CharacterMovement>();
        if(_characterMovement == null)
            Debug.LogError("SelectionUI could not find characterMovement in parent!");
    }

    private void OnEnable()
    {
        _characterMovement.OnRotated += RotateAgainstCharacterRotation;
    }
    
    private void OnDisable()
    {
        _characterMovement.OnRotated -= RotateAgainstCharacterRotation;
    }

    private void RotateAgainstCharacterRotation(float characterYRotation)
    {
        var selectionYRotation = 0f;
        if (characterYRotation != 0)
        {
            selectionYRotation = -characterYRotation;
        }

        transform.localEulerAngles = new Vector3(0, selectionYRotation, 0);
    }

    public void ShowAsSelected()
    {
        selectionArrow.SetActive(true);
        qHint.SetActive(false);
        eHint.SetActive(false);
    }
    
    public void ShowAsNext()
    {
        selectionArrow.SetActive(false);
        qHint.SetActive(false);
        eHint.SetActive(true);
    }
    
    public void ShowAsPrevious()
    {
        selectionArrow.SetActive(false);
        qHint.SetActive(true);
        eHint.SetActive(false);
    }
}

