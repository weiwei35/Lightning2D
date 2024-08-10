using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelEnemyDataSO", menuName = "LightningEvent/LevelEnemyData", order = 0)]
public class LevelEnemyDataSO : ScriptableObject {
    public List<LevelEnemy> enemys = new List<LevelEnemy>();
}

[System.Serializable]
public class LevelEnemy {
    public string enemyId;
    public int[] levelGroup;
    public int[] rewardLevelGroup;
    public float firstTime;
    public float repeatTime;
    public int[] count;
    public float hp;
    public float hpGrow;
    public float hurt;
    public float hurtGrow;
    public int[] speed;
    public float speedGrow;
    public bool isGroup;
    public int[] groupArea;
    public bool isCircleSide;
}