using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomRecover : EnemyController
{
    [Header("移动半径")]
    public float range = 10.0f; // 移动范围
    [Header("角色保护范围")]
    public float offset = 2.0f;
    [Header("生效范围")]
    public float recoverRadius = 3;
    [Header("恢复生命")]
    public float recoverHP = 1f;
    [Header("恢复频率")]
    public float recoverTime = 5f;
    float recoverTimeCount = 0;
    bool isMoving = false;
    float randomX;
    float randomY;
    List<EnemyController> crazyList = new List<EnemyController>();


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        RandomStartObject();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(!isInBlackHall && !isBoom)
        {
            if (Vector3.Distance(transform.position, new Vector3(randomX, randomY, target.position.z)) < 0.05f)
            {
                isMoving = false;
            }
            else
            {
                isMoving = true;
            }
            if(!isMoving)
                RandomMoveObject();
            // 平滑移动物体
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(randomX, randomY, target.position.z), speed * Time.deltaTime);
        }
        if(!Global.isSlowDown)
        {
            recoverTimeCount += Time.deltaTime;
        }
        if(recoverTimeCount >= recoverTime){
            SetAllRecover();
            recoverTimeCount = 0;
        }
    }
    public override void Hurt(float hurt,HurtType type)
    {
        base.Hurt(hurt,type);
        // anim.Play("hurt1");
    }
    public override void HurtByCircle(float hurt,HurtType type)
    {
        base.HurtByCircle(hurt,type);
        // anim.Play("hurt1");
    }
    //在范围内&距离角色一定距离随机出生点
    void RandomStartObject()
    {
        // 生成均匀分布的点
        float randomAngle = Random.Range(0f, 360f); // 随机角度
        float randomRadius = Random.Range(0f, range); // 随机半径
 
        // 计算坐标
        var x= center.position.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius;
        var y = center.position.y + Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius;
        Vector3 start = new Vector3(x,y,-5);
        var distance =  Mathf.Abs(Vector3.Distance(start,target.position));
        if(distance < offset)
            RandomStartObject();
        else
            transform.position = start;
    }
    //在范围内随机行走的点
    void RandomMoveObject()
    {
        // 生成均匀分布的点
        float randomAngle = Random.Range(0f, 360f); // 随机角度
        float randomRadius = Random.Range(0f, range); // 随机半径
 
        // 计算坐标
        randomX= center.position.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius;
        randomY = center.position.y + Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius;
    }
    void SetAllRecover(){
        crazyList.Clear();
        anim.SetTrigger("recover");
        // 找到所有在半径为3的范围内的碰撞器
        Collider[] colliders = Physics.OverlapSphere(transform.position, recoverRadius);
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
                enemy.Recover(recoverHP);
            }
        }
    }
}
