using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnceController : MonoBehaviour
{
    public float lightningCount = 0;
    public float timeCount = 0;
    PlayerController player;
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update() {
        if(!Global.isSlowDown)
            timeCount += Time.deltaTime;
        if(lightningCount >= 1){
            Destroy(gameObject);
            player.isLightCopied = false;
            player.isTimeCopied = false;
        }
        if(timeCount >= player.copyTimeNum-0.1f){
            player.isTimeCopied = false;
        }
        if(!player.isOnceLightningCopy && !player.isOnceTimeCopy){
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        Global.playerCopyList.Remove(gameObject);
    }
}
