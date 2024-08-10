using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColliderMirror : MonoBehaviour
{
    LightningController lightning;
    public Vector3 start;
    public Vector3 end;
    PlayerController player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
    }
    private void OnTriggerEnter(Collider other) {
        lightning = FindObjectOfType<LightningController>();
        if(other.gameObject.layer == 6){
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            if(enemy != null && !enemy.isHitting)
            {
                lightning.HurtEnemy(enemy,HurtType.MirrorLine);
            }
        }
        if(other.gameObject.layer == 17){
            other.gameObject.SetActive(false);
        }
    }
}
