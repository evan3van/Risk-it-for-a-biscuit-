using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using UnityEngine.SceneManagement;

public class SkinManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public List<Sprite> skins = new List<Sprite>();
    private int selectedSkin = 0;
    public GameObject playerSkin;

    // Add public references to your buttons
    public Button nextButton;
    public Button backButton;

    void Start()
    {
        // Subscribe to the button click events
        if (nextButton != null)
            nextButton.onClick.AddListener(NextOption);

        if (backButton != null)
            backButton.onClick.AddListener(BackOption);
    }

    public void NextOption()
    {
        selectedSkin = (selectedSkin + 1) % skins.Count;
        sr.sprite = skins[selectedSkin];
    }

    public void BackOption()
    {
        if (selectedSkin == 0) {
            selectedSkin = skins.Count - 1;
        } else {
            selectedSkin -= 1;
        }
        sr.sprite = skins[selectedSkin];
    }

    public void PlayGame()
    {
        // Assuming your PlayGame method implementation is correct
        // Remember to replace prefab saving logic for runtime builds
        SceneManager.LoadScene("MainScene");
    }
}
