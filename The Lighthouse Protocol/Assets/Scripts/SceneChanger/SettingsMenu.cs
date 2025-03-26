using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public float volume;

    public void SetVolume(float volume)
    {
        /*audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20); */
        audioMixer.SetFloat("volume", volume);
        
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    
}
