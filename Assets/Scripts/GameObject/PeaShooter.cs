using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PeaShooter : MonoBehaviour
{
    
    public static PeaShooter instance;

    [Header("�㶹����")]
    [Tooltip("Ѫ��")]
    public float HP;
    public float curHP { set; get; }
    [Tooltip("����ʱ����")]
    public float interval = 1f;

    private float timer = 0f;

    [Header("Ԥ����")]
    public GameObject gamePrefab;

    [Header("�Ƿ���ֲ�ɹ�")]
    private bool isSuccessPatting = false;

    [Header("�������")]
    [Tooltip("������")]
    private float detectDistance;
    [Tooltip("���㼶")]
    public LayerMask targetLayer;

    private bool hasZM = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        curHP = HP;
    }

    private void Update()
    {
        if (!isSuccessPatting) return;
        ChackZM();
        if (!hasZM) return;
        Timer();
    }

    private void ChackZM()
    {
        detectDistance = 9 - transform.position.x;
        // �ӵ�ǰλ�����ҷ�������
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.right,
            detectDistance,
            targetLayer);

        if (hit.collider != null && hit.collider.CompareTag("ZM"))
        {
            Debug.Log("�Ҳ��⵽��ʬ", this);
            // �����ﴦ�����߼�
            hasZM = true;
        } 
        else
        {
            hasZM = false;
        }

        // ���ӻ���������
        Debug.DrawRay(transform.position, Vector2.right * detectDistance, Color.red);
    }

    //�����㶹ʱ�����
    private void Timer()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            CreatePea();
            timer = 0f;
        }
    }

    //�����㶹
    public void CreatePea()
    {
        // ʵ����Ԥ����
        BulletManager.instance.SpawnPea(gamePrefab, transform.position);
    }

    //ȷ����ֲ�ɹ�
    public void SeccessPlanting()
    {
        isSuccessPatting = true;
    }
}
