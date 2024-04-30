using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCustomiseScript : MonoBehaviour
{
    public int numberOfHumanPlayers = 0;
    public Slider slider;
    public List<Sprite> chosenSprites = new();
    public List<string> playerNames = new();
    public TextMeshProUGUI playerNumber,playerName;
    public GameObject playButton,selectedSkin,nextPlayer;
    public int playerIndex = 1;

    public void ChangeNumberOfPlayers()
    {
        numberOfHumanPlayers = (int)slider.value;
    }

    public void AddPlayerSprite()
    {
        chosenSprites.Add(selectedSkin.GetComponent<SpriteRenderer>().sprite);
    }
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

    public void RemoveAllPlayerCustomisation()
    {
        chosenSprites.Clear();
        playerNames.Clear();
        playerIndex = 1;
    }

    public void CheckIfOnePlayer()
    {
        if(numberOfHumanPlayers == 1)
        {
            playButton.SetActive(true);
            nextPlayer.SetActive(false);
        }
    }

    public void SetPlayerName()
    {
        playerNumber.text = "Player "+playerIndex;
    }
}
