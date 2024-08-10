using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public class TrailCollider : MonoBehaviour
{
    //拖尾特效持续时长
    public static float overTime = 2f;
    public List<SlowCollider> pointList = new List<SlowCollider>();
    private int maxPoints = 100;
    public int minPixelMove = 5;    // Must move at least this many pixels per sample for a new segment to be recorded
    private Vector3 previousPosition;
    private int sqrMinPixelMove;
    private bool canDraw = false;
    PlayerController player;
    public GameObject PointParent;
 
    void Start()
    {
        sqrMinPixelMove = minPixelMove * minPixelMove;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
 
    void Update()
    {
        if (pointList.Count >= maxPoints)
        {
            canDraw = false;
        }else{
            canDraw = true;
        }
        if((player.moveX != 0 || player.moveY != 0) && canDraw)
        {
            previousPosition = player.transform.position;
            AddPoint(GetMousePos());
            // canDraw = true;
        }
        else if ((player.moveX != 0 || player.moveY != 0) && (player.transform.position - previousPosition).sqrMagnitude > sqrMinPixelMove && canDraw)
        {
            previousPosition = player.transform.position;
            AddPoint(GetMousePos());
        }
        if (pointList.Count > 0)
        {
            CheckOverTime();
        }
    }
 
    Vector3 GetMousePos()
    {
        Vector3 pos = player.transform.position;
        return pos;
    }
 
    /// <summary>
    /// 增加顶点
    /// </summary>
    public void AddPoint(Vector3 pos)
    {
        GameObject go = GameObjectPoolTool.GetFromPoolForce(true,"Assets/Resources/SlowCollider.prefab");
        go.transform.position = pos + new Vector3(0,1,0);
        go.transform.parent = PointParent.transform;
        SlowCollider point = go.GetComponent<SlowCollider>();
        point.createTime = Time.realtimeSinceStartup;
        pointList.Add(point);
    }
 
    void CheckOverTime()
    {
        //移除超时的点
        for (int i = 0; i < pointList.Count; i++)
        {            
            if (Time.realtimeSinceStartup - pointList[i].createTime > overTime)
            {
                if(pointList[i] != null){
                    Destroy(pointList[i].gameObject);
                    pointList.RemoveAt(i);
                }
                i--;
            }
        }
    }
}
