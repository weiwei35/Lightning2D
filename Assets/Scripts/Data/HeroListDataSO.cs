using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroListDataSO", menuName = "LightningEvent/HeroListDataSO", order = 0)]
public class HeroListDataSO : ScriptableObject {
    public List<HeroListData> heros = new List<HeroListData>();
    
}
[System.Serializable]
public class HeroListData{
    public int heroId;
    public string heroImg;
    public string heroName;
    public string heroDesc;
}