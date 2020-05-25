using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// This is the GameSettings script, it contains functionality that is specific to the settings of the game
/// </summary>
public class GameSettings : MonoBehaviour
{
    [Header("References")]
    public AudioMixer audioMixer;
    //public Dropdown resolutionDropdown;
    //public Dropdown graphicQualityLevelDropdown;
    //public Toggle fullScreenToggle;
    //public Slider volumeSlider;

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
            UIManager.MyInstance.fullScreenToggle.isOn = true;
        }
        else
        {
            UIManager.MyInstance.fullScreenToggle.isOn = false;
        }
    }

    private void Start()
    {
        // Get volume state
        UIManager.MyInstance.volumeSlider.value = PlayerPrefs.GetFloat("GameVolume", 0f);
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("GameVolume", 0f));

        resolutions = Screen.resolutions;

        UIManager.MyInstance.resolutionDropdown.ClearOptions();

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

        UIManager.MyInstance.resolutionDropdown.AddOptions(options);
        UIManager.MyInstance.resolutionDropdown.value = PlayerPrefs.GetInt("GameResolution", currentResolutionIndex);
        UIManager.MyInstance.resolutionDropdown.RefreshShownValue();

        // Get graphic quality level
        UIManager.MyInstance.graphicQualityLevelDropdown.value = PlayerPrefs.GetInt("GameQualityLevel", QualitySettings.GetQualityLevel());
    }

    /// <summary>
    /// Set fullscreen state
    /// </summary>
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

    /// <summary>
    /// Set resolution state
    /// </summary>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        PlayerPrefs.SetInt("GameResolution", resolutionIndex);
    }

    /// <summary>
    /// Set quality state
    /// </summary>
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GameQualityLevel", qualityIndex);
    }

    /// <summary>
    /// Set volume state
    /// </summary>
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        //audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("GameVolume", volume);
    }
}