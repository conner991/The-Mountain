using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class FootStepClass
{

    public int name;
    //public AudioMixer audioMixer = "main";
    public AudioMixerGroup audioMixer;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;

    //[Range(.1f,3f)]
    //public float pitch;


    [HideInInspector]
    public AudioSource source;
}
