using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyBoss2_Little : EnemyController
{
    public float direction;
    public float distance;
    public float startTime = 0;
    public EnemyBoss2 bossBat;
    bool canFollow = false;
    Tweener tweener;
    public override void Start()
    {
        base.Start();
        Invoke("Rush",1 + startTime);
        // Invoke("RushCircle",0);

    }
    public override void Update()
    {
        base.Update();
        if(!isInBlackHall && !isBoom && canFollow)
        {
            FollowMove ();
        }
        if(Global.isSlowDown){
            if(tweener != null)
                tweener.timeScale = 0.1f;
        }else{
            if(tweener != null)
                tweener.timeScale = 1f;
        }
    }
    void RushCircle(){
        canHurt = true;
        float angleRadians = direction * Mathf.PI / 180;
        float x = transform.position.x + 10 * Mathf.Cos(angleRadians);
        float y = transform.position.y + 10 * Mathf.Sin(angleRadians);
        Vector3 end = new Vector3(x,y,-5);
        tweener = transform.DOMove(end,1f).OnComplete(()=>{
            canFollow = true;
        });
    }

    void Rush(){
        canHurt = true;
        // 计算终点坐标
        tweener = transform.DOMove(target.position,1f).OnComplete(()=>{
            canFollow = true;
        });
    }
    private void FollowMove () {
        if(target != null){
            transform.position = Vector3.MoveTowards(transform.position,target.position+new Vector3(0,1,0),Time.deltaTime * speed);
        }
    }

    public override void Hurt(float hurt,HurtType type)
    {
        if(canHurt)
            base.Hurt(hurt,type);
    }

    public override void Death(){
        base.Death();
        bossBat.batCount --;
    }
}
