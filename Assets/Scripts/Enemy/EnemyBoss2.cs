using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EnemyBoss2 : EnemyController
{
    [Header("冲刺频率")]
    public float rushTime = 5f;
    public GameObject littleBat;
    public GameObject MainBat;
    public int batCount;
    int maxBatCount = 10;
    BoxCollider boxCollider;
    float rushTimeCount = 0;
    bool isRush;
    EnemyPoolController enemyPool;
    // bool secondSection;
    List<GameObject> littles = new List<GameObject>();
    Vector3 endPos;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        maxBatCount = batCount;
        transform.position = center.transform.position;
        enemyPool = transform.parent.GetComponent<EnemyPoolController>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(!isInBlackHall && !isBoom)
        {
            // FollowMove ();
        }
        if(!Global.isSlowDown && !isFreeze){
            rushTimeCount += Time.deltaTime;
        }
        if(enemyPool.GetAllEnemyCount()==1){
            // secondSection = true;
        }
        if(rushTimeCount >= rushTime && !isRush && !isFreeze && !Global.isSlowDown){
            Rush();
            // RushCircle();
        }
    }
    public override void Hurt(float hurt,HurtType type)
    {
        base.Hurt(hurt,type);
        // anim.Play("hurt2");
    }
    public override void HurtByCircle(float hurt,HurtType type)
    {
        base.HurtByCircle(hurt,type);
        // anim.Play("hurt2");
    }
    public override void Death(){
        base.Death();
        Global.isEndBoss = true;
        // anim.Play("hurt2");
    }

    private void FollowMove () {
        if(target != null){
            transform.position = Vector3.MoveTowards(transform.position,target.position+new Vector3(0,1,0),Time.deltaTime * speed);
        }
    }
    void RushCircle(){
        isRush = true;
        boxCollider.enabled = false;
        MainBat.SetActive(false);
        text.gameObject.SetActive(false);

        littles.Clear();

        float angleInterval = 100 / batCount; // 计算角度间隔
        float startAngle = (Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * (180 / Mathf.PI)) - 50;

        for (int i = 0; i < batCount; i++)
        {
            float angle = startAngle + i * angleInterval; // 计算当前物体的角度
            float x = 3 * Mathf.Cos(angle * Mathf.Deg2Rad); // 计算 x 坐标
            float y = 3 * Mathf.Sin(angle * Mathf.Deg2Rad); // 计算 y 坐标

            Vector3 position = new Vector3(x, y, 0) + transform.position; // 转换为笛卡尔坐标系

            // 在计算出的位置生成物体
            var bat = Instantiate(littleBat, position, Quaternion.identity);
            bat.transform.parent = transform.parent;
            EnemyBoss2_Little little = bat.GetComponent<EnemyBoss2_Little>();
            little.bossBat = this;
            little.direction = startAngle + 50;

            littles.Add(bat);
        }
        float time = 10f;
        Invoke("ResetLittle",time);
    }

    void Rush(){
        isRush = true;
        boxCollider.enabled = false;
        MainBat.SetActive(false);
        text.gameObject.SetActive(false);

        littles.Clear();

        float angleIncrement = Mathf.PI * (3 - Mathf.Sqrt(5)); // 黄金角度
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2; // 黄金比例

        for (int i = 0; i < batCount; i++)
        {
            float theta = i * angleIncrement;
            float r = Mathf.Sqrt(i / (float)batCount) * 5 * transform.localScale.x;

            // 将极坐标转换为笛卡尔坐标
            float x = r * Mathf.Cos(theta);
            float z = r * Mathf.Sin(theta);
            Vector3 spawnPosition = new Vector3(x, z, 0) + transform.position;
            // 在计算出的位置生成物体
            var bat = Instantiate(littleBat, spawnPosition, Quaternion.identity);
            bat.transform.parent = transform.parent;
            EnemyBoss2_Little little = bat.GetComponent<EnemyBoss2_Little>();
            little.bossBat = this;
            // little.startTime = 0.03f*i;

            littles.Add(bat);
        }
        DistanceToPlayer();

        float time = 0.03f*batCount+1f+10f;

        Invoke("ResetLittle",time);
    }

    void DistanceToPlayer(){
        // 创建一个结构体或对象来保存物体和它们与玩家的距离
        var objectsWithDistances = littles.Select(obj => new
        {
            obj = obj,
            distanceToPlayer = Vector3.Distance(obj.transform.position, target.position)
        }).ToList();

        // 使用 LINQ 按照距离对列表进行排序
        objectsWithDistances = objectsWithDistances.OrderBy(item => item.distanceToPlayer).ToList();

        // 现在 objectsWithDistances 列表中的物体按照与玩家距离从近到远排序了
        int i = 0;
        foreach (var item in objectsWithDistances)
        {
            EnemyBoss2_Little little = item.obj.GetComponent<EnemyBoss2_Little>();
            little.startTime = 0.03f*i;
            i++;
        }
    }
    void ResetLittle(){
        if(batCount <= 0){
            Death();
        }
        foreach (var item in littles)
        {
            if(item != null){
                item.transform.DOMove(transform.position,0.2f).OnComplete(()=>{
                    Destroy(item);
                });
            }
        }
        // transform.position = new Vector3(endPos.x,endPos.y,-5);
        float scale = 0.5f + 0.5f*((float)batCount/maxBatCount);
        transform.localScale = new Vector3(scale,scale,scale);
        isRush = false;
        boxCollider.enabled = true;
        MainBat.SetActive(true);
        text.gameObject.SetActive(true);
        rushTimeCount = 0;
    }
}
