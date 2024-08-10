using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroupDataSO", menuName = "LightningEvent/EnemyGroupData", order = 0)]
public class EnemyGroupDataSO : ScriptableObject {
    public List<EnemyLevelGroup> enemyLevelGroups = new List<EnemyLevelGroup>();

    public int GetCountById(int level, int id){
        foreach (var item in enemyLevelGroups)
        {
            if(item.levelId == level){
                switch (id)
                {
                    case 1:
                    return item.count2;
                    case 2:
                    return item.count3;
                    case 3:
                    return item.count4;
                    case 4:
                    return item.count5;
                    case 5:
                    return item.count6;
                    case 6:
                    return item.count7;
                    case 7:
                    return item.count8;
                    default:
                    return -1;
                }
            }
        }
        return -1;
    }
    public int GetHardLevelById(int level){
        foreach (var item in enemyLevelGroups)
        {
            if(item.levelId == level){
                return item.hardLevel;
            }
        }
        return -1;
    }
}
[System.Serializable]
public class EnemyLevelGroup{
    public int levelId;
    public int count2;
    public int count3;
    public int count4;
    public int count5;
    public int count6;
    public int count7;
    public int count8;
    public int hardLevel;
}