using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugHPController : BugController
{
    [Header("每次恢复时长")]
    public float recoverTime = 2;
    float countTime = 0;
    bool startRecovery = false;
    Animation anim;
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(energyCurrent >= energy){
            canRecoverEnergy = false;
            energyCurrent = 0;
            RecoveryPlayerHP();
        }
        if(startRecovery && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime >= recoverTime){
            countTime = 0;
        isTriggered = false;
            startRecovery = false;
            canRecoverEnergy = true;
            // Debug.Log("开始充能");
            CancelInvoke("SetPlayerHP");
        }
    }
    void RecoveryPlayerHP(){
        isTriggered = true;
        startRecovery = true;
        InvokeRepeating("SetPlayerHP",0,1);
    }
    void SetPlayerHP(){
        anim = GetComponent<Animation>();
        anim.Play();
        // Debug.Log("开始回血："+Mathf.Max(Mathf.Ceil(player.GetHurtCount() * 0.1f), 1)+"点");
        player.OutsideRecoveryHP(Mathf.Max(Mathf.Ceil(player.GetHurtCount() * 0.1f), 1));
    }
}
