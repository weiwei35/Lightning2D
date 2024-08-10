using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProtectBall : MonoBehaviour
{
    public float protect;
    PlayerController player;
    Tweener tweener;
    bool isMoving = false;
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update() {
        if(player.protectRecovery){
            if(Vector3.Distance(transform.position,player.transform.position) <= 2 && !isMoving){
                isMoving = true;
                RecoveryProtect();
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            PlayerController playerCollider = other.GetComponent<PlayerController>();
            playerCollider.OutsideRecoveryProtect(protect);
            Destroy(gameObject);
        }
    }
    private void OnDestroy() {
        if(tweener != null){
            tweener.Kill();
        }
    }
    void RecoveryProtect(){
        if(transform != null)
            tweener = transform.DOMove(player.transform.position,0.2f);
    }
}
