using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager instance { get; private set; }

    [Header("��Ƶ")]
    [SerializeField] private AudioClip readyClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip winClip;
    [Header("���")]
    public AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject); // // ����糡������
    }

    public void PlayReadyClip(Vector3 position, float volume = 1f)
    {
        

        if (readyClip == null) return;

        // ����1��ʹ��AudioSource��2Dģʽ��
        audioSource.spatialBlend = 0; // ����Ϊ2D��Ч
        audioSource.PlayOneShot(readyClip);
    }

    public void PlayLoseClip(Vector3 position, float volume = 1f)
    {
        if (loseClip == null) return;

        audioSource.spatialBlend = 0; // ����Ϊ2D��Ч
        audioSource.PlayOneShot(loseClip);
    }

    public void PlayWinClip(Vector3 position, float volume = 1f)
    {
        if (winClip == null) return;

        audioSource.spatialBlend = 0; // ����Ϊ2D��Ч
        audioSource.PlayOneShot(winClip);
    }
}