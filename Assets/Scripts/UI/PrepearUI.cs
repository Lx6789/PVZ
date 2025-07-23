using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepearUI : MonoBehaviour
{
    [Header("�������")]
    [Tooltip("����״̬��")]
    public Animator animator;

    private Action onComplete;

    private void Start()
    {
        animator.enabled = false;
    }

    public void Show(Action onComplete)
    {
        this.onComplete = onComplete;
        VoiceManager.instance.PlayReadyClip(transform.position);
        animator.enabled = true;
    }

    private void OnShowComplete()
    {
        onComplete?.Invoke();
        gameObject.SetActive(false);
    }

}
