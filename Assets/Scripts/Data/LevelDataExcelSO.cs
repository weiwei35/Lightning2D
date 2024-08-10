using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataExcelSO", menuName = "LightningEvent/LevelDataExcel", order = 0)]
public class LevelDataExcelSO : ScriptableObject {
    public List<Level> levels = new List<Level>();
    public Level GetLevelDataById(int id) {
        foreach (var item in levels)
        {
            if(item.levelId == id){
                return item;
            }
        }
        return null;
    }
    public int GetLevelCount(){
        int count = 0;
        List<int> levelIdRecord = new List<int>();
        foreach (var item in levels)
        {
            if(!levelIdRecord.Contains(item.levelId)){
                count++;
                levelIdRecord.Add(item.levelId);
            }
        }
        return count;
    }
}
[System.Serializable]
public class Level{
    public int levelId;
    public float levelTime;
    public EnemyGroup enemyLevel1Group;
    public EnemyGroup enemyLevel2Group;
    public EnemyGroup enemyLevel3Group;
    public EnemyGroup enemyLevel4Group;
    public EnemyGroup enemyLevel5Group;
    public EnemyGroup enemyLevel6Group;
    public EnemyGroup enemyLevel7Group;
    public EnemyGroup enemyLevel8Group;
    public float rewardTime;
    public EnemyGroup rewardGroupId;
    // public LevelType levelType;
    public GameObject boss;
}
[System.Serializable]
public class EnemyGroup{
    // public EnemySet[] enemySets;
    public GameObject type;
    public int count;
}

/*
关卡，固定时长结束，分为“杀敌”“奖励”两部分
1.敌人分3等级，等级1怪物刷新x波刷新一波等级2怪物，等级1怪物刷新y波刷新一波等级3怪物
2.每等级怪包含若干怪物组合，每波刷新时在组合库中随机一组
*/