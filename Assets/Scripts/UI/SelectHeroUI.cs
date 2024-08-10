using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHeroUI : MonoBehaviour
{
    public GameObject heroItem;
    public GameObject heroList;
    public HeroListDataSO heroListData;
    //展示所有英雄
    private void OnEnable() {
        foreach (var hero in heroListData.heros) {
            var heroItemLoad = Instantiate(heroItem);
            HeroUIController heroUI = heroItemLoad.GetComponent<HeroUIController>();
            heroUI.heroImg.sprite = Resources.Load("HeroImg/"+hero.heroImg,typeof(Sprite)) as Sprite;
            heroUI.heroName.text = hero.heroName;
            heroUI.heroDesc.text = hero.heroDesc;
            heroUI.hero = hero;
            heroItemLoad.transform.SetParent(heroList.transform);
            heroItemLoad.transform.localScale = Vector3.one;
        }
    }
    HeroListData saveItem;

    public void SaveHero(HeroListData hero){
        saveItem = hero;
    }
    public SceneController sceneController;

    public void Submit(){
        if(saveItem != null){
            Global.heroId = saveItem.heroId;
            sceneController.StartMainGame();
        }
    }
}
