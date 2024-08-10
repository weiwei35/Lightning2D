using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperIce : PaperModel
{
    public float overTime;
    public float countTime = 0;
    SpriteRenderer sprite;
    public GameObject singleC;
    public GameObject doubleC;
    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update() {
        if(isOverLoad && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime>=overTime && isOverLoad){
            countTime = 0;
            isOverLoad = false;
            sprite.color = Color.white;
            singleC.SetActive(true);
            doubleC.SetActive(false);
        }
    }
    public override void OverLoadFun(){
        sprite.color = Color.blue;
        singleC.SetActive(false);
        doubleC.SetActive(true);
    }
}
