using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioMgr : MonoBehaviour
{
    public static AudioMgr inst;
    public FootStepClass[] footSteps;
    public Sound[] sounds;
    public CharacterController2D CC2D;
    private string previousAmbiance;
    private string nextAmbiance;
    private bool startAmbiance;
    static AudioSource audioFile;
    private float fadeTime = 0.2f;

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        
        foreach (FootStepClass fs in footSteps)
        {
            fs.source = gameObject.AddComponent<AudioSource>();
            fs.source.clip = fs.clip;

            fs.source.volume = fs.volume;
            //fs.source.pitch = fs.pitch;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startAmbiance = true;
        PlayAmbiance("ConnerMusic"); //Main theme or background audio
    }

    //void Update()
    //{
    //    if (audioFile != null)
    //    {
    //        if (!CC2D.m_Grounded)
    //        {
    //            audioFile.Stop();   //This needs to be changed
    //        }
    //    }
    //}
    //// Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }
        s.source.Play();
    }

    public void PlayAmbiance(string name) //update ambiance
    {
        nextAmbiance = name;
        if (startAmbiance == false)
            StartCoroutine(FadeOut());

        StartCoroutine(FadeIn());

        previousAmbiance = name;
        startAmbiance = false;
    }

    public void Playfoot(int name)
    {
        FootStepClass fs = Array.Find(footSteps, sound => sound.name == name);
        if (fs == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }
        audioFile = fs.source;
        fs.source.Play();
    }

    public IEnumerator FadeOut()
    {

        Sound s = Array.Find(sounds, sound => sound.name == previousAmbiance);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
        }

        while (s.volume > 0f)
        {
            s.volume -= fadeTime * Time.deltaTime;
            s.source.volume = s.volume;
            yield return new WaitForSeconds(0.001f);
        }
        s.volume = 0f;
    }

    public IEnumerator FadeIn()
    {
        Sound s = Array.Find(sounds, sound => sound.name == nextAmbiance);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
        }

        s.volume = 0.000f;
        s.source.Play();

        while (s.volume < 1f)
        {
            s.volume += Time.deltaTime * fadeTime;
            s.source.volume = s.volume;
            yield return new WaitForSeconds(0.001f);
        }
        s.volume = 1f;
    }
}
