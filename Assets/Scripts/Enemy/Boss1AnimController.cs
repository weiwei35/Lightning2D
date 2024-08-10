using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AnimController : MonoBehaviour
{
    public GameObject footRight;
    public GameObject footLeft;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayLeftFoot () {
        var left = Instantiate(footLeft);
        left.transform.position = transform.position + new Vector3(0,-1.7f,0);
        left.SetActive(true);
    }
    public void PlayRightFoot () {
        var right = Instantiate(footRight);
        right.transform.position = transform.position + new Vector3(0,-1.72f,0);
        right.SetActive(true);
    }
}
