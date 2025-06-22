using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;    
    public Slider volumeSlider;      
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("volume", 0.75f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20); 
        PlayerPrefs.SetFloat("volume", volume); 
    }
}
