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
        Component targetComponent = DynamicCallComponent(plant);
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
        Component targetComponent = DynamicCallComponent(plant);
        DynamicCallMethod(targetComponent, "SeccessPlanting");
        plant.GetComponent<Collider2D>().enabled = true;
    }

    // ��̬��ȡ���
    private Component DynamicCallComponent(GameObject plant)
    {
        string componentName = GetPlantName(plant);
        Component component = plant.GetComponent(componentName)
                           ?? plant.GetComponentInChildren(Type.GetType(componentName));

        if (component == null)
            Debug.LogError($"δ�ҵ����: {componentName}", plant);

        return component;
    }

    // ������÷���
    private void DynamicCallMethod(Component target, string methodName)
    {
        MethodInfo method = target?.GetType().GetMethod(methodName);
        if (method != null)
            method.Invoke(target, null);
        else
            Debug.LogError($"δ�ҵ�����: {methodName}", target?.gameObject);
    }

    // ��ȡ�淶���������
    private string GetPlantName(GameObject plant) =>
        plant.name.Replace("(Clone)", "").Trim();
}