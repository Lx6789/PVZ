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
        Component targetComponent = DynamicCallComponent(plant);
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
        Component targetComponent = DynamicCallComponent(plant);
        DynamicCallMethod(targetComponent, "SeccessPlanting");
        plant.GetComponent<Collider2D>().enabled = true;
    }

    // 动态获取组件
    private Component DynamicCallComponent(GameObject plant)
    {
        string componentName = GetPlantName(plant);
        Component component = plant.GetComponent(componentName)
                           ?? plant.GetComponentInChildren(Type.GetType(componentName));

        if (component == null)
            Debug.LogError($"未找到组件: {componentName}", plant);

        return component;
    }

    // 反射调用方法
    private void DynamicCallMethod(Component target, string methodName)
    {
        MethodInfo method = target?.GetType().GetMethod(methodName);
        if (method != null)
            method.Invoke(target, null);
        else
            Debug.LogError($"未找到方法: {methodName}", target?.gameObject);
    }

    // 获取规范化的组件名
    private string GetPlantName(GameObject plant) =>
        plant.name.Replace("(Clone)", "").Trim();
}