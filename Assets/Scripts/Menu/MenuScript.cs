using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GMSPlayer PlayerScript;
    
    public enum MenuStates{Main, HighScore}
    public MenuStates CurrentMenuState;

    public GameObject MainMenu;
    public GameObject HighScore;

    void Awake()
    {
        CurrentMenuState = MenuStates.Main; 
        HighScore.SetActive(false);
    }

    void Update()
    {
        switch (CurrentMenuState)
        {
            case MenuStates.Main:
                MainMenu.SetActive(true);
                HighScore.SetActive(false);
                break;
            case MenuStates.HighScore:
                HighScore.SetActive(true);
                MainMenu.SetActive(false);
                break;
        }
    }

    public void OnPlay()
    {
        Debug.Log("Play");  
        SceneManager.LoadScene("MainLevel1");
    }
    
    public void OnHighScore()
    {
        Debug.Log("Options");
        CurrentMenuState = MenuStates.HighScore;
    }
    
    public void OnQuit()
    {
        Debug.Log("Quit");
    }

    public void OnMainMenu()
    {
        Debug.Log("Main Menu");
        CurrentMenuState = MenuStates.Main;
    }
}
