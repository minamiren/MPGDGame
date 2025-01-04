using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public BackgroundMusic backgroundMusic;

    public Slider volumeSlider;
    public Slider sensitivitySlider;

    public float defaultVolume = 0.5f;
    public float defaultSensitivity = 0.01f;

    private void Start()
    {
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume", 1);
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.value = savedVolume; // ³]¸m·Æ±ìªì©l­È
            SetVolume(savedVolume);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (sensitivitySlider != null)
        {
            float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 0.5f);
            sensitivitySlider.onValueChanged.RemoveAllListeners();
            sensitivitySlider.value = savedSensitivity;
            SetSensitivity(savedSensitivity);
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        if (backgroundMusic != null)
        {
            backgroundMusic.SetVolume(volume);
        }
    }

    public void SetSensitivity(float sensitivity)
    {
        float lookSpeed = Mathf.Lerp(0.05f, 0.6f, sensitivity);

        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.Save();

        if (playerMovement != null)
        {
            playerMovement.SetSensitivity(lookSpeed);
        }
    }
}
