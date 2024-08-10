using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text itemIcon;
    //PlayerPrefs.SetBool("PlayerLevel", true);
    public GameObject lockIcon;
    public int itemId;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt(itemId.ToString()) == 1 ||PlayerPrefs.GetInt(itemId.ToString()) == 2){
            lockIcon.gameObject.SetActive(false);
        }else{
            lockIcon.gameObject.SetActive(true);
        }
        collection = GameObject.FindWithTag("CollectionPanel").GetComponent<CollectionUI>();
    }

    public CollectionUI collection;
    public void SaveItemId() {
        collection.ShowItemInfo(itemId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        collection.ShowItemInfo(itemId);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        collection.HideItemInfo();
    }
}
