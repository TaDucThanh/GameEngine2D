using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UpdateVolumeUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        LoadVolume();
        AudioManager.Instance.PlayMusic("MenuBackgroundMusic");
        _musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        _sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    private void OnDisable()
    {
        SaveVolume();
        _musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        _sfxVolumeSlider.onValueChanged.RemoveListener(UpdateSFXVolume);
    }
    #endregion

    #region Volume Methods
    public void UpdateMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        if (_audioMixer.GetFloat("MusicVolume", out float musicVol))
            PlayerPrefs.SetFloat("PlayerPrefsMusicVolume", musicVol);

        if (_audioMixer.GetFloat("SFXVolume", out float sfxVol))
            PlayerPrefs.SetFloat("PlayerPrefsSFXVolume", sfxVol);

        PlayerPrefs.Save();
    }

    public void LoadVolume()
    {
        if (_musicVolumeSlider != null)
            _musicVolumeSlider.value = PlayerPrefs.GetFloat("PlayerPrefsMusicVolume", 0f);

        if (_sfxVolumeSlider != null)
            _sfxVolumeSlider.value = PlayerPrefs.GetFloat("PlayerPrefsSFXVolume", 0f);
    }
    #endregion
}
