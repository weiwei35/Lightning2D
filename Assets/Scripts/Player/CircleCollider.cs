using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollider : MonoBehaviour
{
    public float hurtCount;
    public float minDistance;
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == 6){
            float distance = Vector3.Distance(transform.position,other.transform.position);
            if(distance>=minDistance){
                EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
                if(enemy != null)
                {
                    enemy.HurtByCircle(hurtCount,HurtType.BugCircle);
                }
            }
        }
    }
}
