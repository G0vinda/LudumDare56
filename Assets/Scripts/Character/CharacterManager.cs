using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public event Action<CharacterInput> OnCharacterChanged;
    private List<CharacterInput> _characters = new();
    
    private int _currentCharacterIndex;
    private bool _characterAreSetup;

    public static CharacterManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelectPreviousCharacter();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            SelectNextCharacter();
        }
    }

    public void SetupCharacters(List<CharacterInput> characters)
    {
        _characters.Clear();
        _characters.AddRange(characters);
        _characterAreSetup = true;
        SelectCharacter(0);
    }

    private void SelectPreviousCharacter()
    {
        var newCharacterIndex = _currentCharacterIndex - 1;
        if (newCharacterIndex < 0)
            newCharacterIndex = _characters.Count - 1;

        SelectCharacter(newCharacterIndex);
    }

    private void SelectNextCharacter()
    {
        var newCharacterIndex = _currentCharacterIndex + 1;
        if (newCharacterIndex > _characters.Count - 1)
            newCharacterIndex = 0;

        SelectCharacter(newCharacterIndex);
    }

    private void SelectCharacter(int newCharacterIndex)
    {
        if (!_characterAreSetup)
        {
            Debug.LogWarning("Characters need to be setup before character selection");
            return;
        }
        
        _characters[_currentCharacterIndex].enabled = false;
        var newCharacter = _characters[newCharacterIndex];
        newCharacter.enabled = true;
        _currentCharacterIndex = newCharacterIndex;
        CameraManager.Instance.SwitchCameraFollowTarget(newCharacter.transform);
        OnCharacterChanged?.Invoke(newCharacter);
        SetSelectionUI();
    }

    private void SetSelectionUI()
    {
        var selected = _characters[_currentCharacterIndex].GetComponentInChildren<SelectionUI>();
        selected.ShowAsSelected();
        
        var nextIndex = (_currentCharacterIndex + 1) % _characters.Count;
        var next = _characters[nextIndex].GetComponentInChildren<SelectionUI>();
        next.ShowAsNext();
        
        var previousIndex = (_currentCharacterIndex - 1) >= 0 ? _currentCharacterIndex - 1 : _characters.Count - 1;
        var previous = _characters[previousIndex].GetComponentInChildren<SelectionUI>();
        previous.ShowAsPrevious();
    }



   
}
