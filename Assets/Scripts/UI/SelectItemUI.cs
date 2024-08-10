using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SelectItemUI : MonoBehaviour
{
    public InfoUIController uIController;
    // public ItemIcon itemIcon;
    public ItemDataSO itemData;
    public ItemLevelRankSO itemLevelRank;
    public GameObject selectBar;
    public GameObject selectPrefab;
    SelectItem saveItem;
    List<SelectItem> items = new List<SelectItem>();
    PlayerController player;
    public GameSaveManager gameSave;
    public GameController gameController;
    // Start is called before the first frame update
    void OnEnable() {
        Animation animation = GetComponent<Animation>();
        animation.Play("Pick3Show");
        PlayAudio(2);
        // itemData = AssetDatabase.LoadAssetAtPath<ItemDataSO>("Assets/Resources/ItemData.asset");
        if(Global.exp_level > 0 || Global.exp > 0){
            if(Global.continueGame && gameSave.data.isEndLevel){
                items.Clear();
                foreach (var item in gameSave.data.selectItemId)
                {
                    items.Add(itemData.GetSelectItemById(item));
                }
                Global.continueGame = false;
            }else{
                List<int> specType = new List<int>();
                List<int> detailType = new List<int>();
                int type = 0;
                if(Global.isChangeLevel){
                    type = 2;
                }else{
                    type = 1;
                }

                for (int i = 0; i < 3; i++)
                {
                    int spec = itemLevelRank.GetItemSpecType(gameController.levelId,type);
                    specType.Add(spec);
                    if(spec == 3){
                        int detail = itemLevelRank.GetTreasureType(gameController.levelId,type);
                        detailType.Add(detail);
                    }else if(spec == 1){
                        int detail = itemLevelRank.GetPieceType(gameController.levelId,type);
                        detailType.Add(detail);
                    }
                }
                items = itemData.GetRandomSelectItem(3,specType,detailType);
                gameSave.data.selectItemId.Clear();
                foreach (var item in items)
                {
                    gameSave.data.selectItemId.Add(item.id);
                    PlayerPrefs.SetInt(item.id.ToString(),1);
                }
                gameSave.Save();
            }
        }else{
            items.Clear();
            items.Add(itemData.GetRandomTrans());
        }
        
        ShowSelectItem();
    }
    void ShowSelectItem(){
        for (int i = 0; i < items.Count; i++)
        {
            var selectItem = Instantiate(selectPrefab);
            selectItem.transform.SetParent(selectBar.transform);
            selectItem.transform.localScale = new Vector3(1,1,1);
            selectItem.transform.localPosition = Vector3.zero;
            ItemController item = selectItem.GetComponent<ItemController>();
            item.SetItemInfo(items[i]);
            if(items.Count == 1){
                saveItem = items[0];
            }else{
                saveItem = null;
            }
        }
    }
    public void SaveChooseItem(SelectItem item){
        saveItem = item;
    }
    public bool SetPlayerStatus(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(saveItem != null)
        {
            SwitchUI();
            SwitchLogic();
            gameSave.data.currentItemId.Add(saveItem.id);
            PlayerPrefs.SetInt(saveItem.id.ToString(),2);
            return true;
        }else{
            return false;
        }
    }
    public void LoadItem() {
        if(Global.continueGame){
            foreach (var item in gameSave.data.currentItemId)
            {
                LoadItemUI(itemData.GetSelectItemById(item));
                LoadItemLogic(itemData.GetSelectItemById(item));
            }
        }
    }
    void LoadItemUI(SelectItem currentItem){
        //处理UI显示
            switch (currentItem.specialType) {
                case 1:
                    Global.item1Current.Add(currentItem);
                    int count = 0;
                    foreach (var item in Global.item1Current)
                    {
                        Debug.Log(item.name);
                        if(item.type == currentItem.type){
                            count++;
                        }
                    }
                    if(count > 1)
                    {
                        uIController.AddItem1(currentItem.name[0].ToString(),count,currentItem.type);
                    }else{
                        uIController.SetItem1(currentItem.name[0].ToString(),currentItem.name.ToString(),currentItem.desc.ToString(),currentItem.type);
                    }
                    return;
                case 2:
                    Global.item2Current.Add(currentItem);
                    uIController.SetItem2(currentItem.name[0].ToString(),currentItem.name.ToString(),currentItem.desc.ToString());
                    return;
                case 3:
                    Global.item3Current.Add(currentItem);
                    uIController.SetItem3(currentItem.name[0].ToString(),currentItem.name.ToString(),currentItem.desc.ToString());
                    return;
                case 4:
                    Global.item3Current.Add(currentItem);
                    uIController.SetItem3(currentItem.name[0].ToString(),currentItem.name.ToString(),currentItem.desc.ToString());
                    return;
            }
    }

    void SwitchUI(){
        //处理UI显示
            switch (saveItem.specialType) {
                case 1:
                    Global.item1Current.Add(saveItem);
                    int count = 0;
                    foreach (var item in Global.item1Current)
                    {
                        if(item.type == saveItem.type){
                            count++;
                        }
                    }
                    if(count > 1)
                    {
                        uIController.AddItem1(saveItem.name[0].ToString(),count,saveItem.type);
                    }else{
                        uIController.SetItem1(saveItem.name[0].ToString(),saveItem.name.ToString(),saveItem.desc.ToString(),saveItem.type);
                    }
                    return;
                case 2:
                    Global.item2Current.Add(saveItem);
                    uIController.SetItem2(saveItem.name[0].ToString(),saveItem.name.ToString(),saveItem.desc.ToString());
                    return;
                case 3:
                    Global.item3Current.Add(saveItem);
                    uIController.SetItem3(saveItem.name[0].ToString(),saveItem.name.ToString(),saveItem.desc.ToString());
                    return;
                case 4:
                    Global.item3Current.Add(saveItem);
                    uIController.SetItem3(saveItem.name[0].ToString(),saveItem.name.ToString(),saveItem.desc.ToString());
                    return;
            }
    }

    void LoadItemLogic(SelectItem currentItem){
        //处理数值逻辑
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            switch (currentItem.type) {
                case 201:
                    player.SetCircleCopy();
                    return;
                case 202:
                    player.SetOnceLightningCopy();
                    return;
                case 203:
                    player.SetOnceTimeCopy();
                    return;
                case 204:
                    player.SetMegaCopy();
                    return;
                case 301:
                    player.SetLightningMirror();
                    return;
                case 302:
                    player.SetLightningBoom();
                    return;
                case 303:
                    player.SetLightningAttract();
                    return;
                case 304:
                    player.SetLightningOverflow();
                    return;
                case 305:
                    player.SetLightningBoomPlayer();
                    return;
                case 401:
                    player.SetPaperFireBall();
                    return;
                case 402:
                    player.SetPaperIce();
                    return;
                case 403:
                    player.SetPaperHP();
                    return;
                case 404:
                    player.SetPaperProtect();
                    return;
                case 405:
                    player.SetPaperBlack();
                    return;
                case 406:
                    player.SetPaperConnect();
                    return;
                case 501:
                    player.SetBugHP();
                    return;
                case 502:
                    player.SetBugCircle();
                    return;
                case 503:
                    player.SetBugWall();
                    return;
                case 504:
                    player.SetBugFollow();
                    return;
                case 505:
                    player.SetBugMerge();
                    return;
                case 506:
                    player.SetBugAttack();
                    return;
                case 601:
                    player.SetDebuffSlow();
                    return;
                case 602:
                    player.SetDebuffDizzy();
                    return;
                case 603:
                    player.SetBuffSuper();
                    return;
                case 604:
                    player.SetMoveRandom();
                    return;
                case 1001:
                    player.speedToLSpeed = true;
                    return;
                case 1002:
                    player.speedToLCount = true;
                    return;
                case 1003:
                    player.hpToLCount = true;
                    return;
                case 1004:
                    player.hpToLHurt = true;
                    return;
                case 1005:
                    player.protectToLHurt = true;
                    return;
                case 1006:
                    player.protectToLSpeed = true;
                    return;
                default:
                    return;
            }
    }
    void SwitchLogic(){
        //处理数值逻辑
            switch (saveItem.type) {
                case 1:
                    player.SetHP(saveItem.buff);
                    return;
                case 2:
                    player.SetSpeed(saveItem.buff);
                    return;
                case 3:
                    player.SetProtect(saveItem.buff);
                    return;
                case 5:
                    player.SetHPSpeed(saveItem.buff);
                    return;
                case 6:
                    player.SetProtectSpeed(saveItem.buff);
                    return;
                case 201:
                    player.SetCircleCopy();
                    return;
                case 202:
                    player.SetOnceLightningCopy();
                    return;
                case 203:
                    player.SetOnceTimeCopy();
                    return;
                case 204:
                    player.SetMegaCopy();
                    return;
                case 301:
                    player.SetLightningMirror();
                    return;
                case 302:
                    player.SetLightningBoom();
                    return;
                case 303:
                    player.SetLightningAttract();
                    return;
                case 304:
                    player.SetLightningOverflow();
                    return;
                case 305:
                    player.SetLightningBoomPlayer();
                    return;
                case 401:
                    player.SetPaperFireBall();
                    return;
                case 402:
                    player.SetPaperIce();
                    return;
                case 403:
                    player.SetPaperHP();
                    return;
                case 404:
                    player.SetPaperProtect();
                    return;
                case 405:
                    player.SetPaperBlack();
                    return;
                case 406:
                    player.SetPaperConnect();
                    return;
                case 501:
                    player.SetBugHP();
                    return;
                case 502:
                    player.SetBugCircle();
                    return;
                case 503:
                    player.SetBugWall();
                    return;
                case 504:
                    player.SetBugFollow();
                    return;
                case 505:
                    player.SetBugMerge();
                    return;
                case 506:
                    player.SetBugAttack();
                    return;
                case 601:
                    player.SetDebuffSlow();
                    return;
                case 602:
                    player.SetDebuffDizzy();
                    return;
                case 603:
                    player.SetBuffSuper();
                    return;
                case 604:
                    player.SetMoveRandom();
                    return;
                case 1001:
                    player.speedToLSpeed = true;
                    return;
                case 1002:
                    player.speedToLCount = true;
                    return;
                case 1003:
                    player.hpToLCount = true;
                    return;
                case 1004:
                    player.hpToLHurt = true;
                    return;
                case 1005:
                    player.protectToLHurt = true;
                    return;
                case 1006:
                    player.protectToLSpeed = true;
                    return;
                default:
                    return;
            }
    }
    private void OnDisable() {
        items.Clear();
        saveItem = new SelectItem();
        Transform select = selectBar.transform; // 获取当前GameObject的Transform
        for (int i = 0; i < select.childCount; i++) // 遍历所有子物体
        {
            Transform child = select.GetChild(i); // 获取子物体的Transform
            Destroy(child.gameObject); // 打印子物体的名字
        }
    }

    public AudioSource audioSource;
    public AudioClip[] audios;
    public void PlayAudio(int index) {
        audioSource.PlayOneShot(audios[index]);
    }
}
