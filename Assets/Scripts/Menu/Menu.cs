using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour {

    public Canvas MainCanvas;
    public AudioSource Music;
    public float fadein;
    private float volume;
    private float t = 0;
		

	void Awake()

		{
			
		}

    public void Update()
    {
        fadein *= 1.25f;
        t += fadein * Time.deltaTime;
        volume = Mathf.Lerp(0, 1, t);
        Music.volume = volume;
    }

    public void OptionsOn()
	{
		
		MainCanvas.enabled = false;
	}

		public void ReturnOn()
	{
		
		MainCanvas.enabled = true;
	}

		public void LoadOn()
		{
		Application.LoadLevel (1);
		}
}


