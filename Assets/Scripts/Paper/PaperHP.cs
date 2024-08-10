using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PaperHP : PaperModel
{
    public float overTime;
    public float countTime = 0;
    Animator anim;
    public GameObject treeEffect;
    public GameObject HPBall;
    private void Start() {
        anim = GetComponent<Animator>();
        InvokeRepeating("HPRecover",4,4);
    }
    private void Update() {
        if(isOverLoad && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime>=overTime && isOverLoad){
            countTime = 0;
            isOverLoad = false;
            anim.speed = 1;

            CancelInvoke("HPRecover");
        
            InvokeRepeating("HPRecover",0,4);
        }
    }
    void HPRecover(){
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // player.OutsideRecoveryHP(Mathf.Max(Mathf.Ceil(player.GetHurtCount() * 0.1f), 1));

        var hpBall = Instantiate(HPBall);
        hpBall.transform.parent = transform;
        hpBall.transform.position = RandomMoveObject();
        hpBall.GetComponent<HPBall>().hp = Mathf.Max(Mathf.Ceil(player.GetHurtCount() * 0.1f), 1);
        treeEffect.SetActive(true);
        Invoke("SetTreeEffect",1f);

    }
    void SetTreeEffect(){
        treeEffect.SetActive(false);
    }
    public override void OverLoadFun(){
        anim.speed = 2;
        CancelInvoke("HPRecover");
        
        InvokeRepeating("HPRecover",0,2);
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
