using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUIController : MonoBehaviour
{
    public UnityEngine.UI.Image heroImg;
    public TMP_Text heroName;
    public TMP_Text heroDesc;
    public HeroListData hero;
    public void ChooseItem(){
        SelectHeroUI select = GameObject.FindWithTag("HeroPanel").GetComponent<SelectHeroUI>();
        select.SaveHero(hero);
    }
}
