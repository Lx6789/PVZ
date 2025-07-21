using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SunManager : MonoBehaviour
{

    [Header("生产阳光间隔")]
    private float produceTime;
    private float produceTimer;
    [Header("阳光预制体")]
    public GameObject sunPrefab;
    private GameObject curSunPrefab;

    private bool isStartProduce = false;

    private void Start()
    {
        StartProduce();
    }

    private void Update()
    {
        if (!isStartProduce) return;
        ProduceSun();
    }

    public void StartProduce()
    {
        isStartProduce = true;
    }

    private void ProduceSun()
    {
        produceTimer += Time.deltaTime;
        produceTime = Random.Range(30f, 40f);
        if (produceTimer > produceTime)
        {
            produceTimer = 0;
            Vector3 position = new Vector3(Random.Range(-5f, 7.5f), 6.2f, -1f);
            curSunPrefab = Instantiate(sunPrefab, position, Quaternion.identity);
            position.y = Random.Range(3.5f, -4f);
            curSunPrefab.GetComponent<Sunshine>().LinearTo(position);
        }
    }

    
}
