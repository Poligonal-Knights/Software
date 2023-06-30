// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.Audio;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class VolumeManager : MonoBehaviour
// {
//     public AudioMixer mixer;
//     public Slider slider;
//     public void SetLvl(float sliderVal) 
//     {
//         mixer.SetFloat("MusicVol",Mathf.Log10(sliderVal)*20);
//     }
//
//     public void Awake()
//     {
//         float musicVol = 0;
//         mixer.GetFloat("MusicVol", out musicVol);
//         slider.value = Mathf.Pow(10, musicVol);
//     }
// }
