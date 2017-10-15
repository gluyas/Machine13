using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{

    public enum MenuStates{Main, Options}
    public MenuStates CurrentMenuState;

    public GameObject MainMenu;
    public GameObject OptionsMenu;

    void Awake()
    {
        CurrentMenuState = MenuStates.Main; 
        OptionsMenu.SetActive(false);
    }

    void Update()
    {
        switch (CurrentMenuState)
        {
            case MenuStates.Main:
                MainMenu.SetActive(true);
                OptionsMenu.SetActive(false);
                break;
            case MenuStates.Options:
                OptionsMenu.SetActive(true);
                MainMenu.SetActive(false);
                break;
        }
    }

    public void OnPlay()
    {
        Debug.Log("Play");  
    }
    
    public void OnOptions()
    {
        Debug.Log("Options");
        CurrentMenuState = MenuStates.Options;
    }
    
    public void OnQuit()
    {
        Debug.Log("Quit");
    }

    public void OnSensitivity()
    {
        Debug.Log("Sensitivity");
    }

    public void OnMainMenu()
    {
        Debug.Log("Main Menu");
        CurrentMenuState = MenuStates.Main;
    }
}
