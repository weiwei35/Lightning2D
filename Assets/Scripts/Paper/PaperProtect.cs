using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperProtect : PaperModel
{
    public float overTime;
    public float countTime = 0;
    Animator anim;
    public GameObject protectEffect;
    public GameObject ProtectBall;
    private void Start() {
        anim = GetComponent<Animator>();
        InvokeRepeating("ProtectRecover",5,5);
    }
    private void Update() {
        if(isOverLoad && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime>=overTime && isOverLoad){
            countTime = 0;
            isOverLoad = false;
            anim.speed = 1;

            CancelInvoke("ProtectRecover");
        
            InvokeRepeating("ProtectRecover",0,5);
        }
    }
    void ProtectRecover(){
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // player.OutsideRecoveryProtect(Mathf.Max(Mathf.Ceil(player.GetHurtProtectCount() * 0.1f), 1));

        var protectBall = Instantiate(ProtectBall);
        protectBall.transform.parent = transform;
        protectBall.transform.position = RandomMoveObject();
        protectBall.GetComponent<ProtectBall>().protect = Mathf.Max(Mathf.Ceil(player.GetHurtProtectCount() * 0.1f), 1);
        protectEffect.SetActive(true);
        Invoke("SetTreeEffect",2);
    }
    void SetTreeEffect(){
        protectEffect.SetActive(false);
    }
    public override void OverLoadFun(){
        anim.speed = 2;
        CancelInvoke("ProtectRecover");
        
        InvokeRepeating("ProtectRecover",0,2);
    }

    Vector3 RandomMoveObject()
    {
        // 生成均匀分布的点
        float randomAngle = Random.Range(0f, 360f); // 随机角度
        float randomRadius = Random.Range(1f, 3); // 随机半径
 
        // 计算坐标
        float randomX= transform.position.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius;
        float randomY = transform.position.y + Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius;

        return new Vector3(randomX,randomY,-5);
    }
}
