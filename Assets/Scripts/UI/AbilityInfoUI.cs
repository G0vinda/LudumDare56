using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(RectTransform))]
public class AbilityInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text abilityInfoText;
    [SerializeField] private float maxTextWidth = 250f;
    [SerializeField] private Vector2 padding;
    
    private RectTransform _rectTransform;
    
    public void Initialize(AbilityDefinition.AbilityInfo info)
    {
        _rectTransform = GetComponent<RectTransform>();
        abilityInfoText.text = info.infoText;

        abilityInfoText.rectTransform.sizeDelta = new Vector2(maxTextWidth, abilityInfoText.rectTransform.sizeDelta.y);
        abilityInfoText.ForceMeshUpdate();

        var preferredHeight = abilityInfoText.preferredHeight;
        _rectTransform.sizeDelta = new Vector2(maxTextWidth + 2*padding.x, preferredHeight + 2*padding.y);
    }
}
