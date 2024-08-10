using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text icon;
    public TMP_Text count;

    public int itemType;//用于叠加同类型道具

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
