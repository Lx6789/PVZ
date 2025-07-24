using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButtonUI : MonoBehaviour
{

    [Header("ÔÝÍ£Ãæ°å")]
    public GameObject stopPanel;

    public void OnClick()
    {
        GameManager.Instance.GameContinue();
        stopPanel.SetActive(false);
    }
}
