using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{

    private GameObject dark;
    private GameObject progress;

    [Header("卡片对应的预制体")]
    public GameObject objectPrefab;

    private GameObject curGameObject; //克隆出的实际物体

    [Header("时间计时器")]
    private float timer;

    [Header("冷却")]
    public float waitTime = 10f;
    private bool isWait = true;

    [Header("阳光")]
    public int needSunlightNumber;

    private void Start()
    {
        dark = transform.Find("Dark").gameObject;
        progress = transform.Find("Progress").gameObject;
    }

    private void Update()
    {
        UpdateCardStatue();
    }

    //启动计时器以及更新卡牌状态
    private void UpdateCardStatue()
    {
        Timer();
        UpdateDark();
    }

    //判断卡片是否可用
    private bool IsCardEnabled()
    {
        //冷却时间是否结束，及阳光是否充足
        if (IsWaitOver() && SunlightSufficientQuantity())
        {
            isWait = false;
            return true;
        } 
        else
        {
            isWait = true;
            return false;
        }
    }

    //判断冷却是否结束
    private bool IsWaitOver()
    {
        if (waitTime <= timer) return true;
        return false;
    }

    //判断阳光是否充足
    private bool SunlightSufficientQuantity()
    {
        if (ChooseCardPanel.instance.sunNumber >= needSunlightNumber) return true;
        return false;
    }

    //时间计时器
    private void Timer()
    {
        if (!isWait) return;
        timer += Time.deltaTime;
        UpdateProgress();
    }

    //更新进度条
    private void UpdateProgress()
    {
        float per = Mathf.Clamp(timer / waitTime, 0, 1);
        progress.GetComponent<Image>().fillAmount = 1 - per;
    }

    //禁用dark
    private void UpdateDark()
    {
        if (IsCardEnabled())
        {
            dark.SetActive(false);
        } else
        {
            dark.SetActive(true);
        }
    }

    //拖拽开始（鼠标按下）
    public void OnBeginDrag(BaseEventData data)
    {
        if (!IsCardEnabled()) return;
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject = Instantiate(objectPrefab);
        curGameObject.transform.position = TranslaterScreenToWorld(pointerEventData.position);
    }

    //拖拽移动
    public void OnDrag(BaseEventData data)
    {
        if (curGameObject == null) return;
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject.transform.position = TranslaterScreenToWorld(pointerEventData.position);
    }

    //拖拽结束（鼠标松开）
    public void OnEndDrag(BaseEventData data)
    {
        if (curGameObject == null) return;
        PointerEventData pointerEventData = data as PointerEventData;

        // 拿到鼠标所在位置的碰撞体
        Collider2D[] col = Physics2D.OverlapPointAll(TranslaterScreenToWorld(pointerEventData.position));

        foreach (Collider2D c in col)
        {
            if (c.tag == "Land" && c.transform.childCount == 0)
            {
                // 把当前物体设置为格子的子物体
                curGameObject.transform.parent = c.transform;
                curGameObject.transform.localPosition = Vector3.zero;

                // 动态获取组件并调用方法（组件名 = GameObject 的名字）
                string componentName = curGameObject.name.Replace("(Clone)", ""); // 假设组件名和 GameObject 名相同
                Component targetComponent = curGameObject.GetComponent(componentName);
                //Debug.Log(targetComponent);
                if (targetComponent != null)
                {
                    // 反射调用 "SeccessPlanting" 方法
                    System.Reflection.MethodInfo method = targetComponent.GetType().GetMethod("SeccessPlanting");
                    if (method != null)
                    {
                        method.Invoke(targetComponent, null); // 调用无参方法
                    }
                    else
                    {
                        Debug.LogError($"未找到方法: SeccessPlanting");
                    }
                }
                else
                {
                    Debug.LogError($"未找到组件: {componentName}");
                }

                ChooseCardPanel.instance.UpdateSunNumber(-needSunlightNumber);
                curGameObject = null; // 清空引用
                timer = 0;
                return; // 成功放置后直接返回
            }
        }

        // 如果未放置到合法位置，销毁对象
        GameObject.Destroy(curGameObject);
        curGameObject = null;
    }

    //将鼠标坐标转换为世界坐标
    public static Vector3 TranslaterScreenToWorld(Vector3 position)
    {
        Vector3 cameraTranslantePos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraTranslantePos.x, cameraTranslantePos.y, 0);
    }
}
