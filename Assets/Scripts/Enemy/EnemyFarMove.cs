using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarMove : EnemyController
{
    [Header("逃跑距离")]
    public float backDistance = 3f;
    [Header("攻击距离")]
    public float attackDistance = 8f;
    [Header("追击距离")]
    public float followDistance = 15f;
    [Header("武器预制体")]
    public GameObject bullet;
    [Header("武器伤害")]
    public float bulletHurt = 5f;
    [Header("武器速度")]
    public float bulletSpeed = 2f;
    [Header("武器频率")]
    public float bulletTime = 5f;
    bool isFollow = true;
    bool isAttack = false;
    bool isInColdTime = false;
    Vector3 direction = new Vector3();
    bool isBack = false;
    public SpriteRenderer sprite;
    public Transform mainEnemy;
    public Animator animator;
    public GameObject bulletPos1;
    public GameObject bulletPos2;
    public Vector3 bulletPos;
    public List<GameObject> bulletList = new List<GameObject>();

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        // SpawnAtRandomEdge();
        // SpawnAtRandomCircle();
        // animator = GetComponent<Animator>();
        // sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(!isInBlackHall && !isBoom)
        {
            FollowMove ();
            if(!CheckInACircle()){
                isFollow = true;
                isBack = false;
            }
        }
        bulletPos = bulletPos1.transform.position;
        Vector2 v = target.transform.position - transform.position;
            var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            var trailRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            mainEnemy.rotation = trailRotation;
            if(transform.position.x > target.position.x){
                bulletPos = bulletPos1.transform.position;
                // sprite.flipX = true;
                // sprite.flipY = true;
                sprite.transform.localScale = new Vector3(-1,-1,1);
            }else if(transform.position.x < target.position.x){
                bulletPos = bulletPos2.transform.position;
                // sprite.flipY = false;
                // sprite.flipX = true;
                sprite.transform.localScale = new Vector3(-1,1,1);
            }

        // 获取当前物体的欧拉角
        Vector3 currentRotation = transform.rotation.eulerAngles;
        if(transform.position.x > target.position.x){
                currentRotation.z = Mathf.Clamp(currentRotation.z, 150, 210);
            }else if(transform.position.x < target.position.x){
                if(currentRotation.z < 180)
                    currentRotation.z = Mathf.Clamp(currentRotation.z, 0, 30);
                if(currentRotation.z > 180)
                    currentRotation.z = Mathf.Clamp(currentRotation.z, 330, 360);
            }

        // 应用限制后的欧拉角
        mainEnemy.rotation = Quaternion.Euler(currentRotation);

        //控制子弹数量
        if(bulletList.Count >2){
            Destroy(bulletList[0].gameObject);
            bulletList.RemoveAt(0);
        }
    }
    public override void Hurt(float hurt,HurtType type)
    {
        base.Hurt(hurt,type);
    }
    public override void HurtByCircle(float hurt,HurtType type)
    {
        base.HurtByCircle(hurt,type);
        // anim.Play("hurt3");
    }

    private void FollowMove () {
        float distance = Mathf.Abs(Vector3.Distance(transform.position,target.position));
        //逃跑路线：逃跑--backDistance--逃跑&攻击--attackDistance--逃跑--followDistance--追击
        //追击路线：逃跑--backDistance--追击&攻击--attackDistance--追击--followDistance--追击
        if(isFollow){
            if(distance>attackDistance){
                transform.position = Vector3.MoveTowards(transform.position,target.position,Time.deltaTime * speed);
            }else if(distance<attackDistance && distance>backDistance){
                transform.position = Vector3.MoveTowards(transform.position,target.position,Time.deltaTime * speed);
            }else if(distance<=backDistance){
                isFollow = false;
            }
        }else{
            if(distance<=attackDistance){
                isFollow = false;
                if(!isBack)
                    RandomDir();
                if(direction != null)
                    MoveAway();
            }else if(distance>attackDistance && distance<followDistance){
                if(!isBack)
                    RandomDir();
                if(direction != null)
                    MoveAway();
            }else if(distance>followDistance){
                isFollow = true;
                isBack = false;
            }
        }
        if(distance<attackDistance && distance>backDistance && !isAttack){
            isAttack = true;
            // InvokeRepeating("Attack",0,bulletTime);
            if(!isInColdTime)
                StartCoroutine(AttackTimer());
        }else if(distance>=attackDistance){
            // CancelInvoke("Attack");
            StopCoroutine(AttackTimer());
            isAttack = false;
        }
    }
    //远离主角的方向，随机一个角度运动
    void MoveAway(){
        // 按方向移动游戏对象
        transform.position += direction * speed * Time.deltaTime;
    }
    void RandomDir(){
        isBack = true;
        // 计算线段的方向向量
        direction = transform.position - target.position;
        // 创建一个旋转量Quaternion，表示旋转angle度
        var angle = Random.Range(-90,90);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        // 使用rotation * Vector3.forward得到新的方向向量
        direction = rotation * direction;
        // 标准化方向向量以确保速度一致
        direction.Normalize();
    }
    //矩形区域边缘随机取一点
    void SpawnAtRandomEdge()
    {
        // 获取屏幕中心点
        Camera mainCamera = Camera.main;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 screenCenter = new Vector3(screenWidth * 0.5f, screenHeight * 0.5f,0);
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);

        //获取屏幕 4个顶点
        float cameraHeight = mainCamera.orthographicSize; // 如果是正交摄像机，获取宽度
        float cameraWidth = cameraHeight * mainCamera.aspect; // 获取高度，考虑屏幕比率
        Vector3 topLeft = worldCenter + new Vector3(-cameraWidth, cameraHeight);
        Vector3 topRight = worldCenter + new Vector3(cameraWidth, cameraHeight);
        Vector3 bottomLeft = worldCenter + new Vector3(-cameraWidth, -cameraHeight);
        Vector3 bottomRight = worldCenter + new Vector3(cameraWidth, -cameraHeight);

        //取随机一边，取边上的一点
        int line = Random.Range(0,3);
        Vector3 point = new Vector3();
        switch(line){
            case 0:
            point = GetRandomPointOnLine(topLeft,topRight);
            break;
            case 1:
            point = GetRandomPointOnLine(topRight,bottomRight);
            break;
            case 2:
            point = GetRandomPointOnLine(bottomRight,bottomLeft);
            break;
            case 3:
            point = GetRandomPointOnLine(bottomLeft,topLeft);
            break;
        }
        transform.position = new Vector3(point.x,point.y,-5);
    }

    //取线段上的随机一点
    public static Vector3 GetRandomPointOnLine(Vector3 start, Vector3 end)
    {
        // 计算线段的总长度
        float lineLength = Vector3.Distance(start, end);
        
        // 生成一个0到1之间的随机数
        float randomPoint = Random.Range(0f, 1f);
        
        // 计算随机点在线段上的位置
        Vector3 randomPointOnLine = start + (end - start).normalized * (randomPoint * lineLength);
        return randomPointOnLine;
    }

    //攻击逻辑：按频率朝主角方向发射子弹
    bool CheckInACircle(){
        var distance = Vector2.Distance(center.position,transform.position);
        if(distance > radius){
            return false;
        }else
            return true;
    }

    IEnumerator AttackTimer(){
        animator.SetTrigger("attack");
        isAttack = true;
        isInColdTime = true;
        StartCoroutine(SetColdTime());
        
        yield return new WaitForSeconds(bulletTime);
        StartCoroutine(AttackTimer());
    }
    IEnumerator SetColdTime(){
        yield return new WaitForSeconds(bulletTime);
        isInColdTime = false;
    }

    public void Fire() {
        var curBullet = GameObjectPoolTool.GetFromPoolForce(true,"Assets/Resources/Bullet.prefab");
        // var curBullet = Instantiate(bullet);
        curBullet.transform.position = bulletPos;
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
