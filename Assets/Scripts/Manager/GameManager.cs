using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;

    [Header("�������")]
    [Tooltip("��ʼ����ui")]
    public PrepearUI prepearUI;
    [Tooltip("ʧ��ui")]
    public FailUI failUI;
    [Tooltip("ѡ�񿨲�")]
    public GameObject chooseCardPanel;
    [Tooltip("����")]
    public GameObject shovel;
    [Tooltip("��ͣ��ť")]
    public GameObject stopButton;

    private bool isGameEnd = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        Vector3 currentPosition = Camera.main.transform.position;
        Camera.main.transform.DOPath(
            new Vector3[] { currentPosition, new Vector3(6.2f, 0, -10), currentPosition }, 4f, PathType.Linear)
            .OnComplete(ShowReadyUI);
    }

    public void GameEndFail()
    {
        if (isGameEnd) return;
        isGameEnd = true;
        failUI.Show();
    }

    private void ShowReadyUI()
    {
        prepearUI.Show(OnPrepearUIComplete);
    }

    private void OnPrepearUIComplete()
    {
        SunManager.instance.StartProduce();
        ZMManager.Instance.StartSpawn();
        chooseCardPanel.SetActive(true);
        shovel.SetActive(true);
        stopButton.SetActive(true);
    }
}
