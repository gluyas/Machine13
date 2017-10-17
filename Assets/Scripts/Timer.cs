using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public Text time;                       // Visual time in game
    public float timeFloat;                 // Functional time in game
    public Text highscore;                  // Visual highscore in game
    public float highscoreFloat;            // Functional highscore in game

    private void Start()
    {
        highscore.text = "HIGHSCORE\n" + PlayerPrefs.GetFloat("Highscore", timeFloat).ToString("f2");
    }

    public void Update()
    {
        timeFloat += 1 * Time.deltaTime;
        if (time != null) time.text = timeFloat.ToString("f2");
            
        if (timeFloat > PlayerPrefs.GetFloat("Highscore", 0)) {
            PlayerPrefs.SetFloat("Highscore", timeFloat);
        }
        if (Input.GetButtonDown("ResetHighscore"))
        {
            PlayerPrefs.DeleteKey("Highscore");
            PlayerPrefs.SetFloat("Highscore", 0);
            highscore.text = "HIGHSCORE " + PlayerPrefs.GetFloat("Highscore" , timeFloat).ToString("f2");
        }
    }
}
