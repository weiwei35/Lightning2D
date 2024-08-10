using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnectCollider : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 15){
            PaperModel paper = other.gameObject.GetComponent<PaperModel>();
            Debug.Log("符箓：" + other.name);
            //过载符箓
            if(!paper.isOverLoad){
                paper.isOverLoad = true;
                paper.OverLoadFun();
            }
        }
    }
}
