using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resDropdrown;
    // Start is called before the first frame update
    private Resolution[] resolutions;
    private List<Resolution> filteredReslutions;
    private int resIndexToSave;
    private RefreshRate currentRefreshRate;
    private int currentResolutionIndex = 0;
    

    [SerializeField] private Slider soundSlider;
    [SerializeField] private GameObject audioObject;
    [SerializeField] private Toggle fullscreenToggle;
    private int isFullScreen = 1;
    private AudioSource audioSource;
    private float currVolume;
    void Start()
    {
        // audio stuff
        audioSource = audioObject.GetComponent<AudioSource>();
        currVolume = PlayerPrefs.GetFloat("volume", audioSource.volume);
        isFullScreen = PlayerPrefs.GetInt("fullscreen", isFullScreen);
        fullscreenToggle.isOn = isFullScreen == 1;
        Screen.fullScreen = isFullScreen == 1;

        soundSlider.value = currVolume;
        audioSource.volume = currVolume;


        // res stuff 
        resolutions = Screen.resolutions;
        filteredReslutions = new List<Resolution>();

        resDropdrown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.Equals(currentRefreshRate))
            {
                filteredReslutions.Add(resolutions[i]);
            }
        }

        for (int i = 0; i < filteredReslutions.Count; i++)
        {
            string resOption = filteredReslutions[i].width + " x " + filteredReslutions[i].height + " " + filteredReslutions[i].refreshRateRatio + " Hz";
            options.Add(resOption);
            if (filteredReslutions[i].width == Screen.width && filteredReslutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resIndexToSave = PlayerPrefs.GetInt("resolution", currentResolutionIndex);
        resDropdrown.AddOptions(options);
        resDropdrown.value = resIndexToSave;
        resDropdrown.RefreshShownValue();


    }

    public void setResolution(int resolutionIndex)
    {
        resIndexToSave = resolutionIndex;
        Resolution resolution = filteredReslutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void changeVolume()
    {
        currVolume = soundSlider.value;
        audioSource.volume = soundSlider.value;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void save()
    {
        PlayerPrefs.SetInt("resolution", resIndexToSave);
        PlayerPrefs.SetFloat("volume", currVolume);
        PlayerPrefs.SetInt("fullscreen", isFullScreen);
    }

    public void toggleFullscreen()
    {
        if (fullscreenToggle.isOn)
        {
            isFullScreen = 1;
            Screen.fullScreen = true;
        }
        else
        {
            isFullScreen = 0;
            Screen.fullScreen = false;
        }
    }
}
