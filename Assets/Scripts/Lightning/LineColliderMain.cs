using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColliderMain : MonoBehaviour
{
    LightningController lightning;
    public Vector3 start;
    public Vector3 end;
    PlayerController player;
    private void OnTriggerEnter(Collider other) {
        lightning = FindObjectOfType<LightningController>();
        player = FindObjectOfType<PlayerController>();
        if(other.gameObject.layer == 6){
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            if(enemy != null && !enemy.isHitting)
            {
                // Debug.Log("雷电攻击：" + other.name);
                lightning.HurtEnemy(enemy,HurtType.Lightning);
            }
        }
        if(other.gameObject.layer == 8 && player.isLightningAttract){
            //需要吸附对应敌人
            EnemyController enemy = other.gameObject.GetComponentInParent<EnemyController>();
            if(enemy != null && enemy.canHurt)
            {
                // Debug.Log("雷电吸附：" + other.name);
                enemy.MoveToLine(GetCrossPoin(other.gameObject));
            }
        }
        if(other.gameObject.layer == 15){
            PaperModel paper = other.gameObject.GetComponent<PaperModel>();
            // Debug.Log("符箓：" + other.name);
            //过载符箓
            if(!paper.isOverLoad){
                paper.isOverLoad = true;
                paper.OverLoadFun();
            }
        }
        if(other.gameObject.layer == 17){
            other.gameObject.SetActive(false);
        }
    }

    //获取敌人与雷电交点
    Vector3 GetCrossPoin(GameObject pointA){
        Vector3 lineEnd = new Vector3(end.x,end.y,-5);
        Vector3 lineStart = new Vector3(start.x,start.y,-5);

        // 计算线段 L 的方向向量
        Vector3 lineDirection = (lineEnd - lineStart).normalized;

        // 计算 A 点到线段起点的向量
        Vector3 pointAToLineStart = pointA.transform.position - lineStart;

        // 计算 A 点到线段上的最近点的向量
        float t = Vector3.Dot(pointAToLineStart, lineDirection);
        t = Mathf.Clamp01(t / (lineEnd - lineStart).magnitude);
        Vector3 closestPointOnLine = lineStart + t * (lineEnd - lineStart);
        return closestPointOnLine;
    }
}
