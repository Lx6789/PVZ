using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : MonoBehaviour
{
    [Header("Ѫ��")]
    public int health;
    public Animator animator;
    public float curHelth;
    
    private bool isSuccessPatting = false;

    private void Start()
    {
        curHelth = health;
        UpdateAnimation(false, false);
    }

    private void Update()
    {
        if (!isSuccessPatting) return;
        ChackHealth();
    }

    //�ж�Ѫ�����л�����
    private void ChackHealth()
    {
        if (curHelth <= health / 3.0)
        {
            UpdateAnimation(false, true);
        }
        else if (curHelth <= health / 3.0 * 2)
        {
            UpdateAnimation(true, false);
        } 
        else if(curHelth <= 0)
        {
            Destroy(gameObject);
        }
    }

    //�л�����
    private void UpdateAnimation(bool isheart1, bool isheart2)
    {
        animator.SetBool("Isheart1", isheart1);
        animator.SetBool("Isheart2", isheart2);
    }

    //ȷ����ֲ�ɹ�
    public void SeccessPlanting()
    {
        isSuccessPatting = true;
    }
}
