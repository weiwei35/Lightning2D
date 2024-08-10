using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }
    void Update()
    {
        SetAnimation();
    }

    public void SetAnimation () {
        if(Mathf.Abs(player.moveX)>0.1 || Mathf.Abs(player.moveY)>0.1){
            animator.SetBool("isMoving",true);
        }else{
            animator.SetBool("isMoving",false);
        }
    }
    public void PlayHurtAnim () {
        animator.SetTrigger("hurt");
    }
    public void PlayDeadAnim () {
        animator.StopPlayback();
        animator.Update(0.0f); // 强制更新动画状态机
        animator.SetTrigger("dead");
    }
}
