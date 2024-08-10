using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BugAttackController : BugController
{
    EnemyController enemy;
    [Header("攻击伤害")]
    public float attack;
    public MeshRenderer mesh;
    public BugAttackCopy bugAttackCopyPre;
    public override void Update()
    {
        base.Update();
        if(energyCurrent >= energy){
            canRecoverEnergy = false;
            energyCurrent = 0;
            AttackNear();
        }
    }
    void AttackNear(){
        isTriggered = true;
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        float minDistance = 100;
        foreach (var item in enemys)
        {
            float distance = Vector3.Distance(item.transform.position,transform.position);
            if(minDistance > distance){
                enemy = item;
                minDistance = distance;
            }
        }
        
        if(enemy != null){
            mesh.enabled = false;
            var attackCopy = Instantiate(bugAttackCopyPre);
            attackCopy.transform.position = transform.position;
            attackCopy.enemy = enemy;
            attackCopy.bugAttack = GetComponent<BugAttackController>();
            attackCopy.attack = attack;
        }else{
        isTriggered = false;
            canRecoverEnergy = true;
        }
    }
}
