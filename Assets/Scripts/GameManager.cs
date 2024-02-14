using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int numOfPlayers = 2;
    public List<Player> playerList;
    private TextMesh textMesh;
    public int turnNumber;
    void Start()
    {
        textMesh = GetComponent<TextMesh>();  //Getting the UI text so we can edit it
        InstantiateWorld(numOfPlayers);
    }

    void FixedUpdate()
    {
        //Any checks for updates on things in the world will happen here
    }

//method for instantiating the world initially taking into account the number of players
    private void InstantiateWorld(int playerCount)
    {
        
        for (int i = 0; i < playerCount; i++)
        {
            Player player = new()
            {
                playerNumber = i
            };
            playerList.Add(player);
        }

        //Creating neutral territory for 2 players
        if (playerCount == 2)
        {
            SetNeutralTerritory();
        }
    }

    void SetNeutralTerritory()
    {

    }

    void OnTerritoryCapture(Player attacker, Player defender)
    {

    }

    void NextTurn(Player prevPlayer)
    {

    }
}
