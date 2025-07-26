using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    public static BulletManager instance;

    private void Awake()
    {
        instance = this;
    }

    //…˙≥…Õ„∂π
    public GameObject SpawnPea(GameObject peaPrefab ,Vector3 position)
    {
        position.x += 0.4f;
        position.y += 0.2f;
        GameObject curGamePrefab = Instantiate(peaPrefab);
        curGamePrefab.transform.position = position;
        curGamePrefab.transform.parent = transform;
        return curGamePrefab;
    }

    public void BulletStopGame()
    {
        foreach (Transform child in transform)
        {
            Component bulletComponent = GameManager.Instance.DynamicCallComponent(child.gameObject);
            GameManager.Instance.DynamicCallMethod(bulletComponent, "StopGame");
        }
    }

    public void BulletContinueGame()
    {
        foreach (Transform child in transform)
        {
            Component bulletComponent = GameManager.Instance.DynamicCallComponent(child.gameObject);
            GameManager.Instance.DynamicCallMethod(bulletComponent, "ContinueGame");
        }
    }
}
