using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PaperFire : PaperModel
{
    public float overTime;
    public float countTime = 0;
    public GameObject fireBall;
    SpriteRenderer sprite;
    Animator anim;
    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        InvokeRepeating("FireBall",1,1);
    }
    private void Update() {
        if(isOverLoad && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime>=overTime && isOverLoad){
            countTime = 0;
            isOverLoad = false;
            anim.speed = 1;
            sprite.color = Color.white;

            CancelInvoke("FireBall");
        
            InvokeRepeating("FireBall",0,1);
        }
    }

    void FireBall(){
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        List<EnemyController> enemyInArea = new List<EnemyController>();
        foreach (var item in enemys)
        {
            float distance = Vector3.Distance(item.transform.position,transform.position);
            if(distance <= 5){
                enemyInArea.Add(item);
            }
        }
        if(enemyInArea.Count > 0){
            int id = Random.Range(0,enemyInArea.Count);
            Debug.Log("火球攻击"+enemyInArea[id].name);
            if(enemyInArea[id] != null){
                var ball = Instantiate(fireBall);
                ball.transform.position = transform.position;
                ball.transform.DOMove(enemyInArea[id].transform.position,0.2f).OnComplete(()=>{
                    enemyInArea[id].HurtByBugAttack(0.2f,HurtType.PaperFireBall);
                    Destroy(ball.gameObject);
                });
            }
            
        }
    }
    public override void OverLoadFun(){
        sprite.color = Color.red;
        anim.speed = 2;
        CancelInvoke("FireBall");
        
        InvokeRepeating("FireBall",0,0.5f);
    }
}
