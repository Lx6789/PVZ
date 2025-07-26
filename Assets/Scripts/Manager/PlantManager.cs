using System;
using System.Reflection;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;

    private void Awake() => instance = this;

    // 修改植物血量（通过反射）
    public void TakeDamage(float damage, GameObject plant, GameObject zm)
    {
        Component targetComponent = GameManager.Instance.DynamicCallComponent(plant);
        if (targetComponent == null)
        {
            Debug.LogError("未找到有效组件", plant);
            return;
        }

        // 反射操作
        Type type = targetComponent.GetType();
        PropertyInfo propertyInfo = type.GetProperty("curHP");

        if (propertyInfo == null)
        {
            Debug.LogError($"组件 {type.Name} 上未找到 curHP 属性", plant);
            return;
        }

        if (propertyInfo.PropertyType != typeof(float))
        {
            Debug.LogError("curHP 必须是 float 类型", plant);
            return;
        }

        float currentHP = (float)propertyInfo.GetValue(targetComponent);
        propertyInfo.SetValue(targetComponent, currentHP - damage);
        if (currentHP <= 0)
        {
            zm.GetComponent<ZM>().StopAttack();
            Destroy(plant);
        }
        Debug.Log($"{plant.name} 受到 {damage} 伤害，剩余血量: {currentHP - damage}");
    }

    // 生成植物
    public GameObject CreatePlant(Vector3 position, GameObject prefab)
    {
        GameObject plant = Instantiate(prefab, position, Quaternion.identity);
        plant.GetComponent<Collider2D>().enabled = false;
        return plant;
    }

    // 种植植物
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
            // 获取自身及所有子物体的Animator组件
            Animator[] animators = GetComponentsInChildren<Animator>(true);
            foreach (Animator animator in animators)
            {
                if (animator != null)
                {
                    animator.enabled = false; // 禁用Animator组件
                }
            }
            Component plantComponent = GameManager.Instance.DynamicCallComponent(child.gameObject);
            if (plantComponent != null)
            {
                // 检查是否是 MonoBehaviour（可禁用的脚本）
                MonoBehaviour script = plantComponent as MonoBehaviour;
                if (script != null)
                {
                    script.enabled = false; // 禁用脚本
                    Debug.Log($"已禁用 {child.name} 上的 {script.GetType().Name} 脚本");
                }
                else
                {
                    Debug.LogWarning($"{child.name} 上的 {plantComponent.GetType().Name} 不是可禁用脚本");
                }
            }
            else
            {
                Debug.LogWarning($"在 {child.name} 上未找到目标组件");
            }
        }
    }

    public void PlantContinueGame()
    {
        // 获取自身及所有子物体的Animator组件
        Animator[] animators = GetComponentsInChildren<Animator>(true);
        foreach (Animator animator in animators)
        {
            if (animator != null)
            {
                animator.enabled = true; // 启用Animator组件
            }
        }
        foreach (Transform child in transform)
        {
            Component plantComponent = GameManager.Instance.DynamicCallComponent(child.gameObject);
            if (plantComponent != null)
            {
                // 检查是否是 MonoBehaviour（可禁用的脚本）
                MonoBehaviour script = plantComponent as MonoBehaviour;
                if (script != null)
                {
                    script.enabled = true; // 禁用脚本
                    Debug.Log($"已禁用 {child.name} 上的 {script.GetType().Name} 脚本");
                }
                else
                {
                    Debug.LogWarning($"{child.name} 上的 {plantComponent.GetType().Name} 不是可禁用脚本");
                }
            }
            else
            {
                Debug.LogWarning($"在 {child.name} 上未找到目标组件");
            }
        }
    }
}