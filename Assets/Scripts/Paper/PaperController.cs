using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaperController : MonoBehaviour
{
    [Header("随机范围圆心")]
    public Transform center;
    [Header("随机范围半径")]
    public float radius = 10f;
    [Header("召唤频次")]
    public int count = 10;
    [Header("召唤对象")]
    public GameObject paperPre;
    [HideInInspector]
    public int countCurrent = 0;
    [HideInInspector]
    public bool isAddLight = false;
    // Start is called before the first frame update
    void Start()
    {
        // transform.position = SetRandomPos();
        center = GameObject.FindGameObjectWithTag("Center").transform;
        countCurrent = count;
    }

    // Update is called once per frame
    void Update()
    {
        if(countCurrent >= count){
            countCurrent = 0;
            //生成新的
            var paper = Instantiate(paperPre);
            paper.transform.position = SetRandomPos();
            paper.transform.parent = transform;
        }
    }

    public Vector3 SetRandomPos(){
        if(Global.papersPosList.Count == 0){
            Vector3 pointPos;
            float angle = Random.Range(0,Mathf.PI * 2);
            float radiusRandom = Random.Range(0,radius);
            pointPos = center.position + new Vector3(Mathf.Cos(angle) * radiusRandom,Mathf.Sin(angle) * radiusRandom,0);
            Global.papersPosList.Add(pointPos);
            return pointPos;
        }else{
            Vector3 pointPos;
            
            float minDis;
            do
            {
                List<float> dis = new List<float>();
                float angle = Random.Range(0,Mathf.PI * 2);
                float radiusRandom = Random.Range(0,radius);
                pointPos = center.position + new Vector3(Mathf.Cos(angle) * radiusRandom,Mathf.Sin(angle) * radiusRandom,0);
                foreach (var item in Global.papersPosList)
                {
                    var distance = Vector3.Distance(item,pointPos);
                    dis.Add(distance);
                }
                minDis = dis.Min();
            } while (minDis < 3);
            Global.papersPosList.Add(pointPos);
            return pointPos;
        }
    }

    public void DestroyChild() {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }
}
