using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject abilityPanel;


    private void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        abilityPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        abilityPanel.SetActive(false);

    }
}
