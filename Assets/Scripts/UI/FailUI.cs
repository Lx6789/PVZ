using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailUI : MonoBehaviour
{

    [Header("×é¼þ")]
    [Tooltip("¶¯»­")]
    [SerializeField] private Animator animator;
    [Tooltip("UI")]
    public GameObject panel;
    
    private void Hide()
    {
        animator.enabled = false;
    }

    public void Show()
    {
        panel.SetActive(true);
        animator.enabled = true;
    }

}
