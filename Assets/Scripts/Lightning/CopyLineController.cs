using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CopyLineController : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public GameObject pos;
    public LayerMask layer;
    public GameObject follow;
    public float showTime = 0;
    public float startTime = 0.1f;
    public float keepTime = 0.5f;
    LineRenderer line;
    bool canMove = false;
    public float timeCount = 0;
    LightningController lightning;
    PlayerController player;
    Vector3 midPoint;
    public GameObject lineCollider;
    GameObject colliderCur;
    CapsuleCollider capsuleCollider;
    public LightningEffect lightningAsset;
    LightningEffect lightningEffect;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update() {
        timeCount += Time.deltaTime;
        if(timeCount > lightning.lightningPreTime){
            timeCount = 0;
            if(follow == null)
                end.transform.position = start.transform.position;
            DrawLinePoints();
        }
    }
    private void FixedUpdate()
    {
        if(pos.transform.position != follow.transform.position && canMove && follow!= null){
            pos.transform.position = follow.transform.position + new Vector3(0,0.6f,0);
            // end.transform.position = follow.transform.position;
            // SetCircleLine();
            lightningEffect.pos1.transform.position = pos.transform.position;
            lightningEffect.pos2.transform.position = pos.transform.position;
        }
        if(colliderCur != null){
            colliderCur.GetComponent<LineCollider>().start = start.transform.position;
            colliderCur.GetComponent<LineCollider>().end = end.transform.position;
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
            canMove = true;
            lightning.HurtPlayer();
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

        // SetCircleLine();
    }

    public void EndLine () {
        lightningEffect.pos3.transform.DOMove(end.transform.position,startTime);
        lightningEffect.pos4.transform.DOMove(end.transform.position,startTime);
        start.transform.DOMove(end.transform.position,startTime).OnComplete(()=>
        {
            // Global.isSlowDown = false;
            canMove = true;
            var enemys = Transform.FindObjectsOfType<EnemyController>();
            foreach (var item in enemys)
            {
                item.isHitting = false;
            }
            if(follow != null && follow.layer == 7){
                PlayerOnceController copy = follow.GetComponent<PlayerOnceController>();
                copy.lightningCount ++;
            }
            lightning.isEndLight = true;
            Destroy(gameObject);
        });
    }
}
