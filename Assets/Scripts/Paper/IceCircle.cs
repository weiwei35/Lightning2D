using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCircle : MonoBehaviour
{
    public Transform center;
    public float speed;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(center.position,center.forward * 0.01f, speed);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 6){
            Debug.Log("寒冰攻击："+other.name);
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.HurtByPaperIce(0.2f,HurtType.PaperIce);
        }
    }
}
