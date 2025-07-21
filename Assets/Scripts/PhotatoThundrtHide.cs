using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhotatoThundrtHide : MonoBehaviour
{
    [Header("血量")]
    public int health;
    [Header("埋藏时间")]
    public float hideTime;
    public float timer;
    [Header("结束隐埋后的动画")]
    public Animator animator;

    public bool isSuccessPatting = false;
    private bool isHide = true;

    private void Update()
    {
        if (!isSuccessPatting) return;
        Timer();
    }

    private void Timer()
    {
        timer += Time.deltaTime; 
        if (timer >= hideTime)
        {
            isSuccessPatting = false;
            ChangeStatue();
        }
    }

    //切换动画
    private void ChangeStatue()
    {
        animator.SetBool("Isappear", true);
        isHide = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHide) return;
        if (collision.gameObject.CompareTag("ZM"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    //确认种植成功
    public void SeccessPlanting()
    {
        isSuccessPatting = true;
    }


}
