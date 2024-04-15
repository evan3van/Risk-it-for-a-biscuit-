using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    /// <summary>
    /// The AudioMixer that controls the game's overall volume.
    /// </summary>
    public AudioMixer audioMixer;

    /// <summary>
    /// Reference to the GameManager to adjust game-specific settings.
    /// </summary>
    public GameManager gameManager;

    /// <summary>
    /// Array of objects that should change theme when Cyberpunk mode is toggled.
    /// </summary>
    public Referencing[] referencingObjects;

    /// <summary>
    /// Toggle for enabling/disabling Cyberpunk mode.
    /// </summary>
    public Toggle cyberpunkToggle;

    void Start()
    {
        

        // Initialize GameManager reference if not set
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager not found in the scene.");
            }
        }
    }

    /// <summary>
    /// Sets the game's volume to a specified level.
    /// </summary>
    /// <param name="volume">The volume level, typically a value between -80 (mute) and 20 (max),
    /// but can be adjusted based on the AudioMixer's settings.</param>
    public void SetVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("Volume", volume);
        }
        else
        {
            Debug.LogError("AudioMixer is not assigned in the inspector.");
        }
    }

    /// <summary>
    /// Sets the number of AI players in the game.
    /// </summary>
    /// <param name="aiPlayers">Number of AI players to set.</param>
    public void SetNumOfAIPlayers(int aiPlayers)
    {
        if (gameManager != null)
        {
            gameManager.numOfAIPlayers = aiPlayers;
        }
        else
        {
            Debug.LogError("GameManager reference not set or found.");
        }
    }
}