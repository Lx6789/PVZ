using System.Collections;
using UnityEngine;

public class ZM : MonoBehaviour
{
    [Header("所需组件")]
    [Tooltip("刚体组件")]
    public Rigidbody2D rigidbody2d;
    [Tooltip("动画控制器组件")]
    public Animator animator;

    [Header("僵尸属性")]
    [Tooltip("移动速度")]
    public float moveSpeed = 2f;
    [Tooltip("攻击力")]
    public float attackPower = 10f;
    [Tooltip("最大生命值")]
    public int maxHealth = 100;
    private int currentHealth;  // 当前生命值

    [Header("攻击设置")]
    [Tooltip("攻击间隔（秒）")]
    public float attackInterval = 1f;

    private bool isDead = false;     // 是否死亡
    private bool canMove = true;    // 是否可以移动
    private bool isAttacking = false; // 是否正在攻击
    private bool canAttack = true;
    private GameObject currentTarget; // 当前攻击目标
    private Coroutine attackCoroutine; // 攻击协程

    private void Start()
    {
        currentHealth = maxHealth;  // 初始化生命值
    }

    private void Update()
    {
        if (isDead) return;  // 如果死亡则不执行任何操作
        if (!canMove) return;
        Move();  // 如果可以移动则执行移动
    }

    // 僵尸移动方法
    private void Move()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + Vector2.left * moveSpeed * Time.deltaTime);
    }

    // 触发进入检测
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;  // 如果死亡则不处理

        if (collision.CompareTag("Plant"))  // 如果碰到植物
        {
            if (!canMove) return;
            StartAttack(collision.gameObject);  // 开始攻击
        }
    }

    // 触发退出检测
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant") && collision.gameObject == currentTarget)
        {
            StopAttack();  // 停止攻击
        }
    }

    // 开始攻击方法
    private void StartAttack(GameObject target)
    {
        currentTarget = target;      // 设置当前目标
        canMove = false;            // 禁止移动
        isAttacking = true;         // 设置为攻击状态
        UpdateAnimator(false, true, false);  // 更新动画状态为攻击

        // 启动周期性攻击协程
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);  // 如果已有攻击协程则先停止
        }
        attackCoroutine = StartCoroutine(AttackRoutine());  // 启动新协程
    }

    // 停止攻击方法
    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);  // 停止攻击协程
            attackCoroutine = null;
        }

        currentTarget = null;       // 清空目标
        canMove = true;             // 允许移动
        isAttacking = false;        // 设置为非攻击状态
        UpdateAnimator(true, false, false);  // 更新动画状态为行走
    }

    // 攻击协程
    private IEnumerator AttackRoutine()
    {
        while (isAttacking && currentTarget != null)  // 持续攻击条件
        {
            PlantManager.instance.TakeDamage(attackPower, currentTarget, gameObject);  // 对植物造成伤害
            yield return new WaitForSeconds(attackInterval);  // 等待攻击间隔
        }
    }

    // 受到伤害方法
    public void TakeDamage(int damage)
    {
        if (isDead) return;  // 如果已死亡则不处理

        currentHealth -= damage;  // 扣除生命值
        if (currentHealth <= 0)  // 如果生命值<=0
        {
            Die();  // 执行死亡
        }
    }

    // 死亡方法
    private void Die()
    {
        isDead = true;  // 设置为死亡状态
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);  // 停止所有攻击行为
        }

        UpdateAnimator(false, false, true);  // 更新动画状态为死亡
        StartCoroutine(DestroyAfterDeathAnimation());  // 启动死亡动画协程
    }

    // 死亡动画后销毁物体协程
    private IEnumerator DestroyAfterDeathAnimation()
    {
        // 等待死亡动画播放完成
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);  // 销毁游戏对象
    }

    // 更新动画参数方法
    private void UpdateAnimator(bool isWalking, bool isEating, bool isDead)
    {
        animator.SetBool("Wark", isWalking);  // 设置行走状态
        animator.SetBool("Eat", isEating);    // 设置进食状态
        animator.SetBool("Dead", isDead);     // 设置死亡状态
    }

    public void StopGame()
    {
        canMove = false;
        animator.speed = 0f;
        canAttack = false;
    }

    public void ContinueGame()
    {
        canMove = true;
        animator.speed = 1f;
        canAttack = true;
    }
}