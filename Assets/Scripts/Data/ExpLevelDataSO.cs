using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpLevelDataSO", menuName = "LightningEvent/ExpLevelData", order = 0)]
public class ExpLevelDataSO : ScriptableObject {
    public List<ExpLevel> items = new List<ExpLevel>();

    public float GetExpMaxByLevel(int level) {
        return items[level].expMax;
    }

    public int GetLevelCount(){
        return items.Count;
    }
}
[System.Serializable]
public class ExpLevel{
    public int level;
    public float expMax;
}