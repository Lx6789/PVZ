using UnityEngine;

public class WallNut : MonoBehaviour
{
    [Header("血量")]
    public float HP;
    public Animator animator;
    public float curHP { get; set; } 

    private bool isSuccessPatting = false;

    private void Start()
    {
        curHP = HP;
        ResetAnimation(); // 初始化动画状态
    }

    private void Update()
    {
        if (!isSuccessPatting) return;
        CheckHealth();
    }

    // 检测血量并更新动画
    private void CheckHealth()
    {
        // 血量分段检测
        bool isCracked1 = (curHP <= HP * 2f / 3f); // 第一阶段破损
        bool isCracked2 = (curHP <= HP / 3f);      // 第二阶段破损

        UpdateAnimation(isCracked1, isCracked2);
    }

    // 重置为默认动画状态
    private void ResetAnimation()
    {
        UpdateAnimation(false, false);
    }

    // 更新动画状态
    private void UpdateAnimation(bool isCracked1, bool isCracked2)
    {
        animator.SetBool("Isheart1", isCracked1);
        animator.SetBool("Isheart2", isCracked2);
    }

    //确认种植成功
    public void SeccessPlanting()
    {
        isSuccessPatting = true;
    }
}