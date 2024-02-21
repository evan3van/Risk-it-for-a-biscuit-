using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int maxArmySize = 40;
    private int totalTerritories = 47;
    public static int numOfPlayers = 2;
    public List<Player> playerList;
    private TextMesh textMesh;
    public int turnNumber;
    public List<Continent> continents;
    public List<Territory> allTerritories;
    public List<Color> playerColors;
    public Color neutralColor;
    public List<GameObject> counters;
    void Start()
    {

        foreach (Continent continent in continents)
        {
            foreach (Transform territory in continent.transform)
            {
                territory.AddComponent<Territory>();
                continent.territories.Add(territory.GetComponent<Territory>());
            }
        }
        textMesh = GetComponent<TextMesh>();  //Getting the UI text so we can edit it

        if(numOfPlayers > 6){
            numOfPlayers = 6;
        }
        playerColors = new(){
            Color.cyan,
            Color.magenta,
            Color.green,
            Color.yellow,
            Color.gray,
            Color.white
        };
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
            GameObject playerObject = new GameObject("Player "+(i+1));
            Player player = playerObject.AddComponent<Player>();
            player.playerColor = playerColors[i];
            playerList.Add(player);
        }

        maxArmySize -= 5*(playerCount-2);

        //Setting up territories
        SetTerritories();

        PlaceArmies();
    }

    void SetTerritories()
    {
        List<Territory> territoryList = new();
        
        foreach (Continent continent in continents)
        {
            foreach (Territory territory in continent.territories)
            {
                Territory newTerritoryReference = territory;
                allTerritories.Add(territory);
                territoryList.Add(newTerritoryReference);
            }
        }

        int playerCount = playerList.Count;

        if (playerList.Count <= 2){
            GameObject neutralPlayer = new GameObject("Neutral");
            Player neutral = neutralPlayer.AddComponent<Player>();
            neutral.playerColor = neutralColor;
            playerList.Add(neutral);
            playerCount = 3;
        }

        int territoriesPerPlayer = totalTerritories/playerCount;
        int remainderTerritories = totalTerritories%playerCount;
        foreach (Player player in playerList)
        {
            for (int i = 0; i < territoriesPerPlayer; i++)
            {
                int randomNumber = Random.Range(0,territoryList.Count);
                Territory choice = territoryList[randomNumber];
                if(choice != null && choice.controlledBy == null)
                {
                    choice.GetComponent<OnHoverHighlight>().origionalColor = player.playerColor;
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
        if (remainderTerritories != 0)
        {
            for (int i = 0; i < remainderTerritories-1  ; i++)
            {
                territoryList[i].controlledBy = playerList[i];
                playerList[i].controlledTerritories.Add(territoryList[i]);
                territoryList.Remove(territoryList[i]);
            }
        }

        foreach (Player player in playerList){Debug.Log($"Player:{player.name}, Territory count: {player.controlledTerritories.Count}");}   //debugging
    }

    void PlaceArmies()
    {
        foreach (Player player in playerList)
        {
            foreach (Territory territory in player.controlledTerritories)
            {
                
            }
        }

    }
}
