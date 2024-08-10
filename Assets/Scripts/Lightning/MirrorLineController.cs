using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MirrorLineController : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public GameObject pos;
    public LayerMask layer;
    public float showTime = 0;
    public float startTime = 0.1f;
    public float keepTime = 0.5f;
    LineRenderer line;
    public float timeCount = 0;
    public GameObject follow;
    EnemyController enemy;
    LightningController lightning;
    PlayerController player;
    Vector3 midPoint;
    public GameObject lineCollider;
    CapsuleCollider capsule;
    public LightningEffect lightningAsset;
    LightningEffect lightningEffect;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        DrawLinePoints();
    }
    private void Update() {
            
    }
    private void FixedUpdate()
    {
        line.SetPosition(0,start.transform.position);
        line.SetPosition(1,pos.transform.position);
        if(lineCollider!= null)
        {
            midPoint = (line.GetPosition(0) + line.GetPosition(1)) / 2;
            lineCollider.transform.position = midPoint;
            Vector3 direction = line.GetPosition(1) - line.GetPosition(0);
            capsule.isTrigger = true;
            capsule.radius = line.startWidth/2;
            capsule.height = direction.magnitude + line.startWidth;
            capsule.direction = 2;
            if(Vector3.Distance(line.GetPosition(1),line.GetPosition(0)) != 0)
            {Quaternion lookRotation = Quaternion.LookRotation(line.GetPosition(1) - line.GetPosition(0));
            lineCollider.transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x,lookRotation.eulerAngles.y,lookRotation.eulerAngles.z);}
        }
    }
    public void DrawLinePoints() {
        // canMove = false;
        
        lightning.PlayAudio(6);
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
            // canMove = true;
            Invoke("EndLine", keepTime);
        });
        lineCollider = Instantiate(lineCollider);
        lineCollider.transform.position = transform.position;
        lineCollider.transform.parent = transform;
        capsule = lineCollider.GetComponent<CapsuleCollider>();
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
