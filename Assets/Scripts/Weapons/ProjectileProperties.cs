using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour {

    [SerializeField]
    private float lifetime = 2.0f;           //Time untill bullets clear
    int health;

    // Audio

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

    void Awake () {
        //Consistant Hum SFX
        humAudio = gameObject.AddComponent<AudioSource>();
        humAudio.clip = robotMotorHum;
        humAudio.volume = humVolume;
        humAudio.pitch = humPitch;
        humAudio.spatialBlend = 1;
        humAudio.loop = true;
    }

    private void Start()
    {
        humAudio.Play();
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(200 * Time.deltaTime, 0, 0);

    }

    void OnTriggerEnter(Collider other)
    {

    }
}
