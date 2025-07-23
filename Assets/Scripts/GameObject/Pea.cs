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
    public int speed;

    private void Update()
    {
        BulletsMove();
    }

    //�ӵ�ƽ���ƶ�
    private void BulletsMove()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ZM"))
        {
            collision.GetComponent<ZM>().TakeDamage(damageValue);
            Destroy(gameObject);
        }
    }
}
