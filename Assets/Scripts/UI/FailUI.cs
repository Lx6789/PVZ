using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailUI : MonoBehaviour
{

    [Header("���")]
    [Tooltip("����")]
    [SerializeField] private Animator animator;
    [Tooltip("UI")]
    public GameObject panel;
    [Tooltip("ʧ�����")]
    public GameObject gameOverPanel;
    
    private void Hide()
    {
        panel.SetActive(false);
        gameOverPanel.SetActive(true);
        animator.enabled = false;
    }

    public void Show()
    {
        panel.SetActive(true);
        animator.enabled = true;
    }
}
