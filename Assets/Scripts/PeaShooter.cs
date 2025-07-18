using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PeaShooter : MonoBehaviour
{
    
    public static PeaShooter instance;

    [Header("豌豆射手")]
    [Tooltip("血量")]
    public int Health;
    [Tooltip("发射时间间隔")]
    public float interval = 1f;

    private float timer = 0f;

    [Header("预制体")]
    public GameObject gamePrefab;

    private GameObject curGamePrefab;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Timer();
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

}
