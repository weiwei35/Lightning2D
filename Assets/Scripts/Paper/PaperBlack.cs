using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PaperBlack : PaperModel
{
    // public float overTime;
    // public float countTime = 0;
    List<GameObject> enemyInHall = new List<GameObject>();
    PaperController paperController;
    int maxEnemy = 8;
    public GameObject boom;
    bool canOverLoad = false;
    // float bigOffset = 0.3f;
    private void Start() {
        paperController = transform.parent.GetComponent<PaperController>();
    }
    private void Update() {
        BlackHall();
        // if(isOverLoad){
        //     countTime += Time.deltaTime;
        // }
        // if(countTime>=overTime && isOverLoad){
        //     countTime = 0;
        //     bigOffset = bigOffset/2;
        //     isOverLoad = false;
        // }
        // if(paperController.isAddLight){
        //     paperController.isAddLight = false;
        //     if(maxEnemy < 8)
        //         SetHallBigger();
        //     else
        //         SetBoom();
        // }
        for (int i = 0; i < enemyInHall.Count; i++)
        {
            if(enemyInHall[i] == null)
            {
                enemyInHall.Remove(enemyInHall[i]);
            }
            // else{
            //     float distance = Vector3.Distance(enemyInHall[i].transform.position,transform.position);
            //     if(distance > 3){
            //         EnemyController enemy = enemyInHall[i].GetComponent<EnemyController>();
            //         enemy.isInBlackHall = false;
            //         enemy.isBoomHall = false;

            //         enemyInHall.Remove(enemyInHall[i]);
            //     }
            // }
        }
    }
    // void SetHallBigger(){
    //     transform.DOScale(transform.localScale.x+bigOffset,0.5f);
    //     maxEnemy ++;
    // }
    // void SetSmaller(){
    //     transform.DOScale(2,0.2f);
    //     maxEnemy = 1;
    // }
    public void SetOverLoad() {
        canOverLoad = true;
        isOverLoad = false;
    }
    void BlackHall(){
        if(enemyInHall.Count < maxEnemy && canOverLoad){
            var enemys = Transform.FindObjectsOfType<EnemyController>();
            List<EnemyController> enemyInArea = new List<EnemyController>();
            foreach (var item in enemys)
            {
                float distance = Vector3.Distance(item.transform.position,transform.position);
                if(distance <= 3){
                    enemyInArea.Add(item);
                }
            }
            if(enemyInArea.Count > 0){
                int id = Random.Range(0,enemyInArea.Count);
                if(!enemyInHall.Exists(t => t == enemyInArea[id].gameObject)){
                    Debug.Log("黑洞吸引"+enemyInArea[id].name);
                    enemyInArea[id].MoveToLine(transform.position);
                    enemyInArea[id].isInBlackHall = true;
                    enemyInArea[id].RandomMoveInHall(transform.position,transform.localScale.x/4);
                    enemyInHall.Add(enemyInArea[id].gameObject);
                }
            }
        }
    }
    public override void OverLoadFun(){
        // bigOffset = bigOffset*2;
        if(canOverLoad)
        {
            SetBoom();
        }
    }

    void SetBoom(){
        foreach (var item in enemyInHall)
        {
            EnemyController enemy = item.GetComponent<EnemyController>();
            // Vector3 direction = enemy.transform.position - transform.position;
            // direction.Normalize();
            enemy.boomDir = transform.position;
            enemy.MoveBoom(transform.position);
            enemy.isBoomHall = true;
        }
        var boomPaper = Instantiate(boom);
        boomPaper.transform.position = transform.position + new Vector3(0,-0.3f,0);
        // SetSmaller();
        var paperBlack = Instantiate(gameObject);
        paperBlack.transform.parent = transform.parent;
        paperBlack.transform.position = transform.position;
        paperBlack.name = "PaperBlack";
        Destroy(gameObject);
    }
}
