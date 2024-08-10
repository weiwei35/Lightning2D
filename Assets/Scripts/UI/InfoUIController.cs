using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoUIController : MonoBehaviour
{
    public TMP_Text saying;
    public TMP_Text protect;
    public TMP_Text hp;
    public TMP_Text speed;
    public TMP_Text lightningHurt;
    public TMP_Text lightningSpeed;
    public TMP_Text lightningCount;
    public GameObject item1Parent;
    public GameObject item2Parent;
    public GameObject item3Parent;
    List<ItemIcon> item1List = new List<ItemIcon>();
    public ItemIcon item;

    
    public void SetSayingText(string text) {
        saying.text = text;
    }
    public void SetProtectText(float text) {
        protect.text = text.ToString();
    }
    public void SetHPText(float text) {
        hp.text = text.ToString();
    }
    public void SetSpeedText(float text) {
        speed.text = text.ToString();
    }
    public void SetLightningHurtText(float text) {
        lightningHurt.text = text.ToString();
    }
    public void SetLightningSpeedText(float text) {
        lightningSpeed.text = text.ToString();
    }
    public void SetLightningCountText(float text) {
        lightningCount.text = text.ToString();
    }

    public void SetItem1(string name,string nameT,string desc,int type) {
        var icon = Instantiate(item.gameObject);
        icon.transform.SetParent(item1Parent.transform);
        icon.transform.localScale = new Vector3(1,1,1);
        icon.GetComponent<ItemIcon>().icon.text = name;
        icon.GetComponent<ItemIcon>().count.gameObject.SetActive(false);
        icon.GetComponent<ItemIcon>().itemName.text = nameT;
        icon.GetComponent<ItemIcon>().desc.text = desc;
        icon.GetComponent<ItemIcon>().itemType = type;
        item1List.Add(icon.GetComponent<ItemIcon>());
    }
    public void AddItem1(string name,int count,int type){
        foreach (var item in item1List)
        {
            if(item.itemType == type){
                item.count.gameObject.SetActive(true);
                item.count.text = count.ToString();
            }
        }
    }
    public void SetItem2(string name,string nameT,string desc) {
        var icon = Instantiate(item.gameObject);
        icon.transform.SetParent(item2Parent.transform);
        icon.transform.localScale = new Vector3(1,1,1);
        icon.GetComponent<ItemIcon>().count.gameObject.SetActive(false);
        icon.GetComponent<ItemIcon>().icon.text = name;
        icon.GetComponent<ItemIcon>().itemName.text = nameT;
        icon.GetComponent<ItemIcon>().desc.text = desc;
    }
    public void SetItem3(string name,string nameT,string desc) {
        var icon = Instantiate(item.gameObject);
        icon.transform.SetParent(item3Parent.transform);
        icon.transform.localScale = new Vector3(1,1,1);
        icon.GetComponent<ItemIcon>().count.gameObject.SetActive(false);
        icon.GetComponent<ItemIcon>().icon.text = name;
        icon.GetComponent<ItemIcon>().itemName.text = nameT;
        icon.GetComponent<ItemIcon>().desc.text = desc;
    }
}
