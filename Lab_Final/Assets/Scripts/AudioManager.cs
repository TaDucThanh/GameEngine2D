using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Sources
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _loopingSFXSource;
    #endregion

    #region Clips
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] _musicClips;
    [SerializeField] private AudioClip[] _sfxClips;
    #endregion

    #region Public Methods
    public void PlayMusic(string clipName)
    {
        AudioClip clip = FindClip(_musicClips, clipName);
        if (clip == null) return;

        _musicSource.clip = clip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlaySFX(string clipName)
    {
        AudioClip clip = FindClip(_sfxClips, clipName);
        if (clip == null) return;

        _sfxSource.loop = false;
        _sfxSource.PlayOneShot(clip);
    }

    public void PlayLoopingSFX(string clipName)
    {
        AudioClip clip = FindClip(_sfxClips, clipName);
        if (clip == null) return;

        _loopingSFXSource.clip = clip;
        _loopingSFXSource.loop = true;
        _loopingSFXSource.Play();
    }

    public void StopMusic()
    {
        if (_musicSource.isPlaying)
            _musicSource.Stop();
    }

    public void StopLoopingSFX()
    {
        if (_loopingSFXSource.isPlaying)
            _loopingSFXSource.Stop();
    }
    #endregion

    #region Helpers
    private AudioClip FindClip(AudioClip[] clips, string clipName)
    {
        AudioClip clip = System.Array.Find(clips, c => c != null && c.name == clipName);
        if (clip == null)
        {
            Debug.LogWarning($"Clip not found: {clipName}");
        }
        return clip;
    }
    #endregion
}
