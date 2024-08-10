using UnityEngine;
using DG.Tweening;
using System.Collections;
public class LineController : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public GameObject pos;
    public LayerMask layer;
    public GameObject follow;
    public GameObject startPointEP;//特殊雷点
    public GameObject startPoint;
    public float showTime = 0;
    public float startTime = 0.1f;
    public float keepTime = 0.5f;
    LineRenderer line;
    LineRenderer lineB;
    public GameObject copyLine;
    bool canMove = false;
    public bool isChecked = false;
    public float timeCount = 0;
    LightningController lightning;
    PlayerController player;
    public GameObject lineCollider;
    GameObject colliderCur;
    CapsuleCollider capsuleCollider;
    public LightningEffect lightningAsset;
    LightningEffect lightningEffect;

    //冲刺折线相关
    Vector3 endPos1;
    public bool isRush = false;
    public GameObject lineRush;
    public bool canCopy = true;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lineB = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playsound = false;
        StartCoroutine(SetStartPoint());
    }
    //依次出现雷点
    IEnumerator SetStartPoint(){
        yield return new WaitForSeconds(showTime);
        // lightning.PlayAudio(5);
        startPoint.gameObject.SetActive(true);
        var start = Instantiate(startPointEP);
        start.transform.position = transform.position;
        start.transform.parent = transform;
    }
    bool playsound = false;
    private void Update() {
        timeCount += Time.deltaTime;
        if(timeCount > lightning.lightningPreTime - 0.5f){
            if(Input.GetKeyDown(KeyCode.Space) && !Global.isSlowDown && player.rushing && player.skill_rush){
                endPos1 = player.transform.position;
                isRush = true;
            }
        }
        if(timeCount > lightning.lightningPreTime - 0.1f){
            Global.isSlowDown = true;
        }
        if(timeCount > lightning.lightningPreTime - 0.1f && !playsound){
            lightning.PlayLightningAudio();
            playsound = true;
        }
        if(timeCount > lightning.lightningPreTime){
            // Global.isSlowDown = true;
            timeCount = 0;
            if(follow == null)
                end.transform.position = start.transform.position;
            if(!isChecked && canCopy){
                isChecked = true;
                CheckCopy();
            }
            DrawLinePoints();
        }
    }
    private void FixedUpdate()
    {
        if(!isRush)
        {if(pos.transform.position != follow.transform.position && canMove && follow!= null){
            pos.transform.position = follow.transform.position + new Vector3(0,1f,0);
            // end.transform.position = follow.transform.position;
            // SetCircleLine();
            lightningEffect.pos1.transform.position = pos.transform.position;
            lightningEffect.pos2.transform.position = pos.transform.position;
        }
        if(colliderCur!= null){
            colliderCur.GetComponent<LineColliderMain>().start = start.transform.position;
            colliderCur.GetComponent<LineColliderMain>().end = end.transform.position;
            for(int i = 0; i < line.positionCount-1; i++)
            {
                Vector3 midPoint = (line.GetPosition(i) + line.GetPosition(i+1)) / 2;
                colliderCur.transform.position = midPoint;
                colliderCur.transform.parent = transform;
                Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
                capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
                if(capsuleCollider != null){
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
        }
        line.SetPosition(0,start.transform.position);
        line.SetPosition(1,pos.transform.position);}
        else{
            end.transform.position = endPos1;
            // if(pos.transform.position != endPos1 && canMove && follow!= null){
            //     pos.transform.position = endPos1;
            //     lightningEffect.pos1.transform.position = endPos1;
            //     lightningEffect.pos2.transform.position = endPos1;
            // }
            if(colliderCur!= null){
                colliderCur.GetComponent<LineColliderMain>().start = start.transform.position;
                colliderCur.GetComponent<LineColliderMain>().end = endPos1;
                for(int i = 0; i < line.positionCount-1; i++)
                {
                    Vector3 midPoint = (line.GetPosition(i) + line.GetPosition(i+1)) / 2;
                    colliderCur.transform.position = midPoint;
                    colliderCur.transform.parent = transform;
                    Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
                    capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
                    if(capsuleCollider != null){
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
            }
            line.SetPosition(0,start.transform.position);
            line.SetPosition(1,endPos1);
        }
    }

    public void CheckCopy() {
        if(player.GetComponent<PlayerController>().isOnceLightningCopy || player.GetComponent<PlayerController>().isOnceTimeCopy){
            GameObject[] playerOnceCopy = GameObject.FindGameObjectsWithTag("PlayerOnceCopy");
            if(playerOnceCopy.Length > 0){
                foreach (var item in playerOnceCopy)
                {
                    var lineCurCopy = Instantiate(copyLine);
                    lineCurCopy.transform.position = transform.position;
                    CopyLineController lineControllerCopy = lineCurCopy.GetComponent<CopyLineController>();
                    lineControllerCopy.start.transform.position = start.transform.position;
                    lineControllerCopy.end.transform.position = new Vector3(item.transform.position.x,item.transform.position.y,-5) + new Vector3(0,0.6f,0);
                    lineControllerCopy.startTime = startTime;
                    lineControllerCopy.keepTime = keepTime;
                    lineControllerCopy.follow = item;
                    lineControllerCopy.timeCount = lightning.lightningPreTime+1;
                }
            }
        }
        if(player.GetComponent<PlayerController>().isCircleCopy){
                GameObject playerCopy = GameObject.FindGameObjectWithTag("PlayerCopy");
                var lineCurCopy = Instantiate(copyLine);
                lineCurCopy.transform.position = transform.position;
                CopyLineController lineControllerCopy = lineCurCopy.GetComponent<CopyLineController>();
                lineControllerCopy.start.transform.position = start.transform.position;
                lineControllerCopy.end.transform.position = new Vector3(playerCopy.transform.position.x,playerCopy.transform.position.y,-5) + new Vector3(0,0.6f,0);
                lineControllerCopy.startTime = startTime;
                lineControllerCopy.keepTime = keepTime;
                lineControllerCopy.follow = playerCopy;
                lineControllerCopy.timeCount = lightning.lightningPreTime+1;
        }
    }

    public void DrawLinePoints() {
        canMove = false;
        // Global.isSlowDown = true;
        lightningEffect = Instantiate(lightningAsset);
        lightningEffect.transform.parent = transform;
        lightningEffect.pos1.transform.position = start.transform.position;
        lightningEffect.pos2.transform.position = start.transform.position;
        if(!isRush)
        {
            lightningEffect.pos1.transform.DOMove(end.transform.position,startTime);
            lightningEffect.pos2.transform.DOMove(end.transform.position,startTime);
            pos.transform.position = start.transform.position;
            pos.transform.DOMove(end.transform.position,startTime).OnComplete(()=>
            {
                Invoke("EndLine", keepTime);
                Invoke("SetEndLine", 0);
            });
        }
        else{
            lightningEffect.pos1.transform.DOMove(endPos1,startTime);
            lightningEffect.pos2.transform.DOMove(endPos1,startTime);
            pos.transform.position = start.transform.position;
            pos.transform.DOMove(endPos1,startTime).OnComplete(()=>
            {
                Invoke("EndLine", keepTime);
                Invoke("SetEndLine", 0);

                var lineCur = Instantiate(lineRush);
                lineCur.transform.position = endPos1;
                LineController lineController = lineCur.GetComponent<LineController>();
                lineController.start.transform.position = endPos1;
                lineController.end.transform.position = player.transform.position + new Vector3(0,1f,0);;
                lineController.startTime = startTime;
                lineController.keepTime = keepTime;
                lineController.follow = player.gameObject;
                lineController.isRush = false;
                lineController.timeCount = lightning.lightningPreTime+1;
                lineController.canCopy = false;
            });
        }
        lightningEffect.pos3.transform.position = start.transform.position;
        lightningEffect.pos4.transform.position = start.transform.position;
        line.startWidth = lightning.lightningWidth;
        line.endWidth = lightning.lightningWidth;
        
        Vector3 midPoint = (line.GetPosition(0) + line.GetPosition(1)) / 2;
        colliderCur = Instantiate(lineCollider);
        colliderCur.transform.position = midPoint;
        colliderCur.transform.parent = transform;
        Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
        if(capsuleCollider != null){
            capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = true;
            capsuleCollider.radius = line.startWidth/4;
            capsuleCollider.height = direction.magnitude + line.startWidth;
            capsuleCollider.direction = 2;
        }
        // SetCircleLine();
    }

    public void EndLine () {
        lightning.isSetLight = false;
        Vector3 endPos;
        if(!isRush)
        {
            lightningEffect.pos3.transform.DOMove(end.transform.position,startTime);
            lightningEffect.pos4.transform.DOMove(end.transform.position,startTime);

            endPos = end.transform.position;
        }
        else
        {
            lightningEffect.pos3.transform.DOMove(endPos1,startTime);
            lightningEffect.pos4.transform.DOMove(endPos1,startTime);

            endPos = endPos1;
        }

        start.transform.DOMove(endPos,startTime).OnComplete(()=>
        {
            Global.isSlowDown = false;
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

    public void SetEndLine () {
        if(!Global.isChangeLevel)
            lightning.HurtPlayer();
        lightning.isSetLight = true;
    }

    private void OnDestroy() {
        isRush = false;
    }
    void SetCircleLine(){
        lineB.positionCount = 100;
 
        Vector3 point0 = start.transform.position; // 起始点
        Vector3 point3 = end.transform.position; // 结束点
        Vector3 point1 = CenterPoint(point0,point3); // 控制点
        Vector3 point2 = SidePoint(point0,point3); // 控制点
 
        for (int i = 0; i < 100; i++)
        {
            float t = i / (float)(100 - 1); // 参数t，从0到1，分割曲线
            Vector3 point = CalculateCubicBezierPoint(point0,point1, point2, point3, t);
            lineB.SetPosition(i, point);
        }
    }

    //取两点之间的控制点
    Vector3 CenterPoint(Vector3 pointA, Vector3 pointB){
        Vector3 vectorAB = pointB - pointA; // 两点间的向量
        Vector3 direction = vectorAB.normalized; // 方向向量
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up); // 垂直于AB的向量
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, 45) * direction; // 旋转方向向量
        Vector3 pointC = pointA + rotatedDirection * (vectorAB.magnitude); // 新的终点坐标
 
        Debug.DrawLine(pointA, pointB, Color.blue); // 绘制起点和终点之间的连线
        Debug.DrawLine(pointA, pointC, Color.red); // 绘制起点和旋转后终点之间的连线
        Debug.Log("1:"+pointC);
        return pointC;
    }
    //取两点之间的控制点
    Vector3 SidePoint(Vector3 pointB, Vector3 pointA){
        Vector3 vectorAB = pointB - pointA; // 两点间的向量
        Vector3 direction = vectorAB.normalized; // 方向向量
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up); // 垂直于AB的向量
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, 45) * direction; // 旋转方向向量
        Vector3 pointC = pointA + rotatedDirection * (vectorAB.magnitude/3); // 新的终点坐标
 
        Debug.DrawLine(pointA, pointB, Color.blue); // 绘制起点和终点之间的连线
        Debug.DrawLine(pointA, pointC, Color.red); // 绘制起点和旋转后终点之间的连线
        Debug.Log("2:"+pointC);
        return pointC;
    }
    //贝塞尔曲线
    Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * u * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += tt * t * p3;
        return p;
    }
}
