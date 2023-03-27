using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer mixer;
    public void SetLvl(float sliderVal) 
    {
        mixer.SetFloat("MusicVol",Mathf.Log10(sliderVal)*20);
    }
}
