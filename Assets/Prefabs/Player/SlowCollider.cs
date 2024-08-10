using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowCollider : MonoBehaviour
{
    public float createTime;
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Enemy")
        {
            EnemyController enemy = coll.GetComponent<EnemyController>();
            enemy.debuffSlowing = true;
        }
    }
}
