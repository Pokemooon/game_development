using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    public Slider slider;
    public Toggle toggle;
    public AudioSource BGsound;

    public void ControlAudio()
    {
        if (toggle.isOn)
        {
            BGsound.gameObject.SetActive(true);
            Volume();
        }
        else
        {
            BGsound.gameObject.SetActive(false);
        }
    }

    public void Volume()
    {
        BGsound.volume = slider.value;
        
    }
}