using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailUI : MonoBehaviour
{

    [Header("组件")]
    [Tooltip("动画")]
    [SerializeField] private Animator animator;
    [Tooltip("UI")]
    public GameObject panel;
    [Tooltip("失败面板")]
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
