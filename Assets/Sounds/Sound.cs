using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;
    public AudioClip clip;

    public AudioMixerGroup mixerGroup;

    [SerializeField]
    [Range(0f,1f)]
    public float volume = 1f;
    [Range(0f, 1f)]
    public float volumeVariance = 0f;
    [Range(.1f, 3f)]
    public float pitch = 1;
    [Range(0f, 1f)]
    public float pitchVariance = 0f;
    [Range(0f, 1f)]
    public float spatialBlend = 1f;


    
    public float minDistance = 1f;
    public float maxDistance = 500f;
    [Range(0f, 360f)]
    public float spread = 5f;

    public bool loop;

    
    [HideInInspector]
    public AudioSource source;

}
