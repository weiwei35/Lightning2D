using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrazy : EnemyController
{
    [Header("生效频率")]
    public float crazyTime = 8;
    [Header("生效范围")]
    public float crazyRadius = 3;
    [Header("生效时间")]
    public float crazyEndTime = 5;
    float crazyTimeCount = 0;
    bool isInCrazy = false;
    List<EnemyController> crazyList = new List<EnemyController>();
    public override void Start()
    {
        base.Start();
        // SpawnAtRandomCircle();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(!isInBlackHall && !isBoom)
        {
            FollowMove ();
        }
        if(!Global.isSlowDown)
            crazyTimeCount += Time.deltaTime;
        if(crazyTimeCount > crazyTime && !isInCrazy){
            isInCrazy = true;
            Crazy();
        }
    }
    private void FollowMove () {
        if(target != null){
            transform.position = Vector3.MoveTowards(transform.position,target.position+new Vector3(0,1,0),Time.deltaTime * speed);
        }
    }
    void Crazy(){
        crazyList.Clear();
        
        anim.SetTrigger("crazy");
        Invoke("SetAllCrazy",2);
    }
    void SetAllCrazy(){
        // 找到所有在半径为3的范围内的碰撞器
        Collider[] colliders = Physics.OverlapSphere(transform.position, crazyRadius);
         // 过滤掉自身
        List<GameObject> nearbyObjects = new List<GameObject>();
        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject) // 排除自身
            {
                nearbyObjects.Add(collider.gameObject);
            }
        }

        foreach (GameObject collider in nearbyObjects)
        {
            if(collider.gameObject.layer == 6){
                EnemyController enemy = collider.GetComponent<EnemyController>();
                crazyList.Add(enemy);
                enemy.SetCrazy();
            }
        }
        Invoke("Reset",crazyEndTime);
    }

    void Reset(){
        crazyTimeCount = 0;
        isInCrazy = false;
    }
}
