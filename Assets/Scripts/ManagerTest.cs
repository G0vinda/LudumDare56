using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerTest : MonoBehaviour
{
    [SerializeField] private List<CharacterInput> characterInputs;
    [SerializeField] private CharacterManager characterManager;

    private void Start()
    {
        characterManager.SetupCharacters(characterInputs);
    }
}
