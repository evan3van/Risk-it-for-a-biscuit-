using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the selection of player skins through the UI, allowing the user to browse through available skins and select one.
/// </summary>
public class SkinManager : MonoBehaviour
{
    /// <summary>
    /// The SpriteRenderer that will display the currently selected skin.
    /// </summary>
    public SpriteRenderer sr;

    /// <summary>
    /// A list of available skins for the player.
    /// </summary>
    public List<Sprite> skins = new List<Sprite>();

    /// <summary>
    /// Index of the currently selected skin in the skins list.
    /// </summary>
    private int selectedSkin = 0;

    /// <summary>
    /// The GameObject that represents the player's skin in the scene.
    /// </summary>
    public GameObject playerSkin;
    
    /// <summary>
    /// Button to navigate to the next skin option.
    /// </summary>
    // Add public references to your buttons
    public Button nextButton;

    /// <summary>
    /// Button to navigate to the previous skin option.
    /// </summary>
    public Button backButton;

    /// <summary>
    /// Advances to the next skin option, wrapping around to the first skin if the end of the list is reached.
    /// </summary>
    public void NextOption()
    {
        selectedSkin = (selectedSkin + 1) % skins.Count;
        sr.sprite = skins[selectedSkin];
    }

    /// <summary>
    /// Goes back to the previous skin option, wrapping around to the last skin if currently on the first skin.
    /// </summary>
    public void BackOption()
    {
        if (selectedSkin == 0) {
            selectedSkin = skins.Count - 1;
        } else {
            selectedSkin -= 1;
        }
        sr.sprite = skins[selectedSkin];
    }

    /// <summary>
    /// Loads the main scene to start the game with the currently selected skin.
    /// Note: Ensure any necessary data for the selected skin is properly managed when loading the scene.
    /// </summary>
    public void PlayGame()
    {
        // Assuming your PlayGame method implementation is correct
        // Remember to replace prefab saving logic for runtime builds
        SceneManager.LoadScene("MainScene");
    }
}
