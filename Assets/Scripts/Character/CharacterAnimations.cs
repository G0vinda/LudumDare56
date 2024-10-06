using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(CharacterJumping))]
public class CharacterAnimations : MonoBehaviour
{
    [SerializeField] private float landingAnimationTime;
    [SerializeField] private float landingAnimationMinSize;
    [SerializeField] private float jumpAnimationTime;
    [SerializeField] private float jumpAnimationMaxSize;
    
    private CharacterMovement _characterMovement;
    private CharacterJumping _characterJumping;
    private Tween _activeAnimation;
    private bool _waitingForLanding;
    
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _characterJumping = GetComponent<CharacterJumping>();
    }

    private void OnEnable()
    {
        _characterMovement.OnLanding += PlayLandingAnimation;
        _characterJumping.OnJump += PlayJumpAnimation;
    }

    private void OnDisable()
    {
        _characterMovement.OnLanding -= PlayLandingAnimation;
        _characterJumping.OnJump -= PlayJumpAnimation;
    }

    private void PlayLandingAnimation()
    {
        if (!_waitingForLanding)
            return;
        
        CancelCurrentAnimation();
        var landingSequence = DOTween.Sequence();
        landingSequence.Append(transform.DOScaleY(landingAnimationMinSize, landingAnimationTime * 0.5f).SetEase(Ease.OutSine));
        landingSequence.Append(transform.DOScaleY(1, landingAnimationTime * 0.5f).SetEase(Ease.InSine));
        
        _waitingForLanding = false;
    }
    
    private void PlayJumpAnimation()
    {
        CancelCurrentAnimation();
        var jumpSequence = DOTween.Sequence();
        jumpSequence.Append(transform.DOScaleY(jumpAnimationMaxSize, jumpAnimationTime * 0.5f).SetEase(Ease.InSine));
        jumpSequence.Append(transform.DOScaleY(1, jumpAnimationTime * 0.5f).SetEase(Ease.OutSine));
        
        _waitingForLanding = true;
    }
    
    private void CancelCurrentAnimation()
    {
        _activeAnimation?.Kill();
        transform.localScale = Vector3.one;
    }
}
