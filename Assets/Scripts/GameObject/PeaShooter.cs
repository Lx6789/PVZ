using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PeaShooter : MonoBehaviour
{
    
    public static PeaShooter instance;

    [Header("豌豆射手")]
    [Tooltip("血量")]
    public float HP;
    public float curHP { set; get; }
    [Tooltip("发射时间间隔")]
    public float interval = 1f;

    private float timer = 0f;

    [Header("预制体")]
    public GameObject gamePrefab;

    [Header("是否种植成功")]
    private bool isSuccessPatting = false;

    [Header("检测设置")]
    [Tooltip("检测距离")]
    private float detectDistance;
    [Tooltip("检测层级")]
    public LayerMask targetLayer;

    private GameObject curGamePrefab;

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
        // 从当前位置向右发射射线
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.right,
            detectDistance,
            targetLayer);

        if (hit.collider != null && hit.collider.CompareTag("ZM"))
        {
            Debug.Log("右侧检测到僵尸", this);
            // 在这里处理检测逻辑
            hasZM = true;
        } 
        else
        {
            hasZM = false;
        }

        // 可视化调试射线
        Debug.DrawRay(transform.position, Vector2.right * detectDistance, Color.red);
    }

    //发射豌豆时间计算
    private void Timer()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            CreatePea();
            timer = 0f;
        }
    }

    //生成豌豆
    public void CreatePea()
    {
        // 实例化预制体
        curGamePrefab = Instantiate(gamePrefab);

        // 设置父物体和位置（与当前脚本所在对象对齐）
        curGamePrefab.transform.SetParent(transform);
        // 应用本地位置偏移（相对于父对象）
        curGamePrefab.transform.localPosition = new Vector3(0.4f, 0.2f, 0);
    }

    //确认种植成功
    public void SeccessPlanting()
    {
        isSuccessPatting = true;
    }
}
