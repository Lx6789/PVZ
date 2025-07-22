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

    [Header("引用")]
    [Tooltip("位置集合")]
    public Transform[] spawnPointList;
    [Tooltip("僵尸预制体集合")]
    public GameObject[] zmList;

    [Header("僵尸生成相关")]
    [Tooltip("几波")]
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

    //生成僵尸
    public void StartSpawn()
    {
        spawnState = SpawnState.Spawning;
        StartCoroutine(SpawnZM());
    }

    private IEnumerator SpawnZM()
    {
        for (int j = 0; j < wavesNumber.Length; j++)
        {
            //每波生成僵尸的个数
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
        // 设置渲染排序（先生成的order更大，显示在前面）
        SpriteRenderer renderer = newZM.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = 100 - positionIndex; // 动态调整order值 或者使用静态增量：renderer.sortingOrder = Time.frameCount % 32767;
        }
    }
}
