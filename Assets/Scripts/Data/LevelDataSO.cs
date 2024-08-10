using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "LightningEvent/LevelDataSO", order = 0)]
public class LevelDataSO : ScriptableObject {
    public List<LevelData> levels = new List<LevelData>();

    public float GetLevelTimeById(int id) {
        return levels.Find(item => item.id == id && item.type == 0).time;
    }

    public float GetRewardTimeById(int id) {
        return levels.Find(item => item.id == id && item.type == 1).time;
    }

    public LevelData GetLevelById(int id) {
        return levels.Find(item => item.id == id);
    }

    public int GetLevelCount()
    {
        return levels[levels.Count - 1].id;
    }
}

[System.Serializable]
public class LevelData {
    public int id;
    public int type;
    public float time;
}