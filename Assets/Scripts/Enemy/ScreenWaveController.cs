using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScreenWaveController : MonoBehaviour
{
    public Camera cam;
    private RenderTexture rt;
    public Material material;
    public GameObject waveObject;

    private void Start()
    {
        rt = new RenderTexture(Screen.width, Screen.height, 0);
        cam.targetTexture = rt;
        int shaderid = Shader.PropertyToID("_MainTex");
        material.SetTexture(shaderid, rt);
    }
    public float value = 0; // 要变化的值
    public float target = 1f; // 目标值
    public float lerpSpeed = 1f; // 匀速变化的速度
    bool startWave = false;
    private void Update() {
        if(startWave){
            int shaderid = Shader.PropertyToID("_BoomDistance");
            // 使用Mathf.Lerp从当前值变化到目标值
            value = Mathf.Lerp(value, target, lerpSpeed * Time.deltaTime);
            material.SetFloat(shaderid, value);
    
            // 如果已经非常接近目标值，则可以直接设置为目标值
            if (Mathf.Abs(value - target) < 0.001f)
            {
                value = 0;
                waveObject.SetActive(false);
                startWave = false;
            }
        }
    }
    public void SetWave() {
        waveObject.SetActive(true);
        startWave = true;
    }
}