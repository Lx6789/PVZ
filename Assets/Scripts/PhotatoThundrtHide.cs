using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhotatoThundrtHide : MonoBehaviour
{
    [Header("Ѫ��")]
    public int health;
    [Header("���ʱ��")]
    public float hideTime;
    public float timer;
    [Header("���������Ķ���")]
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

    //�л�����
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

    //ȷ����ֲ�ɹ�
    public void SeccessPlanting()
    {
        isSuccessPatting = true;
    }


}
