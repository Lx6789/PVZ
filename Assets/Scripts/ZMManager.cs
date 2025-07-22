using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SpawnState 
{
    NoStart,
    Spawning,
    End
}

public class ZMManager : MonoBehaviour
{
    public static ZMManager instance;

    [Header("����")]
    [Tooltip("λ�ü���")]
    public Transform[] spawnPointList;
    [Tooltip("��ʬԤ���弯��")]
    public GameObject[] zmList;

    [Header("��ʬ�������")]
    [Tooltip("����")]
    public int[] wavesNumber;

    private SpawnState spawnState = SpawnState.NoStart;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartSpawn();
    }

    //���ɽ�ʬ
    public void StartSpawn()
    {
        spawnState = SpawnState.Spawning;
        StartCoroutine(SpawnZM());
    }

    private IEnumerator SpawnZM()
    {
        for (int j = 0; j < wavesNumber.Length; j++)
        {
            //ÿ�����ɽ�ʬ�ĸ���
            for (int i = 0; i < wavesNumber[j]; i++)
            {
                SpawnARandomZM();
                yield return new WaitForSeconds(3);
            }
            yield return new WaitForSeconds(10);
        }
    }

    private void SpawnARandomZM()
    {
        int positionIndex = Random.Range(0, spawnPointList.Length);
        int zmIndex = Random.Range(0, zmList.Length);
        GameObject newZM = Instantiate(zmList[zmIndex], spawnPointList[positionIndex].position, Quaternion.identity);
        // ������Ⱦ���������ɵ�order������ʾ��ǰ�棩
        SpriteRenderer renderer = newZM.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = 100 - positionIndex; // ��̬����orderֵ ����ʹ�þ�̬������renderer.sortingOrder = Time.frameCount % 32767;
        }
    }
}
