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
    [Tooltip("僵尸数量")]
    public int zombieCount;

    [Tooltip("生成间隔（秒）")]
    public float spawnInterval;

    [Tooltip("僵尸预制体列表")]
    public GameObject[] zombiePrefabs; // 更规范的命名（避免缩写ZM）
}

public class ZMManager : MonoBehaviour // 类名更清晰（避免缩写ZM）
{
    public static ZMManager Instance; // Unity惯例：静态实例用大写的Instance

    [Header("生成点设置")]
    [Tooltip("生成位置列表")]
    public Transform[] spawnPoints; // 命名更简洁（避免List后缀）

    [Header("波次设置")]
    [Tooltip("波次数据")]
    public WaveData[] waves;

    [Tooltip("波次间隔时间（秒）")]
    public float timeBetweenWaves = 30f; // 命名更明确

    [Tooltip("初始等待时间（秒）")]
    public float initialDelay = 5f; // 命名更明确

    private SpawnState spawnState = SpawnState.NotStarted;
    private float timer;
    private bool isStart = false;
    private bool isStop = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 防止重复实例化
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

    public void StartSpawning()
    {
        if (spawnState == SpawnState.Spawning) return;

        spawnState = SpawnState.Spawning;
        StartCoroutine(SpawnZombieWaves());
    }

    private IEnumerator SpawnZombieWaves()
    {
        foreach (var wave in waves) // 使用foreach更简洁
        {
            for (int i = 0; i < wave.zombieCount; i++)
            {
                SpawnRandomZombie(wave);
                yield return new WaitForSeconds(wave.spawnInterval);
            }

            if (wave != waves[^1]) // 如果不是最后一波，等待波次间隔
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        spawnState = SpawnState.Ended;
        Debug.Log("所有波次生成完毕！");
    }

    private void SpawnRandomZombie(WaveData wave)
    {
        if (wave.zombiePrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogError("预制体或生成点未设置！");
            return;
        }

        int randomPointIndex = Random.Range(0, spawnPoints.Length);
        int randomZombieIndex = Random.Range(0, wave.zombiePrefabs.Length);

        Transform spawnPoint = spawnPoints[randomPointIndex];
        GameObject zombiePrefab = wave.zombiePrefabs[randomZombieIndex];

        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        zombie.transform.parent = transform;

        // 设置渲染排序
        var renderer = zombie.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = 100 - randomPointIndex;
        }
    }

    //游戏暂停
    public void ZMStopGame()
    {
        isStop = true;
        
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

        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("ZM"))
            {
                child.gameObject.GetComponent<ZM>().ContinueGame();
            }
        }
    }
}