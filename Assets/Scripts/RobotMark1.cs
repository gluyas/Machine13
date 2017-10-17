using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMark1 : MonoBehaviour {

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
    public float speed = 1.0f;
    public float newPositionTime = 5.0f;
    public float xRange = 20;
    public float yRange = 5;
    public float zRange = 20;

    private Vector3 target;
    
    //misc variables
    bool oneCall = true;
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
        target = new Vector3(Random.Range(-xRange, xRange), Random.Range(0.5f, 1), Random.Range(-zRange, zRange));

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

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = new Vector3(Random.Range(-xRange, xRange), Random.Range(0.5f, yRange), Random.Range(-zRange, zRange));
        }
    }
}
