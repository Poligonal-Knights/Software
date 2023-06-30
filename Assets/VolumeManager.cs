using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public void SetLvl(float sliderVal) 
    {
        mixer.SetFloat("MusicVol",Mathf.Log10(sliderVal)*20);
    }

    public void Awake()
    {
        //Aquí solucionaría el problema del audio, pero el hecho de que sea un log10 lo complica.
    }
}