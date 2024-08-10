using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTurn : MonoBehaviour
{
    public CirclePanelController circlePanel;
    public float speed = 20;
    public bool isClockwise = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Global.GameBegain){
            float rotationThisFrame = speed * Time.deltaTime;
            if(isClockwise)
                transform.Rotate(0, 0,-rotationThisFrame);
            else
            {
                transform.Rotate(0, 0, rotationThisFrame);
            }
        }
    }
}
