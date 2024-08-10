using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    [HideInInspector]
    LineRenderer l;
    public Material m;
    // [HideInInspector]
    public LineController line;
    public LineController lineEndShow;
    public ConnectLineController connectLine;
    public MirrorLineController mirrorLine;
    public GameObject startPoint;//雷点
    GameObject player;
    GameObject playerCopy;
    GameObject[] playerOnceCopy;
    LightningController lightning;
    List<Vector3> points = new List<Vector3>();
    List<Vector3> pointsConnect = new List<Vector3>();
    List<GameObject> lines = new List<GameObject>();
    List<GameObject> lineConnects = new List<GameObject>();
    Vector3 center;
    [Header("半径")]
    public float radius;
    [Header("圆心")]
    public GameObject centerPos;
    [Header("反射雷出现时长")]
    public float mirroeStartTime;
    [Header("反射雷停留时长")]
    public float mirroeKeepTime;
    float myValue = 1f;
    float myValue2 = 1f;
    bool isSide = false;
    bool isSideBack = false;
    bool checkBack = false;
    float angle;
    float saveAngle;
    Tweener tweener1;
    Tweener tweener2;
    public GameObject spark;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        center = centerPos.transform.position;
        DrawCircle ();
        lightning = GetComponent<LightningController>();
    }

    private void Update() {
        //雷电跟随角色
        foreach (var item in lines)
        {
            if(item != null){
                LineController lineController = item.GetComponent<LineController>();
                if(lineController.follow != null)
                    lineController.end.transform.position = lineController.follow.transform.position + new Vector3(0,1f,0);
            }
        }
        //雷电跟随角色
        foreach (var item in lineConnects)
        {
            if(item != null){
                ConnectLineController lineController = item.GetComponent<ConnectLineController>();
                if(lineController.follow != null && lineController.isLastLine)
                    lineController.end.transform.position = player.transform.position;
            }
        }
        // if(Global.isSlowDown){
        //     if(tweener1 != null)
        //         tweener1.timeScale = 0.1f;
        //     if(tweener2 != null)
        //         tweener2.timeScale = 0.1f;
        // }else{
        //     if(tweener1 != null)
        //         tweener1.timeScale = 1f;
        //     if(tweener2 != null)
        //         tweener2.timeScale = 1f;
        // }
        if(Mathf.Abs(Vector3.Distance(player.transform.position,center) - radius) <= 0.1f && !isSide){
            isSide = true;
            angle = Mathf.Atan2(player.transform.position.y - center.y, player.transform.position.x - center.x) * (180 / Mathf.PI);
            spark.SetActive(false);
            spark.transform.rotation = Quaternion.Euler(angle, 90, 0);
            if(angle>=90 && angle<=180)
                angle = angle - 360;
            spark.transform.position = player.transform.position;
            spark.SetActive(true);

            myValue = 1;
            tweener1 = DOTween.To(() => myValue, x => myValue = x, 10f, 0.2f).OnComplete(()=>{
                isSide = false;
                checkBack = true;
            });
        }
        if(checkBack){
            if(Mathf.Abs(Vector3.Distance(player.transform.position,center) - radius) >= 2f && !isSideBack){
                checkBack = false;
                myValue2 = 10;
                saveAngle = angle;
                isSideBack = true;
                tweener2 = DOTween.To(() => myValue2, x => myValue2 = x, 1f, 0.1f).OnComplete(()=>{
                    isSideBack = false;
                });
            }
        }
        
        if(isSide){
            SetWidth(angle+270);
        }
        if(isSideBack){
            ResetWidth(saveAngle+270);
        }

        if(lightning.isEndLight){
            canFind = false;
            foreach (var item in lightPrefab)
            {
                Destroy(item);
            }
        }
        if(canFind){
            GetScreenSlid(lightPrefab);
        }
    }

    //画结界
    public void DrawCircle () {
        l = GetComponent<LineRenderer>();

        l.material = m;
        l.loop = true;
        l.startWidth = 0.1f;
        l.endWidth = 0.1f;
        l.positionCount = 360;

        float angleOnce = 360/l.positionCount;
        Vector3[] circlePoints = new Vector3[l.positionCount];

        Vector3 vecStart = new Vector3(0,radius,0);

        for (int i = 0; i < l.positionCount; i++)
        {
            circlePoints[i] = Quaternion.Euler(0,0,angleOnce*i)*vecStart+center;
        }

        l.SetPositions(circlePoints);
        

    }

    //设置结界边缘穿越效果
    public void SetWidth(float angle) {
        AnimationCurve curve = new AnimationCurve();
        
        curve.AddKey((angle-10)/360f, 1f);
        curve.AddKey(angle/360f, myValue);
        curve.AddKey((angle+10)/360f, 1f);
        l.widthCurve = curve;
        l.widthMultiplier = 0.1f;
    }
    //设置结界边缘穿越效果
    public void ResetWidth(float angle) {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey((angle-10)/360f, 1f);
        curve.AddKey(angle/360f, myValue2);
        curve.AddKey((angle+10)/360f, 1f);
        l.widthCurve = curve;
        l.widthMultiplier = 0.1f;
    }

    //在圆上取count个数的随机点
    public void RandomPoints (float count) {
        points.Clear();
        pointsConnect.Clear();
        //特殊雷点：主角和随机一个怪物连接的射线，左右夹角各15°，中随机一个点位
        Vector3 startPos = new Vector3(player.transform.position.x,player.transform.position.y,0);
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        Vector3 endPos;
        if(enemys.Count() > 0){
            int randomEnemyId = Random.Range(0,enemys.Count());
            GameObject randomEnemy = enemys[randomEnemyId].gameObject;
            endPos = new Vector3(randomEnemy.transform.position.x,randomEnemy.transform.position.y,0);
        }else{
            endPos = Vector3.zero;
        }
        
        Ray ray = new Ray(startPos, endPos - startPos);

        Vector3 intersectionPoint1 = CalculateCircleIntersection(ray.origin, -ray.direction, center, radius);//与随机一怪物相连，和圆的交点
        if (intersectionPoint1 != Vector3.zero) // 如果计算出交点，并且交点不在圆的外部
        {
            // Debug.DrawLine(ray.origin, intersectionPoint1, Color.red, 10f); // 在场景中绘制交点
            Quaternion rotation1 = Quaternion.Euler(0, 0, 15);
            // 计算新的方向向量
            Vector3 direction1 = rotation1*(endPos - startPos);
            Vector3 intersectionPoint2 = CalculateCircleIntersection(startPos, -direction1, center, radius);//正15度，和圆的交点
            // Debug.DrawLine(startPos, intersectionPoint2, Color.red, 10f); // 在场景中绘制交点
            Quaternion rotation2 = Quaternion.Euler(0, 0, -15);
            // 计算新的方向向量
            Vector3 direction2 = rotation2*(endPos - startPos);
            Vector3 intersectionPoint3 = CalculateCircleIntersection(startPos, -direction2, center, radius);//负15度，和圆的交点
            // Debug.DrawLine(startPos, intersectionPoint3, Color.red, 10f); // 在场景中绘制交点
            // 你可以在这里处理交点信息
            float angle =RandomAngle(intersectionPoint2,intersectionPoint3);
            Vector3 point = center + new Vector3(Mathf.Cos(angle * Mathf.PI/180) * radius,Mathf.Sin(angle * Mathf.PI/180) * radius,0);
            points.Add(point);
        }
        //普通雷点
        for (int i = 0; i < count-1;)
        {
            bool canSave = true;
            Vector3 pointCur = GetPoint();
            foreach (var item in points)
            {
                // 计算圆心到点的方向向量
                Vector2 direction1 = item - center;
                // 标准化方向向量以便于计算角度
                direction1.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;

                // 计算圆心到点的方向向量
                Vector2 direction2 = pointCur - center;
                // 标准化方向向量以便于计算角度
                direction2.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;

                if(Mathf.Abs(angle1-angle2) < 5){
                    canSave = false;
                    Debug.Log("两雷点离得太近");
                }
            }
            if(canSave){
                points.Add(pointCur);
                i++;
            }
        }
        PlayerController playerController = player.GetComponent<PlayerController>();
        if(playerController.isPaperConnect && Global.papersPosList.Count>0){
            Vector3 pointPos = playerController.GetSymmetricPosition(points[0],centerPos.transform.position);
            pointsConnect.Add(pointPos);
            foreach (var item in Global.papersPosList)
            {
                pointsConnect.Add(item);
            }
            SetConnectLines(lightning.startTime,lightning.keepTime,pointsConnect);
        }
        SetLines(lightning.startTime,lightning.keepTime,points);
        lightPrefab.Clear();
        foreach (var item in points)
        {
            lightPrefab.Add(Instantiate(startPoint));
            canFind = true;
        }
        GetScreenSlid(lightPrefab);
    }
    //关底自动雷电生成雷点
    public void CirclePoints () {
        points.Clear();
        pointsConnect.Clear();
        //普通雷点
        for (int i = 0; i < 20;)
        {
            float angle = (Mathf.PI * 2/20)*i;
            Vector3 pointCur = center + new Vector3(Mathf.Cos(angle) * radius,Mathf.Sin(angle) * radius,0);
            pointCur = new Vector3(pointCur.x,pointCur.y,-5);
            points.Add(pointCur);
            i++;
        }
        SetCircleLines(lightning.startTime,lightning.keepTime,points);
        lightPrefab.Clear();
        foreach (var item in points)
        {
            lightPrefab.Add(Instantiate(startPoint));
            canFind = true;
        }
        GetScreenSlid(lightPrefab);
    }
    //在圆上取count个数的随机点
    public void RandomPointsMirror (float count) {
        List<Vector3> pointsMirror = new List<Vector3>();
        pointsMirror.Clear();
        //普通雷点
        for (int i = 0; i < count;)
        {
            bool canSave = true;
            Vector3 pointCur = GetPoint();
            foreach (var item in pointsMirror)
            {
                // 计算圆心到点的方向向量
                Vector2 direction1 = item - center;
                // 标准化方向向量以便于计算角度
                direction1.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;

                // 计算圆心到点的方向向量
                Vector2 direction2 = pointCur - center;
                // 标准化方向向量以便于计算角度
                direction2.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;

                if(Mathf.Abs(angle1-angle2) < 15){
                    canSave = false;
                    Debug.Log("两雷点离得太近");
                }
            }
            if(canSave){
                pointsMirror.Add(pointCur);
                i++;
            }
        }
        SetLinesMirror(mirroeStartTime,mirroeStartTime,pointsMirror);
    }
    public void RandomPointsCopy (float count) {
        List<Vector3> pointsCopy = new List<Vector3>();
        pointsCopy.Clear();
        //普通雷点
        for (int i = 0; i < count;)
        {
            bool canSave = true;
            Vector3 pointCur = GetPoint();
            foreach (var item in pointsCopy)
            {
                // 计算圆心到点的方向向量
                Vector2 direction1 = item - center;
                // 标准化方向向量以便于计算角度
                direction1.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;

                // 计算圆心到点的方向向量
                Vector2 direction2 = pointCur - center;
                // 标准化方向向量以便于计算角度
                direction2.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;

                if(Mathf.Abs(angle1-angle2) < 15){
                    canSave = false;
                    Debug.Log("两雷点离得太近");
                }
            }
            if(canSave){
                pointsCopy.Add(pointCur);
                i++;
            }
        }
        PlayerController playerController = player.GetComponent<PlayerController>();
        if(playerController.isMegaCopy){
            foreach(var copy in Global.playerCopyList){
                SetLinesMirrorCopy(mirroeStartTime,mirroeStartTime,pointsCopy,copy);
            }
        }
    }
    //在圆上取随机点
    Vector3 GetPoint(){
        float angle = Random.Range(0,Mathf.PI * 2);
        Vector3 point = center + new Vector3(Mathf.Cos(angle) * radius,Mathf.Sin(angle) * radius,0);
        return point;
    }

    //计算射线与圆的交点
    Vector3 CalculateCircleIntersection(Vector3 rayOrigin, Vector3 rayDirection, Vector3 circleCenter, float circleRadius)
    {
        Vector3 relativeOrigin = rayOrigin - circleCenter;
        float a = Vector3.Dot(rayDirection, rayDirection);
        float b = 2 * Vector3.Dot(rayDirection, relativeOrigin);
        float c = Vector3.Dot(relativeOrigin, relativeOrigin) - circleRadius * circleRadius;
 
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            return Vector3.zero; // 无交点
        }
 
        float t = (-b - Mathf.Sqrt(discriminant)) / (2 * a); // 求解二次方程
        return rayOrigin + rayDirection * t; // 计算交点
    }

    //圆上两点间取一个随机点
    float RandomAngle(Vector3 point1,Vector3 point2){
        // 计算圆心到点的方向向量
        Vector2 direction1 = point1 - center;
 
        // 标准化方向向量以便于计算角度
        direction1.Normalize();
 
        // 计算点和圆心连线与正X轴的夹角（角度值）
        float angle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;
        // Debug.Log(angle1);

        // 计算圆心到点的方向向量
        Vector2 direction2 = point2 - center;
 
        // 标准化方向向量以便于计算角度
        direction2.Normalize();
 
        // 计算点和圆心连线与正X轴的夹角（角度值）
        float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
        // Debug.Log(angle2);
        float angle;
        if(angle2 > angle1)
            angle = Random.Range(angle1,angle2);
        else
            angle = Random.Range(angle2,angle1);
        // Debug.Log(angle);
        return angle;
    }

    //从随机点生成连向主角的线段
    public void SetLines (float startTime,float keepTime,List<Vector3> points) {
        float timeOffset = 0;
        timeOffset = (lightning.lightningPreTime/2)/points.Count;
        int i = 0;
        foreach(var point in points){
            var lineCur = Instantiate(line.gameObject);
            lineCur.transform.position = point;
            LineController lineController = lineCur.GetComponent<LineController>();
            lineController.start.transform.position = point;
            lineController.end.transform.position = player.transform.position;
            lineController.startTime = startTime;
            lineController.keepTime = keepTime;
            lineController.follow = player;
            lineController.showTime = i * timeOffset;
            i++;
            lines.Add(lineCur.gameObject);
        }
        foreach (var item in lightPrefab)
        {
            Destroy(item);
        }
        lightPrefab.Clear();
    }
    public void SetCircleLines (float startTime,float keepTime,List<Vector3> points) {
        foreach(var point in points){
            var lineCur = Instantiate(lineEndShow.gameObject);
            lineCur.transform.position = point;
            LineController lineController = lineCur.GetComponent<LineController>();
            lineController.start.transform.position = point;
            lineController.end.transform.position = center;
            lineController.startTime = startTime;
            lineController.keepTime = keepTime;
            lineController.follow = centerPos;
            lineController.timeCount = lightning.lightningPreTime +1;
            lines.Add(lineCur.gameObject);
        }
        foreach (var item in lightPrefab)
        {
            Destroy(item);
        }
        lightPrefab.Clear();
    }
    public void SetLinesMirror (float startTime,float keepTime,List<Vector3> points) {
        foreach(var point in points){
            var lineCur = Instantiate(mirrorLine.gameObject);
            lineCur.transform.position = player.transform.position;
            MirrorLineController lineController = lineCur.GetComponent<MirrorLineController>();
            lineController.start.transform.position = player.transform.position + new Vector3(0,1f,0);
            lineController.end.transform.position = point;
            lineController.startTime = startTime;
            lineController.keepTime = keepTime;
            lineController.showTime = 0;
            lineController.follow = player;
            lineController.timeCount = lightning.lightningPreTime +1;
        }
    }

    //分身反射雷
    public void SetLinesMirrorCopy (float startTime,float keepTime,List<Vector3> points,GameObject copy) {
        foreach(var point in points){
            var lineCur = Instantiate(mirrorLine.gameObject);
            lineCur.transform.position = copy.transform.position;
            MirrorLineController lineController = lineCur.GetComponent<MirrorLineController>();
            lineController.start.transform.position = copy.transform.position + new Vector3(0,1f,0);
            lineController.end.transform.position = point;
            lineController.startTime = startTime;
            lineController.keepTime = keepTime;
            lineController.showTime = 0;
            lineController.follow = copy;
            lineController.timeCount = lightning.lightningPreTime +1;
        }
    }

    //串联电路
    public void SetConnectLines (float startTime,float keepTime,List<Vector3> points) {
        StartCoroutine(IterateWithDelay(startTime,keepTime,points));
        
    }
    IEnumerator IterateWithDelay(float startTime,float keepTime,List<Vector3> points)
    {
        float maxDistance = 0;
        for (int i = 0; i < points.Count-1; i++)
        {
            maxDistance += Vector3.Distance(points[i],points[i+1]);
        }
        maxDistance += Vector3.Distance(player.transform.position,points[points.Count-1]);
        for (int i = 0; i < points.Count-1; i++)
        {
            var lineCur = Instantiate(connectLine.gameObject);
            lineCur.transform.position = points[i];
            ConnectLineController lineController = lineCur.GetComponent<ConnectLineController>();
            lineController.start.transform.position = points[i];
            lineController.end.transform.position = points[i+1];
            float distance = Vector3.Distance(points[i],points[i+1]);
            lineController.startTime = startTime * (distance/maxDistance);
            lineController.keepTime = keepTime + (startTime - lineController.startTime);
            lineController.follow = points[i+1];
            lineConnects.Add(lineCur.gameObject);

            if(i==0){
                lineController.isStartLine = true;
            }

            yield return new WaitForSeconds(startTime * (distance/maxDistance));
        }
        var lineCurEnd = Instantiate(connectLine.gameObject);
        lineCurEnd.transform.position = points[points.Count-1];
        ConnectLineController lineControllerEnd = lineCurEnd.GetComponent<ConnectLineController>();
        lineControllerEnd.start.transform.position = points[points.Count-1];
        lineControllerEnd.end.transform.position = player.transform.position;
        float dis = Vector3.Distance(points[points.Count-1],player.transform.position);
        lineControllerEnd.startTime = startTime * (dis/maxDistance);
        lineControllerEnd.keepTime = keepTime;
        lineControllerEnd.follow = player.transform.position;
        lineControllerEnd.isLastLine = true;
        lineConnects.Add(lineCurEnd.gameObject);
    }

    //获取屏幕边缘
    bool canFind = false;
    List<GameObject> lightPrefab = new List<GameObject>();
    void GetScreenSlid(List<GameObject> light){
        // 获取屏幕中心点
        Camera mainCamera = Camera.main;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 screenCenter = new Vector3(screenWidth * 0.5f, screenHeight * 0.5f,0);
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);

        //获取屏幕 4个顶点
        float cameraHeight = mainCamera.orthographicSize; // 如果是正交摄像机，获取宽度
        float cameraWidth = cameraHeight * mainCamera.aspect; // 获取高度，考虑屏幕比率
        float maxX = worldCenter.x + cameraWidth - 0.5f;
        float minX = worldCenter.x - cameraWidth + 0.5f;
        float maxY = worldCenter.y + cameraHeight;
        float minY = worldCenter.y - cameraHeight + 2;

        Vector2 rectTopLeft = new Vector2(minX,maxY);
        Vector2 rectBottomRight = new Vector2(maxX,minY);
        Vector2 rectBottonLeft = new Vector2(minX,minY);
        Vector2 rectTopRight = new Vector2(maxX,maxY);
        //对角线夹角
        float angle = Vector2.Angle(rectTopLeft - rectBottomRight,rectBottonLeft - rectBottomRight);
        int i = 0;
        foreach (var lightPos in points)
        {
            //角色与雷点夹角
            float anglePlayer = Vector2.Angle(lightPos - player.transform.position,Vector2.right);

            // GameObject light = Instantiate(startPoint);
            if(lightPos.x>maxX || lightPos.x<minX || lightPos.y>maxY || lightPos.y<minY)
            {
                {
                    if(0<anglePlayer && anglePlayer<angle){
                        //与右边相交
                        Vector2 point = SegmentsInterPoint(lightPos,player.transform.position,rectBottomRight,rectTopRight);
                        light[i].SetActive(true);
                        light[i].transform.position = point;
                    }
                    if(180-angle<anglePlayer && anglePlayer<180){
                        //与左边相交
                        Vector2 point = SegmentsInterPoint(lightPos,player.transform.position,rectBottonLeft,rectTopLeft);
                        light[i].SetActive(true);
                        light[i].transform.position = point;
                    }
                    if(angle<anglePlayer && anglePlayer<180-angle){
                        if(player.transform.position.y > lightPos.y){
                            //与下边相交
                            Vector2 point = SegmentsInterPoint(lightPos,player.transform.position,rectBottomRight,rectBottonLeft);
                            light[i].SetActive(true);
                            light[i].transform.position = point;
                        }
                        if(player.transform.position.y < lightPos.y){
                            //与上边相交
                            Vector2 point = SegmentsInterPoint(lightPos,player.transform.position,rectTopLeft,rectTopRight);
                            light[i].SetActive(true);
                            light[i].transform.position = point;
                        }
                    }
                }
            }else{
                light[i].SetActive(false);
            }
            i++;
        }
        
    }
    public static Vector2 SegmentsInterPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        //计算交点坐标  
        float t = Cross(a -c, d -c) / Cross (d-c,b-a);
        float dx = t * (b.x - a.x);
        float dy = t * (b.y - a.y);

        return new Vector2() { x = a.x + dx, y = a.y + dy };
    }
    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - b.x * a.y;
    }
}
