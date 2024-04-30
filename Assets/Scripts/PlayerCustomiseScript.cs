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
    public int playerIndex = 1;

    public void ChangeNumberOfPlayers()
    {
        numberOfHumanPlayers = (int)slider.value;
    }

    public void AddPlayerSprite(Sprite sprite)
    {
        chosenSprites.Add(sprite);
    }
    public void AddPlayerName(TextMeshProUGUI nameText)
    {
        string name = nameText.text;
        playerNames.Add(name);
    }

    public void RemoveAllPlayerCustomisation()
    {
        chosenSprites.Clear();
        playerNames.Clear();
        playerIndex = 1;
    }
}
