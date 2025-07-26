using System.Collections;
using UnityEngine;

public enum SpawnState
{
    NotStarted,
    Spawning,
    Ended
}

[System.Serializable]
public class WaveData
{
    [Tooltip("��ʬ����")]
    public int zombieCount;

    [Tooltip("���ɼ�����룩")]
    public float spawnInterval;

    [Tooltip("��ʬԤ�����б�")]
    public GameObject[] zombiePrefabs; // ���淶��������������дZM��
}

public class ZMManager : MonoBehaviour // ������������������дZM��
{
    public static ZMManager Instance; // Unity��������̬ʵ���ô�д��Instance

    [Header("���ɵ�����")]
    [Tooltip("����λ���б�")]
    public Transform[] spawnPoints; // ��������ࣨ����List��׺��

    [Header("��������")]
    [Tooltip("��������")]
    public WaveData[] waves;

    [Tooltip("���μ��ʱ�䣨�룩")]
    public float timeBetweenWaves = 30f; // ��������ȷ

    [Tooltip("��ʼ�ȴ�ʱ�䣨�룩")]
    public float initialDelay = 5f; // ��������ȷ

    private SpawnState spawnState = SpawnState.NotStarted;
    private float timer;
    private bool isStart = false;
    private bool isStop = false;
    private Coroutine spawnCoroutine; // ����Э������

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ��ֹ�ظ�ʵ����
            return;
        }
        Instance = this;
    }

    private void Update()
    {

        if (isStop) return;
        if (!isStart) return;
        if (spawnState != SpawnState.NotStarted) return;

        timer += Time.deltaTime;
        if (timer >= initialDelay)
        {
            StartSpawning();
        }
    }

    public void StartSpawn()
    {
        isStart = true;
    }

    // �޸�StartSpawning��������Э������
    public void StartSpawning()
    {
        if (spawnState == SpawnState.Spawning) return;

        spawnState = SpawnState.Spawning;
        spawnCoroutine = StartCoroutine(SpawnZombieWaves());
    }

    private IEnumerator SpawnZombieWaves()
    {
        foreach (var wave in waves)
        {
            for (int i = 0; i < wave.zombieCount; i++)
            {
                if (isStop) yield break; // �����Ϸֹͣ��������ֹЭ��

                SpawnRandomZombie(wave);
                yield return new WaitForSeconds(wave.spawnInterval);
            }

            if (wave != waves[^1] && !isStop) // ����Ƿ�ֹͣ
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        if (!isStop) // ֻ��δֹͣʱ�Ÿ���״̬
        {
            spawnState = SpawnState.Ended;
            Debug.Log("���в���������ϣ�");
        }
    }


    private void SpawnRandomZombie(WaveData wave)
    {
        if (wave.zombiePrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogError("Ԥ��������ɵ�δ���ã�");
            return;
        }

        int randomPointIndex = Random.Range(0, spawnPoints.Length);
        int randomZombieIndex = Random.Range(0, wave.zombiePrefabs.Length);

        Transform spawnPoint = spawnPoints[randomPointIndex];
        GameObject zombiePrefab = wave.zombiePrefabs[randomZombieIndex];

        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        zombie.transform.parent = transform;

        // ������Ⱦ����
        var renderer = zombie.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = 100 - randomPointIndex;
        }
    }

    //��Ϸ��ͣ
    //��Ϸ��ͣ
    public void ZMStopGame()
    {
        isStop = true;

        // ֹͣ����Э��
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("ZM"))
            {
                child.gameObject.GetComponent<ZM>().StopGame();
            }
        }
    }


    public void ZMContinueGame()
    {
        isStop = false;

        // ���¿�ʼ����
        if (spawnState == SpawnState.Spawning)
        {
            spawnCoroutine = StartCoroutine(SpawnZombieWaves());
        }

        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("ZM"))
            {
                child.gameObject.GetComponent<ZM>().ContinueGame();
            }
        }
    }
}