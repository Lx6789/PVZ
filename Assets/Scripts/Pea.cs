using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pea : MonoBehaviour
{

    [Header("�ӵ����")]
    [Tooltip("�˺�ֵ")]
    public int damageValue;
    [Tooltip("�ӵ��ٶ�")]
    public float speed;

    private void Update()
    {
        BulletsMove();
    }

    //�ӵ�ƽ���ƶ�
    private void BulletsMove()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
