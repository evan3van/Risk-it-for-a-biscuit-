using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the main menu interactions, providing methods to navigate to different scenes in the game, such as playing the game,
/// accessing the settings menu, returning to the main menu, or quitting the game.
/// </summary>

public class MainMenuScript : MonoBehaviour
{
    public GameObject customisation;
    public GameObject test;

    /// <summary>
    /// Loads the game scene immediately following the current scene in the build settings.
    /// Typically used to start the game from the main menu.
    /// </summary>
    public void PlayGame()
    {
        DontDestroyOnLoad(customisation);
        DontDestroyOnLoad(GameObject.Find("ThemeSwapper"));
        DontDestroyOnLoad(test);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Navigates to the settings menu scene.
    /// </summary>

    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }


    /// <summary>
    /// Navigates to the Player menu scene
    /// selecting number of players determining NO. of AI.
    /// </summary>

    public void GoToPlayerMenu()
    {
        SceneManager.LoadScene("playerMenu");
    }

    /// <summary>
    /// Returns to the main menu scene.
    /// </summary>
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Navigates to the play menu scene.
    /// This can be used for a sub-menu within the game for selecting levels or modes.
    /// </summary>
    
    public void GoToPlayMenu()
    {
        SceneManager.LoadScene("Play Menu");
    } 

    /// <summary>
    /// Quits the game. When running in the Unity editor, this will not do anything.
    /// In a built game, it will close the game application.
    /// </summary>

    public void QuitGame()
    {
        Application.Quit();
    }
}