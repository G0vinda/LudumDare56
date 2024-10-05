using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterInput : MonoBehaviour
{
    private CharacterMovement _characterMovement;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        _characterMovement.HorizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _characterMovement.OnJumpInput();
        }
    }
}
