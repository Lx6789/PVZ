using UnityEngine;

public class WallNut : MonoBehaviour
{
    [Header("Ѫ��")]
    public float HP;
    public Animator animator;
    public float curHP { get; set; } 

    private bool isSuccessPatting = false;

    private void Start()
    {
        curHP = HP;
        ResetAnimation(); // ��ʼ������״̬
    }

    private void Update()
    {
        if (!isSuccessPatting) return;
        CheckHealth();
    }

    // ���Ѫ�������¶���
    private void CheckHealth()
    {
        // Ѫ���ֶμ��
        bool isCracked1 = (curHP <= HP * 2f / 3f); // ��һ�׶�����
        bool isCracked2 = (curHP <= HP / 3f);      // �ڶ��׶�����

        UpdateAnimation(isCracked1, isCracked2);
    }

    // ����ΪĬ�϶���״̬
    private void ResetAnimation()
    {
        UpdateAnimation(false, false);
    }

    // ���¶���״̬
    private void UpdateAnimation(bool isCracked1, bool isCracked2)
    {
        animator.SetBool("Isheart1", isCracked1);
        animator.SetBool("Isheart2", isCracked2);
    }

    //ȷ����ֲ�ɹ�
    public void SeccessPlanting()
    {
        isSuccessPatting = true;
    }
}