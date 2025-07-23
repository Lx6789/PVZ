using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    [Header("车相关")]
    [Tooltip("车速")]
    public float speed;

    private bool isMove =false;

    private void Update()
    {
        if (!isMove) return;
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ZM"))
        {
            Destroy(collision.gameObject);
            isMove = true;
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

}
