using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerSound : MonoBehaviour {

    // Audio stuff
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    public AudioClip robotMotorStart;
    private AudioSource initialAudio;
    [Range(0f, 1f)]
    public float startVolume = 1f;
    [Range(0f, 1f)]
    public float startVolumeVariance = 0f;
    [Range(.1f, 3f)]
    public float startPitch = 1f;
    [Range(0f, 1f)]
    public float startPitchVariance = 0f;

    public AudioClip robotMotorHum;
    private AudioSource humAudio;
    [Range(0f, 1f)]
    public float humVolume = 1f;
    [Range(0f, 1f)]
    public float humVolumeVariance = 0f;
    [Range(.1f, 3f)]
    public float humPitch = 1f;
    [Range(0f, 1f)]
    public float humPitchVariance = 0f;

    public AudioClip robotMotorTick;
    private AudioSource TickAudio;
    [Range(0f, 1f)]
    public float tickVolume = 1f;
    [Range(0f, 1f)]
    public float tickVolumeVariance = 0f;
    [Range(.1f, 3f)]
    public float tickPitch = 1f;
    [Range(0f, 1f)]
    public float tickPitchVariance = 0f;
    public float tickPlayDelay = 1f;

    // Settings

    
    //misc variables
    float count = 0;


    private void Awake()
    {
        //Audio Settings
        //Initial Tick Startup SFX
        initialAudio = gameObject.AddComponent<AudioSource>();
        initialAudio.clip = robotMotorStart;
        initialAudio.volume = startVolume * masterVolume;
        initialAudio.pitch = startPitch;
        initialAudio.spatialBlend = 1;
        //Consistant Hum SFX
        humAudio = gameObject.AddComponent<AudioSource>();
        humAudio.clip = robotMotorHum;
        humAudio.volume = humVolume * masterVolume;
        humAudio.pitch = humPitch;
        humAudio.spatialBlend = 1;
        humAudio.loop = true;
        //Consistant Tick SFX
        TickAudio = gameObject.AddComponent<AudioSource>();
        TickAudio.clip = robotMotorTick;
        TickAudio.volume = tickVolume * masterVolume;
        TickAudio.pitch = tickPitch;
        TickAudio.spatialBlend = 1;
        TickAudio.loop = true;
    }

    private void Start()
    {
        // Play Audio
        initialAudio.Play();
        humAudio.Play();
    }


    void Update() {
        if (count >= -1) {
            count += 1f * Time.deltaTime;
            if (count >= tickPlayDelay) {
                TickAudio.Play();
                count = -5;
            }
        }
    }
}
