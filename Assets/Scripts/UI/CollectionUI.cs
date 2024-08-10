using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : MonoBehaviour
{
    public ItemDataSO itemData;

    public GameObject ItemList1;
    public GameObject ItemList2;
    public GameObject ItemList3;

    public GameObject itemPre;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in itemData.items)
        {
            if(item.specialType == 1){
                var itemList = Instantiate(itemPre);
                CollectionItem collectionItem = itemList.GetComponent<CollectionItem>();
                collectionItem.itemIcon.text = item.name[0].ToString();
                collectionItem.itemId = item.id;
                itemList.transform.SetParent(ItemList1.transform);
                itemList.transform.localScale = new Vector3(1,1,1);
            }
        }
        foreach (var item in itemData.items)
        {
            if(item.specialType == 2){
                var itemList = Instantiate(itemPre);
                CollectionItem collectionItem = itemList.GetComponent<CollectionItem>();
                collectionItem.itemIcon.text = item.name[0].ToString();
                collectionItem.itemId = item.id;
                itemList.transform.SetParent(ItemList2.transform);
                itemList.transform.localScale = new Vector3(1,1,1);
            }
        }
        foreach (var item in itemData.items)
        {
            if(item.specialType == 3){
                var itemList = Instantiate(itemPre);
                CollectionItem collectionItem = itemList.GetComponent<CollectionItem>();
                collectionItem.itemIcon.text = item.name[0].ToString();
                collectionItem.itemId = item.id;
                itemList.transform.SetParent(ItemList3.transform);
                itemList.transform.localScale = new Vector3(1,1,1);
            }
        }
    }

    public GameObject itemInfo;
    public GameObject itemNotfound;
    public TMP_Text itemName;
    public TMP_Text itemDesc;
    public TMP_Text itemStory;
    RectTransform rectTransform;


    public void ShowItemInfo(int id) {
        rectTransform = GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        //已发现，未解锁
        if(PlayerPrefs.GetInt(id.ToString()) == 1){
            itemInfo.SetActive(true);
            itemNotfound.SetActive(false);
            itemName.text = "?????";
            itemDesc.text = "*******************************";
            itemStory.text = "*******************************";
        }
        //解锁
        else if(PlayerPrefs.GetInt(id.ToString()) == 2){
            itemInfo.SetActive(true);
            itemNotfound.SetActive(false);

            itemName.text = itemData.GetSelectItemById(id).name;
            itemDesc.text = itemData.GetSelectItemById(id).desc;
            itemStory.text = itemData.GetSelectItemById(id).story;
        }
        else{
            itemInfo.SetActive(false);
            itemNotfound.SetActive(true);
        }
    }
    public void HideItemInfo(){
        itemInfo.SetActive(false);
        itemNotfound.SetActive(false);
    }
}
