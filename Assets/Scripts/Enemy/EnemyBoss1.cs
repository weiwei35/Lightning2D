using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyBoss1 : EnemyController
{
    [Header("冲刺频率")]
    public float rushTime = 5f;
    public GameObject white;
    public GameObject black;
    public Animator circleAnim;
    public GameObject bigCollider;
    public GameObject bigBoom;
    int ballCount = 0;
    float rushTimeCount = 0;
    Vector3 rushDis;
    bool isRush = false;
    bool isBig = false;
    bool isAttacking = false;
    Quaternion whiteRotate;
    Quaternion blackRotate;
    Tweener tweenWhite;
    Tweener tweenBlack;
    EnemyPoolController enemyPool;
    bool secondSection = true;

    CameraWave screenWave;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        transform.position = center.transform.position;
        whiteRotate = white.transform.rotation;
        blackRotate = black.transform.rotation;
        enemyPool = transform.parent.GetComponent<EnemyPoolController>();
        screenWave = GameObject.FindWithTag("ScreenWave").GetComponent<CameraWave>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(!isInBlackHall && !isBoom && !isBig)
        {
            FollowMove ();
        }
        if(!Global.isSlowDown && !isFreeze){
            rushTimeCount += Time.deltaTime;
            bulletTimeCount += Time.deltaTime;
        }
        if(Global.isSlowDown){
            circleAnim.speed = 0.5f;
            if(tweenWhite != null)
                tweenWhite.timeScale = 0.1f;
            if(tweenBlack != null)
                tweenBlack.timeScale = 0.1f;
        }else{
            circleAnim.speed = 1f;
            if(tweenWhite != null)
                tweenWhite.timeScale = 1;
            if(tweenBlack != null)
                tweenBlack.timeScale = 1;
        }
        if(rushTimeCount >= rushTime && !isRush && !isBig && !isFreeze && !Global.isSlowDown){
            isRush = true;
            RushToPlayer();
            
            screenWave.gameObject.SetActive(true);
            screenWave.CallShockWave(transform.position);
        }
        if(isBig){
            if(Vector3.Distance(transform.position,target.transform.position)<=12){
                Rigidbody rb = target.GetComponent<Rigidbody>();
                Vector3 distance = center.transform.position - target.transform.position;
                distance.Normalize();
                if(!Global.isSlowDown)
                    rb.AddForce(distance * 10);
            }
        }
        if(isAttacking){
            CheckBall();
            if(ballCount == 0){
                isAttacking = false;
                HP -= maxHP*0.2f;
                Duzzy(4);
                isBig = false;
                circleAnim.enabled = true;
                bigCollider.SetActive(false);
            }
        }
        // if(enemyPool.GetAllEnemyCount()==1){
        //     secondSection = true;
        // }

        if(bulletTimeCount >= bulletTime && !isShoot && !isRush && !Global.isSlowDown){
            isShoot = true;
            if(fireSwitch){
                StartFire();
                fireSwitch = false;
            }else{
                StartFire2();
                fireSwitch = true;
            }
            
            Invoke("ResetFire",2f);
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
    //面朝角色扇形攻击
    [Header("武器伤害")]
    public float bulletHurt = 5f;
    [Header("武器速度")]
    public float bulletSpeed = 2f;
    [Header("武器频率")]
    public float bulletTime = 5f;
    float bulletTimeCount = 0;
    bool isShoot = false;
    bool fireSwitch = false;
    void StartFire(){
        float angleInterval = 100 / 10; // 计算角度间隔
        float startAngle = (Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * (180 / Mathf.PI)) - 50;
        for (int i = 0; i < 10; i++)
        {
            float angle = startAngle + i * angleInterval; // 计算当前物体的角度
            float x = 3 * Mathf.Cos(angle * Mathf.Deg2Rad); // 计算 x 坐标
            float y = 3 * Mathf.Sin(angle * Mathf.Deg2Rad); // 计算 y 坐标

            Vector3 position = new Vector3(x, y, 0); // 转换为笛卡尔坐标系

            // 在计算出的位置生成物体
            Fire(position);
        }
    }
    void StartFire2(){
        float angleInterval = 360 / 30; // 计算角度间隔
        float startAngle = (Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * (180 / Mathf.PI)) - 50;
        for (int i = 0; i < 30; i++)
        {
            float angle = startAngle + i * angleInterval; // 计算当前物体的角度
            float x = 3 * Mathf.Cos(angle * Mathf.Deg2Rad); // 计算 x 坐标
            float y = 3 * Mathf.Sin(angle * Mathf.Deg2Rad); // 计算 y 坐标

            Vector3 position = new Vector3(x, y, 0); // 转换为笛卡尔坐标系

            // 在计算出的位置生成物体
            Fire(position);
        }
    }
    void Fire(Vector3 direction){
        var curBullet = GameObjectPoolTool.GetFromPoolForce(true,"Assets/Resources/Bullet.prefab");
        // var curBullet = Instantiate(bullet);
        curBullet.transform.position = transform.position;
        Rigidbody bulletRb = curBullet.GetComponent<Rigidbody>();
        BulletController bulletController = curBullet.GetComponent<BulletController>();
        bulletController.hurt = bulletHurt;
        // Vector3 direction = target.position - transform.position;
        direction.Normalize();
        bulletRb.velocity = direction * bulletSpeed;
        bulletController.bulletSpeed = bulletSpeed;
        bulletController.direction = direction;
        bulletController.center = center.transform.position;
        bulletController.length = 15;
    }
    void ResetFire(){
        bulletTimeCount = 0;
        isShoot = false;
    }

    void RushToPlayer(){
        rushDis = target.position - transform.position;
        rushDis.Normalize();
        transform.DOMove(transform.position,1).OnComplete( ()=>
            {
                transform.DOMove(transform.position+rushDis*10,0.5f).OnComplete( ()=>
                    {
                        rushDis = target.position - transform.position;
                        rushDis.Normalize();
                        transform.DOMove(transform.position,1).OnComplete( ()=>
                            {
                                transform.DOMove(transform.position+rushDis*10,0.5f).OnComplete( ()=>
                                    {
                                        isRush = false;
                                        rushTimeCount = 0;
                                        if(secondSection){
                                            CheckBall();

                                            if(ballCount == 0 || ballCount == 2){
                                                white.SetActive(true);
                                                white.transform.localPosition = Vector3.zero;
                                                CheckBall();
                                                if(ballCount == 3){
                                                    //大招
                                                    isBig = true;
                                                    BigFire();
                                                }
                                            }
                                            else if(ballCount == 1){
                                                black.SetActive(true);
                                                black.transform.localPosition = Vector3.zero;
                                                CheckBall();
                                                if(ballCount == 3){
                                                    //大招
                                                    isBig = true;
                                                    BigFire();
                                                }
                                            }
                                        }
                                    }
                                );
                            }
                        );
                    }
                );
            }
        );
        
    }
    void CheckBall(){
        if(!white.activeInHierarchy && !black.activeInHierarchy){
            ballCount = 0;
        }
        if(white.activeInHierarchy && !black.activeInHierarchy){
            ballCount = 1;
        }
        if(!white.activeInHierarchy && black.activeInHierarchy){
            ballCount = 2;
        }
        if(white.activeInHierarchy && black.activeInHierarchy){
            ballCount = 3;
        }
    }
    void BigFire(){
        transform.position = center.transform.position;
        circleAnim.enabled = false;
        white.transform.rotation = whiteRotate;
        white.transform.DOMove(transform.position + new Vector3(-6,-6,0),0.5f).OnComplete( ()=>
            {
                tweenWhite = white.transform.DOMove(center.transform.position + new Vector3(3,0,0),10f);
            });
        black.transform.rotation = blackRotate;
        black.transform.DOMove(transform.position + new Vector3(6,6,0),0.5f).OnComplete( ()=>
            {
                tweenBlack = black.transform.DOMove(center.transform.position + new Vector3(-3,0,0),10f);
            });
        bigCollider.SetActive(true);
        isAttacking = true;
        Invoke("Attrack",10f);
    }
    void Attrack(){
        rushTimeCount = 0;
        isBig = false;
        circleAnim.enabled = true;
        CheckBall();

        bigCollider.SetActive(false);
        if(ballCount == 3){
            bigBoom.SetActive(true);
            screenWave.gameObject.SetActive(true);
            screenWave.CallShockWave(transform.position);
            bigBoom.transform.DOScale(24,0.5f).OnComplete( ()=>
            {
                bigBoom.SetActive(false);
                PlayerController player = target.GetComponent<PlayerController>();
                player.Hurt(50);
            });
        }
        if(ballCount == 1 || ballCount == 2){
            HP -= maxHP*0.2f;
        }
        white.SetActive(false);
        black.SetActive(false);
        isAttacking = false;
    }
}
