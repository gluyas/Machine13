using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSound : MonoBehaviour {

    // Audio stuff
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    public AudioClip Initialize;
    private AudioSource initialAudio;
    [Range(0f, 1f)]
    public float InitializeVolume = 1f;
    [Range(0f, 1f)]
    public float InitializeVolumeVariance = 0f;
    [Range(.1f, 3f)]
    public float InitializePitch = 1f;
    [Range(0f, 1f)]
    public float InitializePitchVariance = 0f;
    public float minDistance = 20f;
    public float maxDistance = 100f;


    private void Awake()
    {
        //Audio Settings
        //Initial Tick Startup SFX
        initialAudio = gameObject.AddComponent<AudioSource>();
        initialAudio.clip = Initialize;
        initialAudio.volume = InitializeVolume * masterVolume;
        initialAudio.pitch = InitializePitch;
        initialAudio.spatialBlend = 1;
        initialAudio.minDistance = minDistance;
        initialAudio.maxDistance = maxDistance;
        initialAudio.dopplerLevel = 0;
    }

    private void Start()
    {
        // Play Audio
        initialAudio.Play();
    }
}
