using UnityEngine;
using System.Collections;

public class Sunshine : MonoBehaviour
{
    [Header("������ʧʱ��")]
    public float disappearTime = 10f;
    private float timer = 0;

    [Header("������ֵ")]
    public int sunshineNumber;

    [Header("�ƶ�Ŀ��λ��")]
    public Vector3 position;
    public float moveDuration = 1f; // �ƶ�����ʱ�䣨�룩

    private bool isMoving = false;

    private void Update()
    {
        if (!isMoving)
        {
            Timer();
        }
    }

    private void Timer()
    {
        timer += Time.deltaTime;
        if (timer >= disappearTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (isMoving) return; // ��ֹ�ظ����

        // ��ʼ�ƶ�Э��
        StartCoroutine(MoveToTarget());
    }

    // ����ƽ���ƶ���Ŀ��λ��
    private IEnumerator MoveToTarget()
    {
        isMoving = true;
        Vector3 startPos = position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // �����ֵ������0~1��
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, position, t);

            elapsedTime += Time.deltaTime;
            yield return null; // �ȴ���һ֡
        }

        // ȷ������λ�þ�ȷ
        transform.position = position;

        // ����������ֵ����������
        ChooseCardPanel.instance.UpdateSunNumber(sunshineNumber);
        Destroy(gameObject);
    }
}