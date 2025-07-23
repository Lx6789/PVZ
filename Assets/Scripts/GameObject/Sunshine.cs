using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Sunshine : MonoBehaviour
{
    [Header("������ʧʱ��")]
    public float disappearTime = 10f;
    private float timer = 0;

    [Header("������ֵ")]
    public int sunshineNumber;

    [Header("�ƶ�����ʱ�䣨�룩")]
    public float moveDuration = 1f;

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
        isMoving = true;
        transform.DOMove(ChooseCardPanel.instance.GetSunPointTextPosition(), moveDuration).OnComplete(
            () => 
            {
                ChooseCardPanel.instance.UpdateSunNumber(sunshineNumber);
                Destroy(this.gameObject);
            }
            );
    }

    
    public void JumpTop(Vector3 targetPos)
    {
        Vector3 centerPos = (transform.position + targetPos) / 2;
        float distance = Vector3.Distance(transform.position, targetPos);
        centerPos.y += (distance / 2);
        transform.DOPath(new Vector3[] { transform.position, centerPos, targetPos }, 
            moveDuration, PathType.CatmullRom).SetEase(Ease.OutQuad);
    }

    public void LinearTo(Vector3 targetPos)
    {
        transform.DOMove(targetPos, moveDuration);
    }
}