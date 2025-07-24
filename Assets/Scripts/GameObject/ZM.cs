using System.Collections;
using UnityEngine;

public class ZM : MonoBehaviour
{
    [Header("�������")]
    [Tooltip("�������")]
    public Rigidbody2D rigidbody2d;
    [Tooltip("�������������")]
    public Animator animator;

    [Header("��ʬ����")]
    [Tooltip("�ƶ��ٶ�")]
    public float moveSpeed = 2f;
    [Tooltip("������")]
    public float attackPower = 10f;
    [Tooltip("�������ֵ")]
    public int maxHealth = 100;
    private int currentHealth;  // ��ǰ����ֵ

    [Header("��������")]
    [Tooltip("����������룩")]
    public float attackInterval = 1f;

    private bool isDead = false;     // �Ƿ�����
    private bool canMove = true;    // �Ƿ�����ƶ�
    private bool isAttacking = false; // �Ƿ����ڹ���
    private bool canAttack = true;
    private GameObject currentTarget; // ��ǰ����Ŀ��
    private Coroutine attackCoroutine; // ����Э��

    private void Start()
    {
        currentHealth = maxHealth;  // ��ʼ������ֵ
    }

    private void Update()
    {
        if (isDead) return;  // ���������ִ���κβ���
        if (!canMove) return;
        Move();  // ��������ƶ���ִ���ƶ�
    }

    // ��ʬ�ƶ�����
    private void Move()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + Vector2.left * moveSpeed * Time.deltaTime);
    }

    // ����������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;  // ��������򲻴���

        if (collision.CompareTag("Plant"))  // �������ֲ��
        {
            if (!canMove) return;
            StartAttack(collision.gameObject);  // ��ʼ����
        }
    }

    // �����˳����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant") && collision.gameObject == currentTarget)
        {
            StopAttack();  // ֹͣ����
        }
    }

    // ��ʼ��������
    private void StartAttack(GameObject target)
    {
        currentTarget = target;      // ���õ�ǰĿ��
        canMove = false;            // ��ֹ�ƶ�
        isAttacking = true;         // ����Ϊ����״̬
        UpdateAnimator(false, true, false);  // ���¶���״̬Ϊ����

        // ���������Թ���Э��
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);  // ������й���Э������ֹͣ
        }
        attackCoroutine = StartCoroutine(AttackRoutine());  // ������Э��
    }

    // ֹͣ��������
    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);  // ֹͣ����Э��
            attackCoroutine = null;
        }

        currentTarget = null;       // ���Ŀ��
        canMove = true;             // �����ƶ�
        isAttacking = false;        // ����Ϊ�ǹ���״̬
        UpdateAnimator(true, false, false);  // ���¶���״̬Ϊ����
    }

    // ����Э��
    private IEnumerator AttackRoutine()
    {
        while (isAttacking && currentTarget != null)  // ������������
        {
            PlantManager.instance.TakeDamage(attackPower, currentTarget, gameObject);  // ��ֲ������˺�
            yield return new WaitForSeconds(attackInterval);  // �ȴ��������
        }
    }

    // �ܵ��˺�����
    public void TakeDamage(int damage)
    {
        if (isDead) return;  // ����������򲻴���

        currentHealth -= damage;  // �۳�����ֵ
        if (currentHealth <= 0)  // �������ֵ<=0
        {
            Die();  // ִ������
        }
    }

    // ��������
    private void Die()
    {
        isDead = true;  // ����Ϊ����״̬
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);  // ֹͣ���й�����Ϊ
        }

        UpdateAnimator(false, false, true);  // ���¶���״̬Ϊ����
        StartCoroutine(DestroyAfterDeathAnimation());  // ������������Э��
    }

    // ������������������Э��
    private IEnumerator DestroyAfterDeathAnimation()
    {
        // �ȴ����������������
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);  // ������Ϸ����
    }

    // ���¶�����������
    private void UpdateAnimator(bool isWalking, bool isEating, bool isDead)
    {
        animator.SetBool("Wark", isWalking);  // ��������״̬
        animator.SetBool("Eat", isEating);    // ���ý�ʳ״̬
        animator.SetBool("Dead", isDead);     // ��������״̬
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