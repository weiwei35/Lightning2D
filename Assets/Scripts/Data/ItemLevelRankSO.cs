using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemLevelRankSO", menuName = "LightningEvent/ItemLevelRankSO", order = 0)]
public class ItemLevelRankSO : ScriptableObject {
    public List<ItemLevelRank> itemLevelRanks = new List<ItemLevelRank>();
    public int GetItemSpecType(int id, int type){
        foreach (var item in itemLevelRanks)
        {
            if(item.levelId == id && item.levelType == type){
                int random;
                do
                {
                    random = (int)Random.Range(1,item.item3Rank+item.item2Rank+item.item1Rank+1);
                    if(random<=item.item1Rank){
                        return 1;
                    }else if(random>item.item1Rank && random<=item.item2Rank+item.item1Rank){
                        return 2;
                    }else if(random>item.item2Rank+item.item1Rank && random<=item.item2Rank+item.item1Rank+item.item3Rank){
                        return 3;
                    }
                }while(random>item.item3Rank+item.item2Rank+item.item1Rank);
            }
        }
        return 0;
    }
    public int GetPieceType(int id, int type){
        foreach (var item in itemLevelRanks)
        {
            if(item.levelId == id && item.levelType == type){
                int random;
                do
                {
                    random = (int)Random.Range(1,item.smallRank+item.midRank+item.largeRank+1);
                    if(random<=item.smallRank){
                        return 1;
                    }else if(random>item.smallRank && random<=item.smallRank+item.midRank){
                        return 2;
                    }else if(random>item.smallRank+item.midRank && random<=item.smallRank+item.midRank+item.largeRank){
                        return 3;
                    }
                }while(random>item.smallRank+item.midRank+item.largeRank);
            }
        }
        return 0;
    }
    public int GetTreasureType(int id, int type){
        foreach (var item in itemLevelRanks)
        {
            if(item.levelId == id && item.levelType == type){
                int random;
                do
                {
                    random = (int)Random.Range(1,item.littleRank+item.bigRank+1);
                    if(random<=item.littleRank){
                        return 1;
                    }else if(random>item.littleRank && random<=item.littleRank+item.bigRank){
                        return 2;
                    }
                }while(random>item.littleRank+item.bigRank);
            }
        }
        return 0;
    }
}

[System.Serializable]
public class ItemLevelRank{
    public int levelId;
    public int levelType;
    public float item1Rank;
    public float item2Rank;
    public float item3Rank;
    public float smallRank;
    public float midRank;
    public float largeRank;
    public float littleRank;
    public float bigRank;
}