using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int debugCounter = 0;
    private int maxArmySize = 40;
    private int totalTerritories = 47;
    public static int numOfPlayers = 3;
    public List<Player> playerList;
    public int turnNumber;
    public List<Continent> continents;
    public List<Territory> allTerritories;
    public List<Color> playerColors;
    public Color neutralColor;
    public List<Counter> counters;
    public GameObject counterPrefab;
    public Turn turn;
    void Start()
    {
        // Iterates through each continent in the continents collection.
        foreach (Continent continent in continents)
        {
            // Within each continent, it iterates through each territory in the continent.
            foreach (Transform territory in continent.transform)
            {
                // Adds a Territory component to each territory game object.
                territory.AddComponent<Territory>();
                
                // Adds the territory to the list of territories in the continent.
                continent.territories.Add(territory.GetComponent<Territory>());
            }
        }

        // If there are more than 6 players, sets the number of players to 6.
        if(numOfPlayers > 6)
        {
            numOfPlayers = 6;
        }

        // Initializes an array of player colors.
        playerColors = new(){
            Color.cyan,
            Color.magenta,
            Color.green,
            Color.yellow,
            Color.gray,
            Color.white
        };

        GameObject turnObject = new("Turn");
        turn = turnObject.AddComponent<Turn>();
        turnNumber = 1;

        InstantiateWorld();

        turn.myTurn = playerList[0];
        turn.nextTurn(playerList[0]);
    }

    private void InstantiateWorld()
    {
        // Method for setting up the world initially.

        // Setting a player count variable to be equal to the number of players public in the editor.
        int playerCount = numOfPlayers;

        // Creating a list of players with player components.
        for (int i = 0; i < playerCount; i++)
        {
            GameObject playerObject = new GameObject("Player "+(i+1));
            Player player = playerObject.AddComponent<Player>();
            player.playerColor = playerColors[i];
            playerList.Add(player);
        }

        // Calculating the max army size starting at 40 and reducing by 5 for each player.
        maxArmySize -= 5*(playerCount-2);

        // Calling the SetTerritories method.
        SetTerritories();

        // Calling the PlaceArmies method.
        PlaceArmies();
    }

    void SetTerritories()
    {
        // This function sets up territories for players to control.

        // Initializes a list to hold references to all territories.
        List<Territory> territoryList = new List<Territory>();

        // Iterates through each continent.
        foreach (Continent continent in continents)
        {
            // Iterates through each territory within the continent.
            foreach (Territory territory in continent.territories)
            {
                // Adds a reference to the territory to the allTerritories list which should not be changed.
                allTerritories.Add(territory);
                
                // Adds a reference to the territory to the territoryList which can be changed.
                territoryList.Add(territory);


            }
        }

        // Determines the number of players.
        int playerCount = playerList.Count;

        // Checks if the number of players is less than or equal to 2.
        if (playerList.Count <= 2){

            // If there are 2 or fewer players, creates a neutral player.
            GameObject neutralPlayer = new GameObject("Neutral");
            Player neutral = neutralPlayer.AddComponent<Player>();
            neutral.playerColor = neutralColor;

            // Adds the neutral player to the player list.
            playerList.Add(neutral);

            // Adjusts the player count to 3.
            playerCount = 3;
        }

        // Calculates the number of territories each player should control.
        int territoriesPerPlayer = totalTerritories/playerCount;

        // Iterates through each player in the player list.
        foreach (Player player in playerList)
        {
            // Distributes territories.
            for (int i = 0; i < territoriesPerPlayer; i++)
            {
                // Selects a random territory from the territory list.
                int randomNumber = Random.Range(0,territoryList.Count);
                Territory choice = territoryList[randomNumber];
                
                // Checks if the chosen territory is available for control.
                if(choice != null && choice.controlledBy == null)
                {
                    // Sets the colors for the territory to the player's color.
                    choice.GetComponent<SpriteRenderer>().color = player.playerColor;

                    OnHoverHighlight highlight = choice.GetComponent<OnHoverHighlight>();
                    highlight.origionalColor = player.playerColor;
                    highlight.player = player;
                    highlight.currentTurn = turn;    //Not setting reference here correctly

                    // Adds the territory to the player's controlled territories.
                    player.controlledTerritories.Add(choice);
                    
                    // Sets the territory's owner to the current player.
                    choice.controlledBy = player;
                    
                    // Removes the chosen territory from the territory list.
                    territoryList.Remove(choice);
                }
            }
        }
        // Assigns remaining territories to the first player in the player list.
        foreach (Territory territory in territoryList.ToArray())
        {
            // Adds the remaining territories to the controlled territories of the first player.
            playerList[0].controlledTerritories.Add(territory);

            // Sets the colors for the territories to the first player's color.
            territory.GetComponent<SpriteRenderer>().color = playerList[0].playerColor;

            OnHoverHighlight highlight = territory.GetComponent<OnHoverHighlight>();
            highlight.origionalColor = playerList[0].playerColor;
            highlight.player = playerList[0];
            highlight.currentTurn = turn;

            // Removes the territory from the territory list.
            territoryList.Remove(territory);
        }

        // Debugging lines:
        // foreach (Player player in playerList){Debug.Log($"Player:{player.name}, Territory count: {player.controlledTerritories.Count}");}   //debugging
    }

    void PlaceArmies()
    {
        // This function is responsible for placing armies (counters) on territories owned by players.

        // Iterates through each player in the player list.
        foreach (Player player in playerList)
        {
            // Retrieves the player's color.
            Color playerColor = player.playerColor;
            
            // Calculates a darker shade of the player's color for the counter.
            Color counterColor = new Color(playerColor.r * 0.8f, playerColor.g * 0.8f, playerColor.b * 0.8f);

            // Iterates through each territory controlled by the player.
            foreach (Territory territory in player.controlledTerritories)
            {
                // Instantiates a counter prefab for the territory.
                GameObject counter = Instantiate(counterPrefab, transform, true);
                
                // Sets the name of the counter.
                counter.name = "Counter: " + territory.name;
                
                // Sets the color of the counter sprite.
                counter.GetComponent<SpriteRenderer>().color = counterColor;
                
                // Sets the position of the counter above the territory.
                counter.transform.position = new Vector3(territory.transform.position.x, territory.transform.position.y, -2);
                
                // Sets the parent of the counter as the territory to keep hierarchy.
                counter.transform.parent = territory.transform;
                
                // Retrieves the Counter script component attached to the counter object.
                Counter counterScript = counter.GetComponent<Counter>();
                
                // Sets the owner of the counter and puts the counter into the player's counters list.
                counterScript.ownedBy = player;
                player.counters.Add(counterScript);
                
                // Adds the counter script to the list of counters.
                counters.Add(counterScript);
            }
        }


        foreach (Player player in playerList)
        {
            player.unitCount = maxArmySize;
            int iterations = maxArmySize;
            foreach (Counter counter in player.counters)
            {
                // Initialise all counters with value 1 by default.
                counter.troopCount = 1;

                // Decrement iterations variable by 1 for each of these.
                iterations -= 1;
            }
            

            while (iterations > 0)
            {
                // Randomly select counter to add 1 to.
                Counter choice = player.counters[Random.Range(0,player.counters.Count)];

                if(choice.troopCount < 5)
                {
                    choice.troopCount += 1;

                    iterations -= 1;
                }
            }

            foreach (Counter counter in player.counters)
            {
                TextMeshPro tmp = counter.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
                tmp.text = counter.troopCount.ToString();
            }
        }
    }
}
