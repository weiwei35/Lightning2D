using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class BugFollowCopy : MonoBehaviour
{
    public float followHurt;
    public Transform target;
    public bool isBack = false;
    public GameObject anim;
    void Update()
    {
        EnemyController[] enemys = Transform.FindObjectsOfType<EnemyController>();
        if(target == null && !isBack){
            int i = -1;
            do
            {
                i++;
                if(i>enemys.Length-1)
                    break;
            } while (enemys[i].isFollowHitting);
            if(i<=enemys.Length-1){
                target = enemys[i].transform;
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
                anim.SetActive(true);
                if(enemys[i].canHurt)
                    enemys[i].HurtByFollow(followHurt,HurtType.BugFollow);
            }
        }else if(target != null && !isBack){
            EnemyController enemy = target.gameObject.GetComponent<EnemyController>();
            if(enemy.HP>0){
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
                anim.SetActive(true);
                if(enemy.canHurt)
                    enemy.HurtByFollow(followHurt,HurtType.BugFollow);
            }else{
                int i = -1;
                do
                {
                    i++;
                    if(i>enemys.Length-1)
                        break;
                } while (enemys[i].isFollowHitting);
                if(i<=enemys.Length-1){
                    target = enemys[i].transform;
                    Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
                    anim.SetActive(true);
                    if(enemys[i].canHurt)
                        enemys[i].HurtByFollow(followHurt,HurtType.BugFollow);
                }
            }
        }else if(isBack){
            anim.SetActive(false);
            transform.position = Vector3.Lerp(transform.position, target.position, 8 * Time.deltaTime);
            if(Vector3.Distance(transform.position, target.position) < 0.5f){
                Destroy(gameObject);
            }
        }
    }
}
