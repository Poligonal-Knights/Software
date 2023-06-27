using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangedCheck(); });
    }

    public void ValueChangedCheck()
    {
         Time.timeScale = slider.value;
    }
}
