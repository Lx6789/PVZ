using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestryBulletWall : MonoBehaviour
{

    //����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����Լ��
        if (collision == null || collision.gameObject == null) return;

        // ����ǩ
        if (collision.gameObject.tag == "Bullet")
        {
            // �����ӵ�
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Car"))
        {
            Destroy(collision.gameObject);
        }
    }
}
