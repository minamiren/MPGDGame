using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    public float defaultVolume = 0.5f;
    public float defaultSensitivity = 5f;

    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", defaultVolume);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity);
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }

        ApplySettings();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        Debug.Log($"Volume set to: {volume}");
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        Debug.Log($"Sensitivity set to: {sensitivity}");
    }

    public void ApplySettings()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", defaultVolume);
        Debug.Log("Settings Applied");
    }

    public void ResetToDefault()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = defaultVolume;
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = defaultSensitivity;
        }

        SetVolume(defaultVolume);
        SetSensitivity(defaultSensitivity);
    }
}
