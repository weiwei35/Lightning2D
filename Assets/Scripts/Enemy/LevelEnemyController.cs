using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelEnemyController : MonoBehaviour
{
    public LevelEnemyDataSO levelEnemy;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    float timeCount = 0;
    // Update is called once per frame
    void Update()
    {
        if(Global.GameBegain&&!Global.isSlowDown){
            timeCount += Time.deltaTime;
        }
        for (int i = 0; i < levelEnemiesCur.Count; i++)
        {
            if(timeCount > levelEnemiesCur[i].firstTime && !enemyFirstAdd[i] && Global.GameBegain&&!Global.isSlowDown){
                SetEnemy(levelEnemiesCur[i]);
                enemyFirstAdd[i] = true;
            }
        }
    }
    List<LevelEnemy> levelEnemiesCur = new List<LevelEnemy>();
    List<bool> enemyFirstAdd = new List<bool>();
    int levelId;
    public void SetLevel(int level) {
        levelId = level;
        foreach (var item in levelEnemy.enemys)
        {
            if(item.levelGroup.Contains(level)){
                levelEnemiesCur.Add(item);
                enemyFirstAdd.Add(false);
            }
        }
    }

    public void SetRewardLevel(int level){
        foreach (var item in levelEnemy.enemys)
        {
            if(item.rewardLevelGroup.Contains(level)){
                levelEnemiesCur.Clear();
                enemyFirstAdd.Clear();
                levelEnemiesCur.Add(item);
                enemyFirstAdd.Add(false);
            }
        }
    }

    void SetEnemy(LevelEnemy enemy){
        GameObject enemyPre = Resources.Load("Enemy/"+enemy.enemyId,typeof(GameObject)) as GameObject;
        int enemyCount = Random.Range(enemy.count[0], enemy.count[1]+1);
        Vector3 areaCenter = Vector3.zero;
        int areaRadius = 0;
        if(enemy.isGroup){
            //x:-18,30;y:-24,24
            int x = Random.Range(-18,31);
            int y = Random.Range(-24,25);
            areaCenter = new Vector3(x,y,-5);
            areaRadius = Random.Range(enemy.groupArea[0],enemy.groupArea[1]+1);
        }
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyObj = Instantiate(enemyPre) as GameObject;
            enemyObj.transform.SetParent(this.transform);

            EnemyController enemyController = enemyObj.GetComponent<EnemyController>();
            enemyController.HP = enemy.hp + (levelId - 1)*enemy.hpGrow;
            enemyController.attack = enemy.hurt + (levelId - 1)*enemy.hurtGrow;
            int speed = Random.Range(enemy.speed[0], enemy.speed[1]+1);
            speed = (int)(speed + (levelId - 1)*enemy.speedGrow);
            enemyController.speed = speed/10f;

            if(enemy.isCircleSide){
                enemyController.SpawnAtRandomCircle();
            }
            if(enemy.isGroup){
                enemyController.SpawnAtRandomGroup(areaCenter,areaRadius);
            }
        }
        if(enemy.repeatTime > 0)
            StartCoroutine(SetRepeatEnemy(enemy));
        else if(enemy.repeatTime > 0 && Global.isReward){
            StartCoroutine(SetRewardRepeatEnemy(enemy));
        }
    }

    IEnumerator SetRepeatEnemy(LevelEnemy enemy){
        if(!Global.isReward){
            yield return new WaitForSeconds(enemy.repeatTime);
            SetEnemyArray(enemy);
            StartCoroutine(SetRepeatEnemy(enemy));
        }
    }
    IEnumerator SetRewardRepeatEnemy(LevelEnemy enemy){
        if(Global.isReward){
            yield return new WaitForSeconds(enemy.repeatTime);
            SetEnemyArray(enemy);
            StartCoroutine(SetRepeatEnemy(enemy));
        }
    }

    void SetEnemyArray(LevelEnemy enemy){
        GameObject enemyPre = Resources.Load("Enemy/"+enemy.enemyId,typeof(GameObject)) as GameObject;
        int enemyCount = Random.Range(enemy.count[0], enemy.count[1]+1);
        Vector3 areaCenter = Vector3.zero;
        int areaRadius = 0;
        if(enemy.isGroup){
            //x:-18,30;y:-24,24
            int x = Random.Range(-18,31);
            int y = Random.Range(-24,25);
            areaCenter = new Vector3(x,y,-5);
            areaRadius = Random.Range(enemy.groupArea[0],enemy.groupArea[1]+1);
        }
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyObj = Instantiate(enemyPre) as GameObject;
            enemyObj.transform.SetParent(this.transform);
            EnemyController enemyController = enemyObj.GetComponent<EnemyController>();
            enemyController.HP = enemy.hp + (levelId - 1)*enemy.hpGrow;
            enemyController.attack = enemy.hurt + (levelId - 1)*enemy.hurtGrow;
            int speed = Random.Range(enemy.speed[0], enemy.speed[1]+1);
            speed = (int)(speed + (levelId - 1)*enemy.speedGrow);
            enemyController.speed = speed/10f;
            if(enemy.isCircleSide){
                enemyController.SpawnAtRandomCircle();
            }
            if(enemy.isGroup){
                enemyController.SpawnAtRandomGroup(areaCenter,areaRadius);
            }
        }
    }
    public GameObject center;
    List<GameObject> enemyArray = new List<GameObject>();
    public void DestroyAllEnemy() {
        foreach (Transform item in transform)
        {
            // Destroy(item.gameObject);
            enemyArray.Add(item.gameObject);
            float time = Vector3.Distance(item.transform.position,center.transform.position) * 0.05f;
            StartCoroutine(DestroyEnemy(item.gameObject,time));
        }
    }
    IEnumerator DestroyEnemy(GameObject enemy,float time){
        yield return new WaitForSeconds(time);
        if(enemy!=null && enemy.GetComponent<EnemyController>() != null){
            enemy.GetComponent<EnemyController>().anim.SetTrigger("dead");
            StartCoroutine(SetDestroy(enemy));
        }
    }
    IEnumerator SetDestroy(GameObject gameObject){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    public int GetAllEnemyCount() {
        int i = 0;
        foreach (Transform item in transform)
        {
            i++;
        }
        return i;
    }

    AudioSource audioSource;
    public AudioClip[] audios;
    public void PlayAudio(int index) {
        audioSource.PlayOneShot(audios[index]);
    }
}
