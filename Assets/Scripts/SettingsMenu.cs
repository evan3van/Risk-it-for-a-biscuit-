using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manages audio settings for the game, allowing for volume adjustments through an AudioMixer.
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    /// <summary>
    /// The AudioMixer that controls the game's overall volume.
    /// </summary>
    public AudioMixer audioMixer;

    public GameManager gameManager;

    /// <summary>
    /// Sets the game's volume to a specified level.
    /// </summary>
    /// <param name="volume">The volume level, typically a value between -80 (mute) and 20 (max), 
    /// but can be adjusted based on the AudioMixer's settings.</param>
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    
 public void SetNumOfAIPlayers(int aiPlayers)
    {
        gameManager.numOfAIPlayers = aiPlayers; // Access the numOfAIPlayers variable from the GameManager instance
        // Add option to pick how many!
    }

    private void Start()
    {
        // If you don't have a reference to the GameManager, you can find it in the scene
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }
}
