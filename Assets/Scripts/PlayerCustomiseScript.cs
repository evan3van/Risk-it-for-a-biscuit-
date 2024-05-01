using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script to handle player customisation in the main menu and pass this info onto the main scene
/// </summary>
public class PlayerCustomiseScript : MonoBehaviour
{
    /// <summary>
    /// The number of human players
    /// </summary>
    public int numberOfHumanPlayers = 0;

    /// <summary>
    /// The number of AI
    /// </summary>
    public int numberOfAIPlayers = 0;

    /// <summary>
    /// The sliders for setting values
    /// </summary>
    public Slider slider,slider2,slider3;

    /// <summary>
    /// The selected player sprites
    /// </summary>
    public List<Sprite> chosenSprites = new();

    /// <summary>
    /// The set player names
    /// </summary>
    public List<string> playerNames = new();

    /// <summary>
    /// Visual text for the player's customisation options
    /// </summary>
    public TextMeshProUGUI playerNumber,playerName,AIMax;

    /// <summary>
    /// UI elements for selecting things
    /// </summary>
    public GameObject playButton,selectedSkin,nextPlayer;

    /// <summary>
    /// The current player customising
    /// </summary>
    public int playerIndex = 1;

    /// <summary>
    /// The AI's action speed (miliseconds)
    /// </summary>
    public int AISpeed = 500;

    /// <summary>
    /// Sets the max AI number based on the number of players chosen
    /// </summary>
    public void SetMaxAI()
    {
        AIMax.text = ""+(6-numberOfHumanPlayers);
        slider2.maxValue = 6-numberOfHumanPlayers;
    }

    /// <summary>
    /// Changes the number of players based on the slider value
    /// </summary>
    public void ChangeNumberOfPlayers()
    {
        numberOfHumanPlayers = (int)slider.value;
    }

    /// <summary>
    /// Changes the AI speed based on the slider value
    /// </summary>
    public void ChangeAISpeed()
    {
        AISpeed = (100*50) - (int)slider3.value*50;
    }

    /// <summary>
    /// Changes the number of AI players based on the slider value
    /// </summary>
    public void ChangeNumberOfAI()
    {
        numberOfAIPlayers = (int)slider2.value;
    }

    /// <summary>
    /// Adds to the list of chosen player sprites
    /// </summary>
    public void AddPlayerSprite()
    {
        chosenSprites.Add(selectedSkin.GetComponent<SpriteRenderer>().sprite);
    }

    /// <summary>
    /// Adds a player's set name to the list
    /// </summary>
    /// <param name="nameText">The chosen name in the text box</param>
    public void AddPlayerName(TextMeshProUGUI nameText)
    {
        string name = nameText.text;
        if(name == "")
        {
            playerNames.Add("Player "+playerIndex);
        }
        else
        {
            playerNames.Add(name);
        }
    }

    /// <summary>
    /// Sets up the customisation UI for the next player
    /// </summary>
    public void NextPlayer()
    {
        playerIndex++;
        playerName.text = "Player "+playerIndex;
        if (playerIndex == numberOfHumanPlayers)
        {
            playButton.SetActive(true);
            nextPlayer.SetActive(false);
        }
    }

    /// <summary>
    /// Resets the customisation
    /// </summary>
    public void RemoveAllPlayerCustomisation()
    {
        chosenSprites.Clear();
        playerNames.Clear();
        playerIndex = 1;
    }

    /// <summary>
    /// Performs a check for if only 1 player has been chosen
    /// </summary>
    public void CheckIfOnePlayer()
    {
        if(numberOfHumanPlayers == 1)
        {
            playButton.SetActive(true);
            nextPlayer.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the player number UI text
    /// </summary>
    public void SetPlayerName()
    {
        playerNumber.text = "Player "+playerIndex;
    }
}
