using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ManagerTest : MonoBehaviour
{
    [SerializeField] private List<CharacterInput> characterInputs;
    [FormerlySerializedAs("characterManager")] [SerializeField] private CharacterSelectionManager characterSelectionManager;

    private void Start()
    {
        characterSelectionManager.SetupCharacters(characterInputs);
    }
}
