using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Sensitivity : MonoBehaviour
{
	public float Sens = 150f;
	
	public void UpdateSensitivity (float sens)
	{
		Sens = sens;
		GMSPlayer.xMouseSensitivity = Sens;
		GMSPlayer.yMouseSensitivity = Sens;
		Debug.Log("sens = " + sens);
	}
}
