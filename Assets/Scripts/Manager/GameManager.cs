using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;

    [Header("引用组件")]
    [Tooltip("开始动画ui")]
    public PrepearUI prepearUI;
    [Tooltip("失败ui")]
    public FailUI failUI;
    [Tooltip("选择卡槽")]
    public GameObject chooseCardPanel;
    [Tooltip("铲子")]
    public GameObject shovel;
    [Tooltip("暂停按钮")]
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
        GameStop();
        failUI.Show();
    }

    public void GameStop()
    {
        chooseCardPanel.GetComponent<ChooseCardPanel>().CardStopGame();
        SunManager.instance.SunStopGame();
        ZMManager.Instance.ZMStopGame();
        BulletManager.instance.BulletStopGame();
    }

    public void GameContinue()
    {
        chooseCardPanel.GetComponent<ChooseCardPanel>().CardContinueGame();
        SunManager.instance.SunContinueGame();
        ZMManager.Instance.ZMContinueGame();
        BulletManager.instance.BulletContinueGame();
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

    // 动态获取组件
    public Component DynamicCallComponent(GameObject obj)
    {
        string componentName = GetPlantName(obj);
        Component component = obj.GetComponent(componentName)
                           ?? obj.GetComponentInChildren(Type.GetType(componentName));

        if (component == null)
            Debug.LogError($"未找到组件: {componentName}", obj);

        return component;
    }

    // 反射调用方法
    public void DynamicCallMethod(Component target, string methodName)
    {
        MethodInfo method = target?.GetType().GetMethod(methodName);
        if (method != null)
            method.Invoke(target, null);
        else
            Debug.LogError($"未找到方法: {methodName}", target?.gameObject);
    }

    // 获取规范化的组件名
    public string GetPlantName(GameObject plant) =>
        plant.name.Replace("(Clone)", "").Trim();
}
