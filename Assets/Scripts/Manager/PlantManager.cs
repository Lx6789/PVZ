using System;
using System.Reflection;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;

    private void Awake() => instance = this;

    // �޸�ֲ��Ѫ����ͨ�����䣩
    public void TakeDamage(float damage, GameObject plant, GameObject zm)
    {
        Component targetComponent = GameManager.Instance.DynamicCallComponent(plant);
        if (targetComponent == null)
        {
            Debug.LogError("δ�ҵ���Ч���", plant);
            return;
        }

        // �������
        Type type = targetComponent.GetType();
        PropertyInfo propertyInfo = type.GetProperty("curHP");

        if (propertyInfo == null)
        {
            Debug.LogError($"��� {type.Name} ��δ�ҵ� curHP ����", plant);
            return;
        }

        if (propertyInfo.PropertyType != typeof(float))
        {
            Debug.LogError("curHP ������ float ����", plant);
            return;
        }

        float currentHP = (float)propertyInfo.GetValue(targetComponent);
        propertyInfo.SetValue(targetComponent, currentHP - damage);
        if (currentHP <= 0)
        {
            zm.GetComponent<ZM>().StopAttack();
            Destroy(plant);
        }
        Debug.Log($"{plant.name} �ܵ� {damage} �˺���ʣ��Ѫ��: {currentHP - damage}");
    }

    // ����ֲ��
    public GameObject CreatePlant(Vector3 position, GameObject prefab)
    {
        GameObject plant = Instantiate(prefab, position, Quaternion.identity);
        plant.GetComponent<Collider2D>().enabled = false;
        return plant;
    }

    // ��ֲֲ��
    public void GrowPlants(GameObject plant)
    {
        plant.transform.parent = transform;
        Component targetComponent = GameManager.Instance.DynamicCallComponent(plant);
        GameManager.Instance.DynamicCallMethod(targetComponent, "SeccessPlanting");
        plant.GetComponent<Collider2D>().enabled = true;
    }

    public void PlantStopGame()
    {
        foreach (Transform child in transform)
        {
            // ��ȡ���������������Animator���
            Animator[] animators = GetComponentsInChildren<Animator>(true);
            foreach (Animator animator in animators)
            {
                if (animator != null)
                {
                    animator.enabled = false; // ����Animator���
                }
            }
            Component plantComponent = GameManager.Instance.DynamicCallComponent(child.gameObject);
            if (plantComponent != null)
            {
                // ����Ƿ��� MonoBehaviour���ɽ��õĽű���
                MonoBehaviour script = plantComponent as MonoBehaviour;
                if (script != null)
                {
                    script.enabled = false; // ���ýű�
                    Debug.Log($"�ѽ��� {child.name} �ϵ� {script.GetType().Name} �ű�");
                }
                else
                {
                    Debug.LogWarning($"{child.name} �ϵ� {plantComponent.GetType().Name} ���ǿɽ��ýű�");
                }
            }
            else
            {
                Debug.LogWarning($"�� {child.name} ��δ�ҵ�Ŀ�����");
            }
        }
    }

    public void PlantContinueGame()
    {
        // ��ȡ���������������Animator���
        Animator[] animators = GetComponentsInChildren<Animator>(true);
        foreach (Animator animator in animators)
        {
            if (animator != null)
            {
                animator.enabled = true; // ����Animator���
            }
        }
        foreach (Transform child in transform)
        {
            Component plantComponent = GameManager.Instance.DynamicCallComponent(child.gameObject);
            if (plantComponent != null)
            {
                // ����Ƿ��� MonoBehaviour���ɽ��õĽű���
                MonoBehaviour script = plantComponent as MonoBehaviour;
                if (script != null)
                {
                    script.enabled = true; // ���ýű�
                    Debug.Log($"�ѽ��� {child.name} �ϵ� {script.GetType().Name} �ű�");
                }
                else
                {
                    Debug.LogWarning($"{child.name} �ϵ� {plantComponent.GetType().Name} ���ǿɽ��ýű�");
                }
            }
            else
            {
                Debug.LogWarning($"�� {child.name} ��δ�ҵ�Ŀ�����");
            }
        }
    }
}