using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Animations;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int startArmySize;
    private int totalTerritories = 47;
    private int numOfPlayers = 2;
    public List<Player> playerList;
    private TextMesh textMesh;
    public int turnNumber;
    public List<Continent> continents;
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
        //Creating a list of players
        for (int i = 0; i < playerCount; i++)
        {
            Player player = new()
            {
                playerNumber = i+1
            };
            playerList.Add(player);
        }

        //Setting up territories
        SetTerritories(playerList);
        if (playerCount == 2)
        {
            SetNeutralTerritory();
        }
    }

    void SetTerritories(List<Player> players)
    {
        List<Territory> territoryList = new();
        
        foreach (Continent continent in continents)
        {
            foreach (Territory territory in continent.territories)
            {
                territoryList.Add(territory);
            }
        }

        int playerCount = players.Count;

        if (players.Count <= 2){
            playerCount = 3;
        }

        int territoriesPerPlayer = totalTerritories/playerCount;
        foreach (Player player in players)
        {
            for (int i = 0; i < territoriesPerPlayer; i++)
            {
                Territory choice = territoryList[Random.Range(0,territoryList.Count)];
                if(choice.controlledBy == null)
                {
                    player.controlledTerritories.Add(choice);
                    choice.controlledBy = player;
                    territoryList.Remove(choice);
                }
                else
                {
                    Debug.Log("Error: trying to select occupied territory");
                }
            }
        }
    }

    void UpdatePossibleTerritories(List<Territory> territories, Territory territory)
    {
        territories.Remove(territory);
    }

    void SetNeutralTerritory()
    {

    }

    void OnTerritoryCapture(Player attacker, Player defender)
    {

    }

    void NextTurn()
    {

    }
}
