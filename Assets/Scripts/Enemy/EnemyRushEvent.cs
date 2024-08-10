using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyRushEvent : MonoBehaviour
{
    public EnemyRush enemy;
    public void RushToPlayer(){
        enemy.RushToPlayer();
    }
}
