using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour
{
    Transform target; // 跟随的Transform
    public float smoothSpeed; // 平滑移动的速度
    public Transform follow;//被跟随的Transform
    [HideInInspector]
    public PlayerController player;
    [HideInInspector]
    public int bugId = 0;
    [Header("充能值")]
    public int energy = 5;
    [HideInInspector]
    public int energyCurrent = 0;
    [HideInInspector]
    public bool canRecoverEnergy = true;
    public bool isTriggered = false;
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    public virtual void Update()
    {
        if(bugId == 0){
            target = player.follow;
        }else{
            target = player.bugs[bugId-1].follow;
        }
        
        // 根据玩家的位置和偏移量来设置当前的位置
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        if((target.position.x - transform.position.x) > 0){
            transform.localScale = new Vector3(-0.4f,0.4f,0.4f);
        }else if((target.position.x - transform.position.x) < 0){
            transform.localScale = new Vector3(0.4f,0.4f,0.4f);
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
