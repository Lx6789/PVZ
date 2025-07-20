using UnityEngine;
using System.Collections;

public class Sunflower : MonoBehaviour
{
    [Header("血量")]
    public int health;

    [Header("阳光")]
    public GameObject sunshinePrefab;
    private GameObject curSunshinePrefab;

    [Header("生产阳光")]
    private float interval;
    public Color targetColor = Color.red; // 目标颜色（默认白色，代表最亮）
    public float duration = 5f;           // 持续时间（秒）
    private Color startColor;

    [Header("获取组件")]
    public SpriteRenderer spriteRenderer;

    private bool isSuccessPlanting = false;
    private bool isCreated = false;
    private float timer = 0f;

    private void Start()
    {
        startColor = spriteRenderer.color;
        interval = Random.Range(10f, 30f); // 初始化间隔时间
    }

    private void Update()
    {
        if (!isSuccessPlanting && !isCreated) return;
        Timer();
    }

    private void Timer()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            isCreated = false; // 禁止重复生成
            CreateSunshine();
        }
    }

    // 变亮并生产阳光
    private void CreateSunshine()
    {
        StartCoroutine(BrightenAndCreateSunshine());
    }

    private IEnumerator BrightenAndCreateSunshine()
    {
        float elapsedTime = 0f;

        // 颜色渐变到亮色
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            spriteRenderer.color = Color.Lerp(startColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = targetColor;

        // 生成阳光（位置在向日葵上方）
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        curSunshinePrefab = Instantiate(sunshinePrefab, spawnPos, Quaternion.identity);
        curSunshinePrefab.transform.SetParent(transform); // 设为子物体
        
        // 重置状态
        spriteRenderer.color = startColor; // 恢复初始颜色
        interval = Random.Range(10f, 30f); // 计算下一次间隔
        isCreated = true; // 允许下一次生成
    }

    // 确认种植成功
    public void SeccessPlanting()
    {
        isSuccessPlanting = true;
        isCreated = true;
    }
}