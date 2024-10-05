using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject selectionArrow;
    [SerializeField] private GameObject qHint;
    [SerializeField] private GameObject eHint;

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

