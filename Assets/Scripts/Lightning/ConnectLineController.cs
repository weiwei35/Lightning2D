using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ConnectLineController : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public GameObject pos;
    public Vector3 follow;
    public GameObject startPointEP;//特殊雷点
    public GameObject startPoint;
    public float startTime = 0.1f;
    public float keepTime = 0.5f;
    LineRenderer line;
    public GameObject copyLine;
    public bool isChecked = false;
    public float timeCount = 0;
    LightningController lightning;
    PlayerController player;
    public GameObject lineCollider;
    GameObject colliderCur;
    CapsuleCollider capsuleCollider;
    public LightningEffect lightningAsset;
    LightningEffect lightningEffect;
    bool canMove = false;
    public bool isStartLine = false;
    public bool isLastLine = false;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(isStartLine){
            startPoint.gameObject.SetActive(true);
            var start = Instantiate(startPointEP);
            start.transform.position = transform.position;
            start.transform.parent = transform;
        }
        
    }
    private void Update() {
        timeCount += Time.deltaTime;
        if(timeCount > lightning.lightningPreTime){
            lightning.PlayAudio(1);
            Global.isSlowDown = true;

            timeCount = 0;
            if(follow == null)
                end.transform.position = start.transform.position;
            if(!isChecked){
                isChecked = true;
            }
            DrawLinePoints();
        }
    }
    private void FixedUpdate()
    {
        if(pos.transform.position != player.transform.position && canMove && isLastLine && lightningEffect!= null){
            pos.transform.position = player.transform.position + new Vector3(0,1f,0);

            // pos.transform.position = end.transform.position + new Vector3(0,1f,0);
            // end.transform.position = follow.transform.position;
            // SetCircleLine();
            lightningEffect.pos1.transform.position = pos.transform.position;
            lightningEffect.pos2.transform.position = pos.transform.position;
        }
        if(colliderCur!= null){
            colliderCur.GetComponent<LineConnectCollider>().start = start.transform.position;
            colliderCur.GetComponent<LineConnectCollider>().end = end.transform.position;
            for(int i = 0; i < line.positionCount-1; i++)
            {
                Vector3 midPoint = (line.GetPosition(i) + line.GetPosition(i+1)) / 2;
                colliderCur.transform.position = midPoint;
                colliderCur.transform.parent = transform;
                Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
                capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
                capsuleCollider.isTrigger = true;
                capsuleCollider.radius = line.startWidth/4;
                capsuleCollider.height = direction.magnitude + line.startWidth;
                capsuleCollider.direction = 2;
                if(Vector3.Distance(line.GetPosition(i+1),line.GetPosition(i)) != 0){
                    Quaternion lookRotation = Quaternion.LookRotation(line.GetPosition(i+1) - line.GetPosition(i));
                    colliderCur.transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x,lookRotation.eulerAngles.y,lookRotation.eulerAngles.z);
                }
            }
        }
        line.SetPosition(0,start.transform.position);
        line.SetPosition(1,pos.transform.position);
    }

    public void DrawLinePoints() {
        canMove = false;
        // Global.isSlowDown = true;
        lightningEffect = Instantiate(lightningAsset);
        lightningEffect.transform.parent = transform;
        lightningEffect.pos1.transform.position = start.transform.position;
        lightningEffect.pos2.transform.position = start.transform.position;
        lightningEffect.pos1.transform.DOMove(end.transform.position,startTime);
        lightningEffect.pos2.transform.DOMove(end.transform.position,startTime);
        lightningEffect.pos3.transform.position = start.transform.position;
        lightningEffect.pos4.transform.position = start.transform.position;
        line.startWidth = lightning.lightningWidth;
        line.endWidth = lightning.lightningWidth;
        pos.transform.position = start.transform.position;
        pos.transform.DOMove(end.transform.position,startTime).OnComplete(()=>
        {
            Invoke("EndLine", keepTime);
        });
        Vector3 midPoint = (line.GetPosition(0) + line.GetPosition(1)) / 2;
        colliderCur = Instantiate(lineCollider);
        colliderCur.transform.position = midPoint;
        colliderCur.transform.parent = transform;
        Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
        capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = line.startWidth/4;
        capsuleCollider.height = direction.magnitude + line.startWidth;
        capsuleCollider.direction = 2;
    }

    public void EndLine () {
        lightningEffect.pos3.transform.DOMove(end.transform.position,startTime);
        lightningEffect.pos4.transform.DOMove(end.transform.position,startTime);
        start.transform.DOMove(end.transform.position,startTime).OnComplete(()=>
        {
            canMove = false;
            Destroy(gameObject);
        });
        
    }
}
