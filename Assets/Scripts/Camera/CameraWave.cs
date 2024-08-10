using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWave : MonoBehaviour
{
    [SerializeField] private float _shockWaveTime = 0.75f; // 冲击波持续时间
    private Coroutine _shockWaveCoroutine; // 冲击波效果的协程引用

    private Material _material;
    private static int _BoomDistance = Shader.PropertyToID("_BoomDistance"); // 波从中心点的距离的Shader属性ID
    private static int _BoomStartPos = Shader.PropertyToID("_BoomStartPos");

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    // 公共方法，用于触发冲击波效果
    public void CallShockWave(Vector2 pos)
    {
        if (_shockWaveCoroutine != null)
        {
            StopCoroutine(_shockWaveCoroutine); // 如果已有协程在运行，先停止它
        }

        // 将世界坐标转换为视口坐标（UV坐标） 视口坐标是一个单位矩形，左下角是(0,0)，右上角是(1,1)
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(pos);
        Vector2 uvPos = new Vector2(viewportPos.x, viewportPos.y);

        _material.SetVector(_BoomStartPos, uvPos); // 设置位置

        _shockWaveCoroutine = StartCoroutine(ShockWaveAction(-0.1f, 1f)); // 启动新的协程来实现冲击波效果
    }

    // 冲击波效果的协程方法
    private IEnumerator ShockWaveAction(float startPos, float endPos)
    {
        _material.SetFloat(_BoomDistance, startPos); // 设置初始值

        float elapsedTime = 0f; // 初始化经过时间
        while (elapsedTime < _shockWaveTime)
        {
            elapsedTime += Time.deltaTime; // 累加经过时间
            float lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / _shockWaveTime); // 计算插值量
            _material.SetFloat(_BoomDistance, lerpedAmount); // 设置材质属性
            yield return null; // 等待下一帧
        }
        gameObject.SetActive(false);
    }
}
