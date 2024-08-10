using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class CirclePanelController : MonoBehaviour
{
    public GameObject player;
    public GameObject center;
    public GameObject start1, start2, start3;
    public float radius1, radius2, radius3;
    public List<SpriteRenderer> sprites1 = new List<SpriteRenderer>();
    public Material light_m;
    public Material dark_m;
    public bool checkingDoor =  true;
    //根据角色距离圆心的距离判断所在环数，根据角色与圆心直线夹角判断所在区域
    public void GetPartByPlayer(){
        float distance = Vector3.Distance(player.transform.position,center.transform.position);
        if(distance < radius1 && distance > 8){
            Vector3 PtoC = player.transform.position - center.transform.position;
            PtoC.Normalize();
            Vector3 StoC = start1.transform.position - center.transform.position;
            StoC.Normalize();
            
            float atan2 = Mathf.Atan2(PtoC.y, PtoC.x) - Mathf.Atan2(StoC.y, StoC.x);

            float anglePS = (atan2 * (180 / Mathf.PI));
            float angle = Mathf.Abs(anglePS);

            if(anglePS < 0){
                sprites1[5].material = dark_m;
                sprites1[6].material = dark_m;
                sprites1[7].material = dark_m;
                if(angle < 45 && angle > 0){
                    sprites1[0].material = light_m;
                    if(saveDoor)
                        inDoor_DU = true;
                }else{
                    inDoor_DU = false;
                    sprites1[0].material = dark_m;
                }
                if(angle < 90 && angle > 45){
                    sprites1[1].material = light_m;
                    if(saveDoor)
                        inDoor_JING = true;
                }else{
                    sprites1[1].material = dark_m;
                    inDoor_JING = false;
                }
                if(angle < 135 && angle > 90){
                    sprites1[2].material = light_m;
                    if(saveDoor)
                        inDoor_SI = true;
                }else{
                    sprites1[2].material = dark_m;
                    inDoor_SI = false;
                }
                if(angle < 180 && angle > 135){
                    sprites1[3].material = light_m;
                    if(saveDoor)
                        inDoor_JING_Bad = true;
                } else{
                    sprites1[3].material = dark_m;
                    inDoor_JING_Bad = false;
                }
                if(angle < 225 && angle > 180){
                    sprites1[4].material = light_m;
                    if(saveDoor)
                        inDoor_KAI = true;
                } else{
                    sprites1[4].material = dark_m;
                    inDoor_KAI = false;
                }
                if(angle > 225){
                    sprites1[5].material = light_m;
                    if(saveDoor)
                        inDoor_XIU = true;
                } else{
                    sprites1[5].material = dark_m;
                    inDoor_XIU = false;
                }
            }else{
                sprites1[4].material = dark_m; 
                sprites1[0].material = dark_m; 
                sprites1[1].material = dark_m;
                sprites1[2].material = dark_m;
                sprites1[3].material = dark_m;
                if(angle < 45 && angle > 0){
                    sprites1[7].material = light_m;
                    if(saveDoor)
                        inDoor_SHANG = true;
                }else{
                    sprites1[7].material = dark_m;
                    inDoor_SHANG = false;
                }
                if(angle < 90 && angle > 45){
                    sprites1[6].material = light_m;
                    if(saveDoor)
                        inDoor_SHENG = true;
                }else{
                    sprites1[6].material = dark_m;
                    inDoor_SHENG = false;
                }
                if(angle < 135 && angle > 90){
                    sprites1[5].material = light_m;
                    if(saveDoor)
                        inDoor_XIU = true;
                }else{
                    sprites1[5].material = dark_m;
                    inDoor_XIU = false;
                }
                if(angle > 135){
                    sprites1[4].material = light_m;
                    if(saveDoor)
                        inDoor_KAI = true;
                } else{
                    sprites1[4].material = dark_m;
                    inDoor_KAI = false;
                }
            }
        }else{
            foreach(var sprite in sprites1){
                sprite.material = dark_m;
            }
            inDoor_DU = false;
            inDoor_JING = false;
            inDoor_SI = false;
            inDoor_JING_Bad = false;
            inDoor_KAI = false;
            inDoor_XIU = false;
            inDoor_SHENG = false;
            inDoor_SHANG = false;
        }
    }
    public DoorDataSO doorData;
    public GameObject doorItemUI;
    public GameObject doorItemPrefab;
    public void SetDoorUI(int id){
        var door = Instantiate(doorItemPrefab);
        door.transform.SetParent(doorItemUI.transform);
        door.transform.localScale = new Vector3(1,1,1);
        door.transform.localPosition = Vector3.zero;
        ItemIconDoor itemIconDoor = door.GetComponent<ItemIconDoor>();
        //设置奇门icon内容
        itemIconDoor.icon.text = doorData.GetDoorItem(id).name[0].ToString();
        itemIconDoor.itemName.text = doorData.GetDoorItem(id).name;
        itemIconDoor.desc.text = doorData.GetDoorItem(id).desc;
    }
    public void DeletDoorUI(){
        foreach (Transform item in doorItemUI.transform)
        {
            Destroy(item.gameObject);
        }
    }
    void CheckDoorId(){
        DeletDoorUI();
        int id = -1;
        if(inDoor_KAI){
            id = 0;
        }else if(inDoor_XIU){
            id = 1;
        }else if(inDoor_SHENG){
            id = 2;
        }else if(inDoor_JING){
            id = 3;
        }else if(inDoor_SHANG){
            id = 4;
        }else if(inDoor_DU){
            id = 5;
        }else if(inDoor_SI){
            id = 6;
        }else if(inDoor_JING_Bad){
            id = 7;
        }

        if(id!= -1){
            SetDoorUI(id);
        }
    }

    public bool inDoor_DU = false;
    public bool inDoor_JING = false;
    public bool inDoor_SI = false;
    public bool inDoor_JING_Bad = false;
    public bool inDoor_KAI = false;
    public bool inDoor_XIU = false;
    public bool inDoor_SHENG = false;
    public bool inDoor_SHANG = false;

    public float turnTime = 5;
    public float sleepTime = 10;
    float countTime = 0;
    bool isTurn = true;
    public GameObject[] circles;
    bool saveDoor = false;

    private void Update() {
        if(checkingDoor){
            GetPartByPlayer();
        }
        if(isTurn && Global.GameBegain){
            DeletDoorUI();
            saveDoor = false;
            countTime += Time.deltaTime;
            if(countTime >= turnTime){
                saveDoor = true;
                GetPartByPlayer();
                CheckDoorId();
                isTurn = false;
                countTime = 0;
                checkingDoor = false;
                foreach (var item in circles)
                {
                    SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
                    sprite.material.DOFade(0,0.3f).OnComplete(()=>{
                        item.SetActive(false);
                    });
                }
            }
        }else if(Global.GameBegain){
            saveDoor = false;
            countTime += Time.deltaTime;
            if(countTime >= sleepTime){
                isTurn = true;
                countTime = 0;
                checkingDoor = true;
                foreach (var item in circles)
                {
                    SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
                    sprite.material.color = new Color(sprite.material.color.r,sprite.material.color.g,sprite.material.color.b,0);
                    item.SetActive(true);
                    sprite.material.DOFade(1,0.3f);
                }
            }
        }
        
    }
}
