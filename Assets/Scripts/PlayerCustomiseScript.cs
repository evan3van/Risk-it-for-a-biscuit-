using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCustomiseScript : MonoBehaviour
{
    public int numberOfHumanPlayers = 0;
    public int numberOfAIPlayers = 0;
    public Slider slider,slider2,slider3;
    public List<Sprite> chosenSprites = new();
    public List<string> playerNames = new();
    public TextMeshProUGUI playerNumber,playerName,AIMax;
    public GameObject playButton,selectedSkin,nextPlayer;
    public int playerIndex = 1;
    public int AISpeed;
    public void SetMaxAI()
    {
        AIMax.text = ""+(6-numberOfHumanPlayers);
        slider2.maxValue = 6-numberOfHumanPlayers;
    }

    public void ChangeNumberOfPlayers()
    {
        numberOfHumanPlayers = (int)slider.value;
    }
    public void ChangeAISpeed()
    {
        AISpeed = (int)slider3.value*100;
    }
    public void ChangeNumberOfAI()
    {
        numberOfAIPlayers = (int)slider2.value;
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
