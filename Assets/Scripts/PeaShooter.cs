using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PeaShooter : MonoBehaviour
{
    
    public static PeaShooter instance;

    [Header("�㶹����")]
    [Tooltip("Ѫ��")]
    public int Health;
    [Tooltip("����ʱ����")]
    public float interval = 1f;

    private float timer = 0f;

    [Header("Ԥ����")]
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
        curGamePrefab = Instantiate(gamePrefab);

        // ���ø������λ�ã��뵱ǰ�ű����ڶ�����룩
        curGamePrefab.transform.SetParent(transform);
        // Ӧ�ñ���λ��ƫ�ƣ�����ڸ�����
        curGamePrefab.transform.localPosition = new Vector3(0.4f, 0.2f, 0);
    }

}
