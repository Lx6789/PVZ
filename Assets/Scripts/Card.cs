using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{

    private GameObject dark;
    private GameObject progress;

    [Header("��Ƭ��Ӧ��Ԥ����")]
    public GameObject objectPrefab;

    private GameObject curGameObject; //��¡����ʵ������

    [Header("ʱ���ʱ��")]
    private float timer;

    [Header("��ȴ")]
    public float waitTime = 10f;
    private bool isWait = true;

    [Header("����")]
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

    //������ʱ���Լ����¿���״̬
    private void UpdateCardStatue()
    {
        Timer();
        UpdateDark();
    }

    //�жϿ�Ƭ�Ƿ����
    private bool IsCardEnabled()
    {
        //��ȴʱ���Ƿ�������������Ƿ����
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

    //�ж���ȴ�Ƿ����
    private bool IsWaitOver()
    {
        if (waitTime <= timer) return true;
        return false;
    }

    //�ж������Ƿ����
    private bool SunlightSufficientQuantity()
    {
        if (ChooseCardPanel.instance.sunNumber >= needSunlightNumber) return true;
        return false;
    }

    //ʱ���ʱ��
    private void Timer()
    {
        if (!isWait) return;
        timer += Time.deltaTime;
        UpdateProgress();
    }

    //���½�����
    private void UpdateProgress()
    {
        float per = Mathf.Clamp(timer / waitTime, 0, 1);
        progress.GetComponent<Image>().fillAmount = 1 - per;
    }

    //����dark
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

    //��ק��ʼ����갴�£�
    public void OnBeginDrag(BaseEventData data)
    {
        if (!IsCardEnabled()) return;
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject = Instantiate(objectPrefab);
        curGameObject.transform.position = TranslaterScreenToWorld(pointerEventData.position);
    }

    //��ק�ƶ�
    public void OnDrag(BaseEventData data)
    {
        if (curGameObject == null) return;
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject.transform.position = TranslaterScreenToWorld(pointerEventData.position);
    }

    //��ק����������ɿ���
    public void OnEndDrag(BaseEventData data)
    {
        if (curGameObject == null) return;
        PointerEventData pointerEventData = data as PointerEventData;

        // �õ��������λ�õ���ײ��
        Collider2D[] col = Physics2D.OverlapPointAll(TranslaterScreenToWorld(pointerEventData.position));

        foreach (Collider2D c in col)
        {
            if (c.tag == "Land" && c.transform.childCount == 0)
            {
                // �ѵ�ǰ��������Ϊ���ӵ�������
                curGameObject.transform.parent = c.transform;
                curGameObject.transform.localPosition = Vector3.zero;

                // ��̬��ȡ��������÷���������� = GameObject �����֣�
                string componentName = curGameObject.name.Replace("(Clone)", ""); // ����������� GameObject ����ͬ
                Component targetComponent = curGameObject.GetComponent(componentName);
                //Debug.Log(targetComponent);
                if (targetComponent != null)
                {
                    // ������� "SeccessPlanting" ����
                    System.Reflection.MethodInfo method = targetComponent.GetType().GetMethod("SeccessPlanting");
                    if (method != null)
                    {
                        method.Invoke(targetComponent, null); // �����޲η���
                    }
                    else
                    {
                        Debug.LogError($"δ�ҵ�����: SeccessPlanting");
                    }
                }
                else
                {
                    Debug.LogError($"δ�ҵ����: {componentName}");
                }

                ChooseCardPanel.instance.UpdateSunNumber(-needSunlightNumber);
                curGameObject = null; // �������
                timer = 0;
                return; // �ɹ����ú�ֱ�ӷ���
            }
        }

        // ���δ���õ��Ϸ�λ�ã����ٶ���
        GameObject.Destroy(curGameObject);
        curGameObject = null;
    }

    //���������ת��Ϊ��������
    public static Vector3 TranslaterScreenToWorld(Vector3 position)
    {
        Vector3 cameraTranslantePos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraTranslantePos.x, cameraTranslantePos.y, 0);
    }
}
