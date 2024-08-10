using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugWallCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log("墙壁阻挡："+other.name);
        if(other.gameObject.layer == 12){
            Destroy(other.gameObject);
        }
        if(other.gameObject.layer == 6){
            // EnemyController enemy = other.GetComponent<EnemyController>();
            BoxCollider boxCollider = other.GetComponent<BoxCollider>();
            boxCollider.isTrigger = false;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == 6){
            // EnemyController enemy = other.GetComponent<EnemyController>();
            BoxCollider boxCollider = other.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }
    }
    float countTime = 0;
    public float wallTime = 5;
    private void Update() {
        if(!Global.isSlowDown)
            countTime += Time.deltaTime;
        if(countTime > wallTime)
            Destroy(gameObject);
    }
}
