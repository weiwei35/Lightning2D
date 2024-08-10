using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugCircleController : BugController
{
    [Header("伤害区域时长")]
    public float circleTime = 10;
    float countTime = 0;
    bool startCircle = false;
    [Header("伤害区域范围半径")]
    public float circleRadiusMin = 2;
    public float circleRadiusMax = 5;
    [Header("伤害数值")]
    public float hurtCount = 2;
    Animation anim;
    public override void Update()
    {
        base.Update();
        if(energyCurrent >= energy){
            canRecoverEnergy = false;
            energyCurrent = 0;
            SetCircleHurt();
        }
        if(startCircle && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime >= circleTime){
            countTime = 0;
        isTriggered = false;
            startCircle = false;
            canRecoverEnergy = true;
            // Debug.Log("开始充能");
            player.bugCircleCollider.gameObject.SetActive(false);
        }
    }
    void SetCircleHurt(){
        anim = GetComponent<Animation>();
        anim.Play();
        isTriggered = true;
        startCircle = true;
        player.bugCircleCollider.gameObject.SetActive(true);
    }
}
