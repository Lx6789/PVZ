using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButton : MonoBehaviour
{
    [Header("ÔÝÍ£Ãæ°å")]
    public GameObject stopPanel;

    public void OnClick()
    {
        GameManager.Instance.GameStop();
        stopPanel.SetActive(true);
    }
}
