using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake() => Instance = this;

    public enum Sound
    {
        Sound1
    }
}
