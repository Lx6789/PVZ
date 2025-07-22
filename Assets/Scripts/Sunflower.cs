using UnityEngine;
using System.Collections;

public class Sunflower : MonoBehaviour
{
    [Header("Ѫ��")]
    public float HP;
    public float curHP { set; get; }

    [Header("����")]
    public GameObject sunshinePrefab;
    private GameObject curSunshinePrefab;

    [Header("��������")]
    private float interval;
    public Color targetColor = Color.red; // Ŀ����ɫ��Ĭ�ϰ�ɫ������������
    public float duration = 5f;           // ����ʱ�䣨�룩
    private Color startColor;
    public float jumpMinDistance = 0.3f;
    public float jumpMaxDistance = 2f;

    [Header("��ȡ���")]
    public SpriteRenderer spriteRenderer;

    private bool isSuccessPlanting = false;
    private bool isCreated = false;
    private float timer = 0f;

    private void Start()
    {
        startColor = spriteRenderer.color;
        interval = Random.Range(10f, 20f); // ��ʼ�����ʱ��
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

        curSunshinePrefab = Instantiate(sunshinePrefab, transform.position, Quaternion.identity);
        curSunshinePrefab.transform.SetParent(transform); // ��Ϊ������
        float distance = Random.Range(jumpMinDistance, jumpMaxDistance);
        distance = Random.Range(0, 2) < 1 ? -distance : distance;
        Vector3 position = transform.position;
        position.x += distance; 
        curSunshinePrefab.GetComponent<Sunshine>().JumpTop(position);
        
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