using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BugFollowController : BugController
{
    [Header("追踪时长")]
    public float followTime = 8;
    float countTime = 0;
    [Header("追踪个数")]
    public int bugCount = 3;
    [Header("攻击伤害")]
    public float followHurt = 2;
    public MeshRenderer mesh;
    public BugFollowCopy bugFollowPre;
    List<BugFollowCopy> bugCopys = new List<BugFollowCopy>();
    bool startFollow = false;
    bool trueStart = false;
    public override void Update()
    {
        base.Update();
        if(energyCurrent >= energy){
            canRecoverEnergy = false;
            energyCurrent = 0;
            trueStart = true;
        }
        if(trueStart){
            SetBugsFollow();
        }
        if(startFollow && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime >= followTime){
            countTime = 0;
            startFollow = false;
            isTriggered = false;
            canRecoverEnergy = true;
            // Debug.Log("开始充能");
            mesh.enabled = true;
            foreach (var item in bugCopys)
            {
                item.isBack = true;
                // Destroy(item.gameObject);
                item.target = transform;
            }
            bugCopys.Clear();
            var enemys = Transform.FindObjectsOfType<EnemyController>();
            foreach (var item in enemys)
            {
                item.isFollowHitting = false;
            }
        }
    }

    void SetBugsFollow(){
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        if(enemys.Length > 0){
            trueStart = false;
            isTriggered = true;
            mesh.enabled = false;
            startFollow = true;
            var enemyListByHP = enemys.OrderByDescending(p => p.HP).Take(bugCount).ToList();
            foreach (var item in enemyListByHP)
            {
                Debug.Log(item.name +":"+item.HP);
                BugFollowCopy bugCopy = Instantiate(bugFollowPre);
                // bugCopy.transform.parent = transform.parent;
                bugCopy.transform.position = transform.position;
                // BugFollowController bugCopyFollow = bugCopy.GetComponent<BugFollowController>();
                bugCopy.target = item.transform;
                bugCopy.followHurt = followHurt;
                bugCopys.Add(bugCopy);
            }
        }
    }
}
