using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class CharacterManager : MonoBehaviour

{
    public GameObject[] characters; // Assign your characters in the inspector
    private int SelectedSkin = 0;

    private void Start()
    {
        UpdateCharacterSelection();
    }

    public void NextCharacter()
    {
        SelectedSkin++;
        if (SelectedSkin >= characters.Length) SelectedSkin = 0; // Loop back to the first character
        UpdateCharacterSelection();
    }

    public void PreviousCharacter()
    {
        SelectedSkin--;
        if (SelectedSkin < 0) SelectedSkin = characters.Length - 1; // Loop to the last character
        UpdateCharacterSelection();
    }

    void UpdateCharacterSelection()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == SelectedSkin);
        }
    }

    public void SelectCharacterAndLoadScene()
    {
        // Optionally, save the selected character index to PlayerPrefs or another persistent storage solution
        PlayerPrefs.SetInt("SelectedSkin", SelectedSkin);

        // Load the next scene where the game takes place
        SceneManager.LoadScene("MainScene");
    }
}

