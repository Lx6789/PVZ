using UnityEngine;
using System.Collections;

public class Sunflower : MonoBehaviour
{
    [Header("Ѫ��")]
    public int health;

    [Header("����")]
    public GameObject sunshinePrefab;
    private GameObject curSunshinePrefab;

    [Header("��������")]
    private float interval;
    public Color targetColor = Color.red; // Ŀ����ɫ��Ĭ�ϰ�ɫ������������
    public float duration = 5f;           // ����ʱ�䣨�룩
    private Color startColor;

    [Header("��ȡ���")]
    public SpriteRenderer spriteRenderer;

    private bool isSuccessPlanting = false;
    private bool isCreated = false;
    private float timer = 0f;

    private void Start()
    {
        startColor = spriteRenderer.color;
        interval = Random.Range(10f, 30f); // ��ʼ�����ʱ��
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
            isCreated = false; // ��ֹ�ظ�����
            CreateSunshine();
        }
    }

    // ��������������
    private void CreateSunshine()
    {
        StartCoroutine(BrightenAndCreateSunshine());
    }

    private IEnumerator BrightenAndCreateSunshine()
    {
        float elapsedTime = 0f;

        // ��ɫ���䵽��ɫ
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            spriteRenderer.color = Color.Lerp(startColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = targetColor;

        // �������⣨λ�������տ��Ϸ���
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        curSunshinePrefab = Instantiate(sunshinePrefab, spawnPos, Quaternion.identity);
        curSunshinePrefab.transform.SetParent(transform); // ��Ϊ������
        
        // ����״̬
        spriteRenderer.color = startColor; // �ָ���ʼ��ɫ
        interval = Random.Range(10f, 30f); // ������һ�μ��
        isCreated = true; // ������һ������
    }

    // ȷ����ֲ�ɹ�
    public void SeccessPlanting()
    {
        isSuccessPlanting = true;
        isCreated = true;
    }
}