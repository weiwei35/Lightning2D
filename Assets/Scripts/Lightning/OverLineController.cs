using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OverLineController : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public GameObject pos;
    public LayerMask layer;
    public float showTime = 0;
    public float startTime = 0.1f;
    public float keepTime = 0.5f;
    LineRenderer line;
    bool isChecked = false;
    EnemyController enemy;
    LightningController lightning;
    PlayerController player;
    public LightningEffect lightningAsset;
    LightningEffect lightningEffect;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(!isChecked){
            isChecked = true;
            // CheckCopy();
        }
        DrawLinePoints();
    }
    private void Update() {
            
    }
    private void FixedUpdate()
    {
        if(pos.transform.position != end.transform.position)
            pos.transform.position = end.transform.position;
        line.SetPosition(0,start.transform.position);
        line.SetPosition(1,pos.transform.position);
    }

    public void CheckCopy() {
        if(player.GetComponent<PlayerController>().isOnceLightningCopy || player.GetComponent<PlayerController>().isOnceTimeCopy){
            GameObject[] playerOnceCopy = GameObject.FindGameObjectsWithTag("PlayerOnceCopy");
            if(playerOnceCopy.Length > 0){
                foreach (var item in playerOnceCopy)
                {
                    var lineCurCopy = Instantiate(gameObject);
                    lineCurCopy.transform.position = transform.position;
                    MirrorLineController lineControllerCopy = lineCurCopy.GetComponent<MirrorLineController>();
                    lineControllerCopy.start.transform.position = start.transform.position;
                    lineControllerCopy.end.transform.position = new Vector3(item.transform.position.x,item.transform.position.y,-5);
                    lineControllerCopy.startTime = startTime;
                    lineControllerCopy.keepTime = keepTime;
                    lineControllerCopy.timeCount = lightning.lightningPreTime+1;
                }
            }
        }
        if(player.GetComponent<PlayerController>().isCircleCopy){
                GameObject playerCopy = GameObject.FindGameObjectWithTag("PlayerCopy");
                var lineCurCopy = Instantiate(line.gameObject);
                lineCurCopy.transform.position = transform.position;
                MirrorLineController lineControllerCopy = lineCurCopy.GetComponent<MirrorLineController>();
                lineControllerCopy.start.transform.position = start.transform.position;
                lineControllerCopy.end.transform.position = new Vector3(playerCopy.transform.position.x,playerCopy.transform.position.y,-5);
                lineControllerCopy.startTime = startTime;
                lineControllerCopy.keepTime = keepTime;
                lineControllerCopy.timeCount = lightning.lightningPreTime+1;
        }
    }

    public void DrawLinePoints() {
        lightning.PlayAudio(0);
        lightningEffect = Instantiate(lightningAsset);
        lightningEffect.transform.parent = transform;
        lightningEffect.pos1.transform.position = start.transform.position;
        lightningEffect.pos2.transform.position = start.transform.position;
        lightningEffect.pos1.transform.DOMove(end.transform.position,startTime);
        lightningEffect.pos2.transform.DOMove(end.transform.position,startTime);
        lightningEffect.pos3.transform.position = start.transform.position;
        lightningEffect.pos4.transform.position = start.transform.position;
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;
        pos.transform.position = start.transform.position;
        pos.transform.DOMove(end.transform.position,startTime).OnComplete(()=>
        {
            Invoke("EndLine", keepTime);
        });
    }

    public void EndLine () {
        lightningEffect.pos3.transform.DOMove(end.transform.position,startTime);
        lightningEffect.pos4.transform.DOMove(end.transform.position,startTime);
        start.transform.DOMove(end.transform.position,startTime).OnComplete(()=>
        {
            // Global.isSlowDown = false;
            var enemys = Transform.FindObjectsOfType<EnemyController>();
            foreach (var item in enemys)
            {
                item.isHitting = false;
            }
            Destroy(gameObject);
        });
    }
}
