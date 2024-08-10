using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BugAttackCopy : MonoBehaviour
{
    public float attack;
    public EnemyController enemy;
    public BugAttackController bugAttack;
    private void Start() {
        if(enemy != null  && enemy.canHurt){
            Vector3 targetPos = new Vector3(enemy.transform.position.x,enemy.transform.position.y,-10);
            transform.DOMove(targetPos,0.2f).OnComplete(()=>
                {
                    transform.DOMove(bugAttack.transform.position,0.2f).OnComplete(()=>
                        {
        bugAttack.isTriggered = false;
                            bugAttack.canRecoverEnergy = true;
                            bugAttack.mesh.enabled = true;
                            Destroy(gameObject);
                        });
                });
            enemy.HurtByBugAttack(attack,HurtType.BugAttack);
        }else{
        bugAttack.isTriggered = false;
            bugAttack.canRecoverEnergy = true;
            bugAttack.mesh.enabled = true;
            Destroy(gameObject);
        }
        
    }
}
