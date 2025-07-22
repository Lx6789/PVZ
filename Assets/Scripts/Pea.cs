using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pea : MonoBehaviour
{

    [Header("子弹相关")]
    [Tooltip("伤害值")]
    public int damageValue;
    [Tooltip("子弹速度")]
    public int speed;

    private void Update()
    {
        BulletsMove();
    }

    //子弹平行移动
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
