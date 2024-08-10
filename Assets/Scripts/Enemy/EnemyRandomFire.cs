using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomFire : EnemyController
{
    [Header("移动半径")]
    public float range = 10.0f; // 移动范围
    [Header("角色保护范围")]
    public float offset = 2.0f;
    [Header("武器预制体")]
    public GameObject bullet;
    [Header("武器伤害")]
    public float bulletHurt = 5f;
    [Header("武器速度")]
    public float bulletSpeed = 2f;
    [Header("武器频率")]
    public float bulletTime = 5f;
    float bulletTimeCount = 0;
    bool isMoving = false;
    float randomX;
    float randomY;
    public GameObject bulletPos1;
    public GameObject bulletPos2;
    public Vector3 bulletPos;
    public SpriteRenderer sprite;
    public List<GameObject> bulletList = new List<GameObject>();

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
            bulletTimeCount += Time.deltaTime;
        }
        if(bulletTimeCount >= bulletTime && Vector3.Distance(transform.position, target.transform.position) <= 8f){
            // Fire();
            anim.SetTrigger("attack");
            bulletTimeCount = 0;
        }

        if(transform.position.x > target.position.x){
            // sprite.flipX = false;
            // sprite.flipY = true;
            bulletPos = bulletPos2.transform.position;
            sprite.transform.localScale = new Vector3(-1,1,1);
        }else if(transform.position.x < target.position.x){
            // sprite.flipY = false;
            // sprite.flipX = false;
            bulletPos = bulletPos1.transform.position;
            sprite.transform.localScale = new Vector3(1,1,1);
        }

        //控制子弹数量
        if(bulletList.Count >1){
            Destroy(bulletList[0].gameObject);
            bulletList.RemoveAt(0);
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

    public void Fire() {
        var curBullet = GameObjectPoolTool.GetFromPoolForce(true,"Assets/Resources/Bullet.prefab");
        // var curBullet = Instantiate(bullet);
        curBullet.transform.position = transform.position;
        Rigidbody bulletRb = curBullet.GetComponent<Rigidbody>();
        BulletController bulletController = curBullet.GetComponent<BulletController>();
        bulletController.hurt = bulletHurt;
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        bulletRb.velocity = direction * bulletSpeed;
        bulletController.bulletSpeed = bulletSpeed;
        bulletController.direction = direction;
        bulletController.center = center.transform.position;
        bulletController.length = 15;
        bulletList.Add(curBullet);
    }
}
