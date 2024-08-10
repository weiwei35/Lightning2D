using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoorDataSO", menuName = "LightningEvent/DoorData", order = 0)]
public class DoorDataSO : ScriptableObject {
    public List<DoorItem> doorItems = new List<DoorItem>();
    public DoorItem GetDoorItem(int id) {
        foreach (var item in doorItems)
        {
            if(item.id == id){
                return item;
            }
        }
        return null;
    }
}

[System.Serializable]
public class DoorItem{
    public int id;
    public string name;
    public string desc;
}