using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followPlayer;
    // Update is called once per frame
    void Update()
    {
        if(!Global.isGameOver){
            transform.position = followPlayer.transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y,-15);
        }
        
    }
}
