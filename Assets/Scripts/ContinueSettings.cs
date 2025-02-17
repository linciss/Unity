using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueSettings : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource audioSource;
    private float currVolume;

    
    void Awake() {
        audioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        currVolume = PlayerPrefs.GetFloat("volume", audioSource.volume);
        audioSource.volume = currVolume;
    }
}
