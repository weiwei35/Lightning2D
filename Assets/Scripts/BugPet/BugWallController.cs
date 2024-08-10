using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugWallController : BugController
{
    [Header("墙壁作用时长")]
    public float wallTime = 5;
    [Header("墙壁预制体")]
    public GameObject wallPre;
    float countTime = 0;
    bool startWall = false;
    GameObject wall;
    public override void Update()
    {
        base.Update();
        if(energyCurrent >= energy){
            canRecoverEnergy = false;
            energyCurrent = 0;
            SetWall();
        }
        if(startWall && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime >= wallTime){
            countTime = 0;
            isTriggered = false;
            startWall = false;
            canRecoverEnergy = true;
        }
    }

    void SetWall(){
        isTriggered = true;

        startWall = true;
        wall = Instantiate(wallPre);
        BugWallCollider bugWall = wall.GetComponent<BugWallCollider>();
        bugWall.wallTime = wallTime;
        wall.transform.position = transform.position;
    }
}
