using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [Header("雷电频率")]
    public float lightningTime = 3f;
    public float lightningTimeOriginal = 3f;
    [Header("雷电预告时长")]
    public float lightningPreTime = 3f;
    [Header("雷电点数")]
    public float lightningCount = 3;
    float lightningCountCurrent = 0;
    [Header("雷电伤害")]
    public float lightningHurt = 0;
    [Header("雷电宽度")]
    public float lightningWidth = 0.3f;
    [Header("雷电出现时长")]
    public float startTime = 0.1f;
    [Header("雷电持续时长")]
    public float keepTime = 0.5f;
    [Header("结界伤害")]
    public float circleHurt = 5f;
    [Header("雷劫经验")]
    public float lightningExp = 5f;
    [Header("UI")]
    public InfoUIController uiController;
    [Header("奇门")]
    public CirclePanelController circlePanel;
    float curTime = 0;
    //雷电结束
    public bool isEndLight = false;
    public bool endLight = false;
    public bool endConnect = true;
    //雷电到达player
    public bool isSetLight = false;
    public bool endsetLight = false;
    CircleController circle;
    PlayerController player;
    GameObject petBugs;
    GameObject papers;
    bool showEndLight = false;
    // Start is called before the first frame update
    void Start()
    {
        circle = GetComponent<CircleController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        petBugs = GameObject.FindGameObjectWithTag("PetBugs");
        papers = GameObject.FindGameObjectWithTag("Papers");

        uiController.SetLightningCountText(Mathf.Round(lightningCount * 10) /10);
        uiController.SetLightningHurtText(lightningHurt);
        uiController.SetLightningSpeedText(lightningTime);

        lightningTimeOriginal = lightningTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Global.isChangeLevel){
            curTime = 0;
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Lightning");
            foreach (var obj in taggedObjects)
            {
                Destroy(obj);
            }
            if(!showEndLight){
                circle.CirclePoints();
                PlayAudio(7);
                showEndLight = true;
            }
        }else{
            if(showEndLight){
                showEndLight = false;
            }
        }
        if(!Global.isSlowDown && !Global.isChangeLevel && Global.GameBegain){
            curTime += Time.deltaTime;
        }
        
        if(curTime >= lightningTime && !Global.isChangeLevel){
            curTime = 0;
            float random = (lightningCount - Mathf.Floor(lightningCount))*100;
            int i = Random.Range(0,101);
            if(i < random){
                lightningCountCurrent = Mathf.Floor(lightningCount)+1;
            }else{
                lightningCountCurrent = Mathf.Floor(lightningCount);
            }
            if(circlePanel.inDoor_KAI){
                lightningCountCurrent = lightningCountCurrent+1;
            }

            if(player.isPaperConnect){
                lightningCountCurrent = 1;
            }
            circle.RandomPoints(lightningCountCurrent);
            PlayAudio(5);
            isLight = false;
        }
        
        if(isEndLight && !endLight){
            endLight = true;
            StartCoroutine(EndLight());
        }
        if(player.isLightningMirror)
        {
            if(isSetLight && !endsetLight){
                endsetLight = true;
                StartCoroutine(SetEndLight());
            }
        }
        
    }

    public void HurtEnemy (EnemyController enemy,HurtType type) {
        enemy.Hurt(lightningHurt,type);
    }

    IEnumerator EndLight(){
        isEndLight = false;
        if(circlePanel.inDoor_SHENG){
            player.OutsideRecoveryHP(1);
        }
        if(!Global.isChangeLevel)
            Global.exp += lightningExp;
        if(player.isOnceLightningCopy)
            player.lightningCount ++;
        foreach (Transform item in petBugs.transform)
        {
            BugController bug = item.GetComponent<BugController>();
            if(bug!= null && bug.canRecoverEnergy)
            {
                bug.energyCurrent += (int)lightningCount;
            }
        }
        foreach (Transform item in papers.transform)
        {
            PaperController paper = item.GetComponent<PaperController>();
            if(paper!= null)
            {
                paper.countCurrent ++;
                paper.isAddLight = true;
            }
        }
        yield return new WaitForSeconds(0.5f);
        endLight = false;
    }
    IEnumerator SetEndLight(){
        isSetLight = false;
        circle.RandomPointsMirror(lightningCountCurrent);
        if(player.isMegaCopy){
            circle.RandomPointsCopy(lightningCountCurrent);
        }
        
        yield return new WaitForSeconds(0.5f);
        endsetLight = false;
    }

    public void HurtPlayer () {
        if(player.canHurt){
            player.Hurt(circleHurt);
        }
        if(player.isLightningBoomPlayer){
            player.SetBoom();
            if(player.isMegaCopy){
                foreach(var copy in Global.playerCopyList){
                    player.SetCopyBoom(copy);
                }
            }
        }
    }

    public void ConnectPaper() {
        lightningCount = 1;
    }
    public AudioSource audioSource;
    public AudioClip[] audios;
    public void PlayAudio(int index) {
        audioSource.PlayOneShot(audios[index]);
    }
    bool isLight = false;
    public void PlayLightningAudio() {
        if(!isLight){
            isLight = true;
            audioSource.PlayOneShot(audios[7]);
        }
    }
}
