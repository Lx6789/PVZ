using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCardPanel : MonoBehaviour
{

    public static ChooseCardPanel instance;

    [Header("阳光")]
    public Text sunNumberText;
    public int sunNumber = 50;

    [Header("卡片数")]
    public int cardNumber;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        sunNumberText.text = sunNumber.ToString();
    }

    //修改阳光数
    public void UpdateSunNumber(int sun)
    {
        if (sunNumber == 0 && sun < 0) return;
        sunNumber += sun;
        sunNumberText.text = sunNumber.ToString();
    }
}
