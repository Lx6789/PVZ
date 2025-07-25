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
    private Vector3 sunPositionTextPosition;

    [Header("卡片数")]
    public int cardNumber;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        sunNumberText.text = sunNumber.ToString();
        CalcSunPointTextPosition();
    }

    //修改阳光数
    public void UpdateSunNumber(int sun)
    {
        if (sunNumber == 0 && sun < 0) return;
        sunNumber += sun;
        sunNumberText.text = sunNumber.ToString();
    }

    public Vector3 GetSunPointTextPosition()
    {
        return sunPositionTextPosition;
    }

    private void CalcSunPointTextPosition()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(sunNumberText.transform.position);
        position.x += 1;
        position.z = 0;
        sunPositionTextPosition = position;
    }

    public void CardStopGame()
    {
        Transform childTransform = transform.Find("Cards");
        if (childTransform != null)
        {
            foreach (Transform child in childTransform) 
            {
                child.gameObject.GetComponent<Card>().StopGame();
            }
        }
    }

    public void CardContinueGame()
    {
        Transform childTransform = transform.Find("Cards");
        if (childTransform != null)
        {
            foreach (Transform child in childTransform)
            {
                child.gameObject.GetComponent<Card>().ContinueGame();
            }
        }
    }
}
