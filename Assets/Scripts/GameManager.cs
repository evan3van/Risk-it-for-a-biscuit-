using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// Manages game states, player turns, and global game settings.
/// It initializes the game world, including territories, continents, and players, and manages the turn-based logic.
/// </summary>
public class GameManager : MonoBehaviour
{
    private int maxArmySize = 40;
    private int totalTerritories = 47;

    /// <summary>
    /// Number of players participating in the game.
    /// </summary>
    public int numOfPlayers = 3;

    public int numOfAIPlayers;

    /// <summary>
    /// List of all player objects participating in the game.
    /// </summary>
    public List<Player> playerList;

    /// <summary>
    /// Current turn number in the game.
    /// </summary>
    public int turnNumber;

    /// <summary>
    /// List of all continents in the game.
    /// </summary>
    public List<Continent> continents;

    /// <summary>
    /// List of all territories in the game.
    /// </summary>
    public List<Territory> allTerritories;

    /// <summary>
    /// List of colors assigned to players.
    /// </summary>
    public List<Color> playerColors;

    /// <summary>
    /// Color assigned to neutral territories or units.
    /// </summary>
    public Color neutralColor;

    /// <summary>
    /// List of counter objects used to represent armies on the map.
    /// </summary>
    public List<Counter> counters;

    /// <summary>
    /// Prefab used to instantiate new counter objects.
    /// </summary>
    public GameObject counterPrefab;

    /// <summary>
    /// Prefab used to instantiate arrow objects.
    /// </summary>
    public GameObject arrowPrefab;

    /// <summary>
    /// Reference to the current turn manager.
    /// </summary>
    public Turn turn;

    /// <summary>
    /// GameObject representing the player UI name display.
    /// </summary>
    public GameObject playerUIName;

    /// <summary>
    /// Dictionary mapping territory names to their neighbouring territories' names.
    /// </summary>
    public Dictionary<string,List<string>> territoriesNeighbours;

    /// <summary>
    /// List of sprites representing players.
    /// </summary>
    public List<Sprite> playerSprites;

    public ThemeSwapper themeSwapper;

    public GameObject map,winMenu,gameCanvas;

    public Sprite cyberMap;
    public bool canUpdate = false;
    void Awake()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        Debug.Log("Screen Width: " + screenWidth);
        Debug.Log("Screen Height: " + screenHeight);

        gameObject.SetActive(true);
        gameCanvas.SetActive(true);
        if(GameObject.Find("ThemeSwapper") != null ){
            themeSwapper = GameObject.Find("ThemeSwapper").GetComponent<ThemeSwapper>();
            if(themeSwapper.Theme == "Cyber"){
                map.GetComponent<SpriteRenderer>().sprite = themeSwapper.cyberBackground;
            }
            playerSprites = themeSwapper.playerSprites;
        }
        // Iterates through each continent in the continents collection.
        foreach (Continent continent in continents)
        {
            // Within each continent, it iterates through each territory in the continent.
            foreach (Transform territoryObject in continent.transform)
            {
                // Adds a Territory component to each territory game object.
                Territory territory = territoryObject.AddComponent<Territory>();
                
                // Adds the territory to the list of territories in the continent.
                continent.territories.Add(territory);
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

        turn = GameObject.Find("Turn").GetComponent<Turn>();
        turnNumber = 1;

        InstantiateWorld();

        turn.players = playerList;
        turn.playerUIName = playerUIName;
        turn.PlayPhase();

        foreach (Territory territory in allTerritories)
        {
            territory.turn = turn;
            territory.arrowPrefab = arrowPrefab;
        }
        canUpdate = true;
    }
    private void Update() 
    {
        if (canUpdate){
            foreach (Player player in playerList)
            {
                if (player.unitCount == 0)
                {
                    EliminatePlayer(player);
                    Debug.Log($"Player: {player}, has been eliminated");
                }
            }
            if (playerList.Count <= 1)
            {
                gameCanvas.SetActive(false);
                gameObject.SetActive(false);
                winMenu.SetActive(true);
            }
        }
    }
     
    /// <summary>
    /// Prepares the game world, setting up players, territories, and initial army placement.
    /// </summary> 
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
             if (i >= numOfPlayers - numOfAIPlayers)
             {
                 player.IsAI = true;
                 playerObject.AddComponent<AIBehavior>();
             }
            player.sprite = playerSprites[i];
            playerList.Add(player);
        }

        // Calculating the max army size starting at 40 and reducing by 5 for each player.
        maxArmySize -= 5*(playerCount-2);

        // Calling the SetTerritories method.
        SetTerritories();

        // Calling the PlaceArmies method.
        PlaceArmies();

        // Set the neighbouring territories for each territory.
        SetNeighbours();
    }

    /// <summary>
    /// Assigns territories to players, distributing them evenly and setting initial control.
    /// </summary>
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

            territory.controlledBy = playerList[0];

            // Removes the territory from the territory list.
            territoryList.Remove(territory);
        }

        // Debugging lines:
        // foreach (Player player in playerList){Debug.Log($"Player:{player.name}, Territory count: {player.controlledTerritories.Count}");}   //debugging
    }

    /// <summary>
    /// Places initial armies on territories controlled by players.
    /// </summary>
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
                counter.transform.position = new Vector3(territory.transform.position.x, territory.transform.position.y, -3);
                
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
                tmp.overrideColorTags = true;
                tmp.color = Color.black;
            }
        }
    }

    /// <summary>
    /// Displays a list of all territories in the game for debugging purposes.
    /// </summary>
    public void displayAllTerritories(){
        string territories = "";
        foreach (Territory territory in allTerritories)
        {
            territories = territories + ", "+ territory.name;

        }
        Debug.Log(territories);
        Debug.Log(allTerritories.Count);
    }

    /// <summary>
    /// Sets up neighboring relationships between territories, enabling gameplay mechanics like attacks.
    /// </summary>
    public void SetNeighbours()
    {
        // Creates a dictionary that maps all the neighbours to each other
        territoriesNeighbours = new Dictionary<string, List<string>>
        {
            {"North Africa", new List<string> {"Egypt", "Central Africa", "East Africa", "Western Europe", "Southern Europe"}},
            {"Egypt", new List<string> {"North Africa", "East Africa", "Middle East", "Southern Europe"}},
            {"Central Africa", new List<string> {"North Africa", "East Africa", "South Africa","Egypt"}},
            {"East Africa", new List<string> {"Egypt", "North Africa", "Central Africa", "South Africa", "Madagascar", "Middle East"}},
            {"Madagascar", new List<string> {"East Africa", "South Africa"}},
            {"South Africa", new List<string> {"Central Africa", "East Africa", "Madagascar"}},
            {"Cuba", new List<string> {"Eastern United States", "Central America"}},
            {"Greenland", new List<string> {"Quebec", "Ontario", "North West Territory", "Iceland"}},
            {"Eastern United States", new List<string> {"Central America", "Western United States", "Ontario", "Quebec", "Cuba"}},
            {"Central America", new List<string> {"Eastern United States", "Western United States", "Venezuela", "Cuba"}},
            {"Quebec", new List<string> {"Eastern United States", "Ontario", "Greenland"}},
            {"Western United States", new List<string> {"Eastern United States", "Central America", "Alberta", "Ontario"}},
            {"Ontario", new List<string> {"Quebec", "Eastern United States", "Western United States", "Alberta", "North West Territory", "Greenland"}},
            {"Alberta", new List<string> {"Western United States", "Ontario", "North West Territory", "Alaska"}},
            {"North West Territory", new List<string> {"Alberta", "Ontario", "Greenland", "Alaska"}},
            {"Alaska", new List<string> {"North West Territory", "Alberta", "Kamchatka"}},
            {"New Zealand", new List<string> {"Eastern Australia"}},
            {"Indonesia", new List<string> {"Siam", "New Guinea", "Western Australia"}},
            {"New Guinea", new List<string> {"Indonesia", "Eastern Australia", "Western Australia"}},
            {"Western Australia", new List<string> {"Indonesia", "Eastern Australia", "New Guinea"}},
            {"Eastern Australia", new List<string> {"New Guinea", "Western Australia", "New Zealand"}},
            {"Ukraine", new List<string> {"Ural", "Afghanistan", "Middle East", "Southern Europe", "Northern Europe", "Scandinavia"}},
            {"Scandinavia", new List<string> {"Iceland", "Great Britain", "Northern Europe", "Ukraine"}},
            {"Southern Europe", new List<string> {"North Africa", "Egypt", "Middle East", "Western Europe", "Northern Europe", "Ukraine"}},
            {"Western Europe", new List<string> {"Great Britain", "Northern Europe", "Southern Europe", "North Africa"}},
            {"Northern Europe", new List<string> {"Great Britain", "Scandinavia", "Ukraine", "Southern Europe", "Western Europe"}},
            {"Great Britain", new List<string> {"Iceland", "Scandinavia", "Northern Europe", "Western Europe"}},
            {"Iceland", new List<string> {"Greenland", "Scandinavia", "Great Britain"}},
            {"Japan", new List<string> {"Kamchatka", "Mongolia"}},
            {"Mongolia", new List<string> {"Japan", "China", "Siberia", "Irkutsk", "Kamchatka"}},
            {"Irkutsk", new List<string> {"Siberia", "Yakutsk", "Kamchatka", "Mongolia"}},
            {"Kamchatka", new List<string> {"Alaska", "Yakutsk", "Irkutsk", "Mongolia", "Japan"}},
            {"Yakutsk", new List<string> {"Siberia", "Irkutsk", "Kamchatka"}},
            {"Siberia", new List<string> {"Ural", "China", "Mongolia", "Irkutsk", "Yakutsk"}},
            {"China", new List<string> {"Siam", "India", "Afghanistan", "Ural", "Siberia", "Mongolia"}},
            {"Siam", new List<string> {"India", "China", "Indonesia"}},
            {"India", new List<string> {"Middle East", "Afghanistan", "China", "Siam"}},
            {"Afghanistan", new List<string> {"Ukraine", "Ural", "China", "India", "Middle East"}},
            {"Ural", new List<string> {"Ukraine", "Siberia", "China", "Afghanistan"}},
            {"Middle East", new List<string> {"Egypt", "East Africa", "Southern Europe", "Ukraine", "Afghanistan", "India"}},
            {"Argentina", new List<string> {"Brazil", "Peru"}},
            {"Brazil", new List<string> {"Venezuela", "Peru", "Argentina", "North Africa"}},
            {"South America Island 2", new List<string> {"South America Island 1","South Africa"}},
            {"South America Island 1", new List<string> {"Argentina","Brazil","South America Island 2"}},
            {"Peru", new List<string> {"Venezuela", "Brazil", "Argentina"}},
            {"Venezuela", new List<string> {"Central America", "Brazil", "Peru"}},
            {"Philipines", new List<string> {"Siam","Western Australia","Indonesia"}}
        };

        Dictionary<Territory, List<Territory>> neighbours = new Dictionary<Territory, List<Territory>>();
        // Iterate through the original dictionary and populate the new dictionary with Territory script references.
        foreach (KeyValuePair<string, List<string>> entry in territoriesNeighbours)
        {
            // Find the GameObject with the same name as the territory string key.
            GameObject territoryObject = GameObject.Find(entry.Key);
    
            // Ensure the GameObject exists.
            if (territoryObject != null)
            {
                // Get the Territory script attached to the GameObject.
                Territory territoryScript = territoryObject.GetComponent<Territory>();
        
                // Check if the Territory script is found.
                if (territoryScript != null)
                {
                    // Create a list to hold references to neighboring territories.
                    List<Territory> neighbors = new List<Territory>();
            
                    // Iterate through the list of neighboring territory strings.
                    foreach (string neighborName in entry.Value)
                    {
                        // Find the GameObject with the name of the neighboring territory.
                        GameObject neighborObject = GameObject.Find(neighborName);
                
                        // Ensure the neighboring GameObject exists.
                        if (neighborObject != null)
                        {   
                            // Get the Territory script attached to the neighboring GameObject.
                            Territory neighborScript = neighborObject.GetComponent<Territory>();
                    
                            // Check if the neighboring Territory script is found.
                            if (neighborScript != null)
                            {
                                // Add the neighboring Territory script to the list of neighbors.
                                neighbors.Add(neighborScript);
                            }
                            else{Debug.Log($"Found neighbour {neighborName}, but not script for {neighborName}");}
                        }
                        else{Debug.Log($"Neighbour {neighborName} not found");}
                    }
            
                    // Add the territory and its list of neighbors to the new dictionary.
                    neighbours.Add(territoryScript, neighbors);
                }
                else{Debug.Log($"Found {entry.Key}, but not script for {entry.Key}");}
            }
            else{Debug.Log($"Territory: {entry.Key}, not found");}
        }

        
        foreach (KeyValuePair<Territory,List<Territory>> keyValuePair in neighbours)
        {  
            keyValuePair.Key.neighbourTerritories = keyValuePair.Value;
        }
        
    }

    public void EliminatePlayer(Player player)
    {
        playerList.Remove(player);
        turn.players.Remove(player);
    }
}
