using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestryBulletWall : MonoBehaviour
{

    //销毁子弹
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 防御性检查
        if (collision == null || collision.gameObject == null) return;

        // 检查标签
        if (collision.gameObject.tag == "Bullet")
        {
            // 销毁子弹
            Destroy(collision.gameObject);
        }
    }
}
