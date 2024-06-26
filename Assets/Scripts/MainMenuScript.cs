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
    /// <summary>
    /// The customisation gameobject to move to the next scene
    /// </summary>
    public GameObject customise;

    /// <summary>
    /// Loads the game scene immediately following the current scene in the build settings.
    /// Typically used to start the game from the main menu.
    /// </summary>
    public void PlayGame()
    {
        DontDestroyOnLoad(GameObject.Find("ThemeSwapper"));
        DontDestroyOnLoad(customise);
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
    /// Quits the game
    /// </summary>

    public void QuitGame()
    {
        Application.Quit();
    }
}