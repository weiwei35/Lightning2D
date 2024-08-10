using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemIconDoor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text icon;
    public GameObject tips;
    public TMP_Text itemName;
    public TMP_Text desc;
    RectTransform rectTransform;

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        tips.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tips.SetActive(false);
    }
}
