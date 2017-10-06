using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFOV : MonoBehaviour {

    public Camera playerView;
    public float punchAmount = 8f;
    private float fov;
    private float t = 0f;
    private bool endphase = false;


    private void Start()
    {
        fov = playerView.fieldOfView;
        punchAmount = punchAmount + fov;
    }
    public void fovPunch(float upwardSpeed, float downwardSpeed, float punchTime)   {
        if (t < punchTime / 2 && !endphase)
        {
            Debug.Log("Upward");
            playerView.fieldOfView = Mathf.Lerp(fov, punchAmount, t);
            t += upwardSpeed * Time.deltaTime;
        }
        if (t >= punchTime / 2 || endphase) {
            if (!endphase)
            {
                t = 0;
            }
            endphase = true;
            Debug.Log("Downward");
            playerView.fieldOfView = Mathf.Lerp(punchAmount, fov, t);
            t += downwardSpeed * Time.deltaTime;
        }
        if (t >= punchTime / 2 && endphase)
            return;
    }
}
