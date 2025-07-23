using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager instance { get; private set; }

    [Header("音频")]
    [SerializeField] private AudioClip readyClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip winClip;
    [Header("组件")]
    public AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject); // // 如需跨场景保留
    }

    public void PlayReadyClip(Vector3 position, float volume = 1f)
    {
        

        if (readyClip == null) return;

        // 方法1：使用AudioSource（2D模式）
        audioSource.spatialBlend = 0; // 设置为2D音效
        audioSource.PlayOneShot(readyClip);
    }

    public void PlayLoseClip(Vector3 position, float volume = 1f)
    {
        if (loseClip == null) return;

        audioSource.spatialBlend = 0; // 设置为2D音效
        audioSource.PlayOneShot(loseClip);
    }

    public void PlayWinClip(Vector3 position, float volume = 1f)
    {
        if (winClip == null) return;

        audioSource.spatialBlend = 0; // 设置为2D音效
        audioSource.PlayOneShot(winClip);
    }
}