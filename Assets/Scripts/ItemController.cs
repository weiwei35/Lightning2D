using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public TMP_Text buffIcon;
    public TMP_Text buffName;
    public TMP_Text buffDesc;
    public TMP_Text buffStory;
    SelectItem itemCurrent;
    SelectItemUI select;
    public void SetItemInfo(SelectItem item) {
        buffIcon.text = item.name[0].ToString();
        buffName.text = item.name;
        buffDesc.text = item.desc;
        buffStory.text = item.story;
        itemCurrent = item;
    }
    private void Start() {
        select = GameObject.FindWithTag("EndLevel").GetComponent<SelectItemUI>();
    }
    public void ChooseItem(){
        select.SaveChooseItem(itemCurrent);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        select.PlayAudio(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        select.PlayAudio(1);
    }
}
