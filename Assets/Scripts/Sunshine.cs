using UnityEngine;
using System.Collections;

public class Sunshine : MonoBehaviour
{
    [Header("阳光消失时间")]
    public float disappearTime = 10f;
    private float timer = 0;

    [Header("阳光数值")]
    public int sunshineNumber;

    [Header("移动目标位置")]
    public Vector3 position;
    public float moveDuration = 1f; // 移动持续时间（秒）

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
        if (isMoving) return; // 防止重复点击

        // 开始移动协程
        StartCoroutine(MoveToTarget());
    }

    // 阳光平滑移动到目标位置
    private IEnumerator MoveToTarget()
    {
        isMoving = true;
        Vector3 startPos = position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // 计算插值比例（0~1）
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, position, t);

            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        // 确保最终位置精确
        transform.position = position;

        // 更新阳光数值并销毁自身
        ChooseCardPanel.instance.UpdateSunNumber(sunshineNumber);
        Destroy(gameObject);
    }
}