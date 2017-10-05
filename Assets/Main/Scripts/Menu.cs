using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour 
	{
		public Canvas MainCanvas;
		

		void Awake()

		{
			
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


