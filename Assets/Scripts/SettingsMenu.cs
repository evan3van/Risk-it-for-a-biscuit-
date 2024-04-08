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

    /// <summary>
    /// Sets the game's volume to a specified level.
    /// </summary>
    /// <param name="volume">The volume level, typically a value between -80 (mute) and 20 (max), 
    /// but can be adjusted based on the AudioMixer's settings.</param>
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
