using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("References")]
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Dropdown graphicQualityLevelDropdown;
    public Toggle fullScreenToggle;
    public Slider volumeSlider;

    // Screen resolutions
    Resolution[] resolutions;

    // Variables
    private int getFullScreenInt;

    private void Awake()
    {
        // Get fullscreen state
        getFullScreenInt = PlayerPrefs.GetInt("GameFullScreen", 1);
        if (getFullScreenInt == 1)
        {
            fullScreenToggle.isOn = true;
        }
        else
        {
            fullScreenToggle.isOn = false;
        }

        // Get volume state
        volumeSlider.value = PlayerPrefs.GetFloat("GameVolume", 0f);
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("GameVolume", 0f));
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " : " + resolutions[i].refreshRate + " Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }

            /*if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && resolutions[i].refreshRate == Screen.refreshRate)
            {
                currentResolutionIndex = i;
            }*/
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("GameResolution", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        // Get graphic quality level
        graphicQualityLevelDropdown.value = PlayerPrefs.GetInt("GameQualityLevel", QualitySettings.GetQualityLevel());
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if (isFullscreen == false)
        {
            PlayerPrefs.SetInt("GameFullScreen", 0);
        }
        else
        {
            PlayerPrefs.SetInt("GameFullScreen", 1);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        PlayerPrefs.SetInt("GameResolution", resolutionIndex);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GameQualityLevel", qualityIndex);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        //audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("GameVolume", volume);
    }
}