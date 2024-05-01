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
/// It initializes the game world, including territories, continents, and players.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The maximum number of armies to start the game with
    /// </summary>
    private int maxArmySize = 40;
    /// <summary>
    /// The total number of territories
    /// </summary>
    private int totalTerritories = 47;

    /// <summary>
    /// Number of players participating in the game
    /// </summary>
    public int numOfPlayers = 6;

    /// <summary>
    /// The number of AI players
    /// </summary>
    public int numOfAIPlayers;

    /// <summary>
    /// List of all player objects in the game
    /// </summary>
    public List<Player> playerList;

    /// <summary>
    /// Current turn number in the game
    /// </summary>
    public int turnNumber;

    /// <summary>
    /// List of all continents in the game
    /// </summary>
    public List<Continent> continents;

    /// <summary>
    /// List of all territories in the game
    /// </summary>
    public List<Territory> allTerritories;

    /// <summary>
    /// List of colors assigned to players
    /// </summary>
    public List<Color> playerColors;

    /// <summary>
    /// List of counter objects used to represent armies on the map
    /// </summary>
    public List<Counter> counters;

    /// <summary>
    /// Prefab used to instantiate new counter objects
    /// </summary>
    public GameObject counterPrefab;

    /// <summary>
    /// Prefab used to instantiate arrow objects
    /// </summary>
    public GameObject arrowPrefab;

    /// <summary>
    /// Reference to the current turn manager
    /// </summary>
    public Turn turn;

    /// <summary>
    /// GameObject representing the player UI name display
    /// </summary>
    public GameObject playerUIName;

    /// <summary>
    /// Dictionary mapping territory names to their neighbouring territories' names
    /// </summary>
    public Dictionary<string,List<string>> territoriesNeighbours;

    /// <summary>
    /// List of sprites representing players
    /// </summary>
    public List<Sprite> playerSprites;

    /// <summary>
    /// A reference to the theme swapper component
    /// </summary>
    public ThemeSwapper themeSwapper;

    /// <summary>
    /// The game objects representing the map,win menu and canvas for toggling
    /// </summary>
    public GameObject map,winMenu,gameCanvas;

    /// <summary>
    /// The sprite for the cyber theme
    /// </summary>
    public Sprite cyberMap;

    /// <summary>
    /// A flag to allow the update method to start
    /// </summary>
    public bool canUpdate = false;

    /// <summary>
    /// List of all the mission cards
    /// </summary>
    public List<Sprite> missionCards;

    /// <summary>
    /// Reference to the player customisation from the main menu
    /// </summary>
    public PlayerCustomiseScript playerCustomiseScript;

    /// <summary>
    /// List of player UI names
    /// </summary>
    public List<string> playerNames;

    /// <summary>
    /// Initialises the theme swapper and customise info to pull information from the main menu.
    /// Adds territory components to each territory gameobject.
    /// Performs other general game initialisation/setup for any other scripts that need initialisation
    /// </summary>
    void Start()
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

        if (GameObject.Find("CustomiseInfo") != null )
        {
            playerCustomiseScript = GameObject.Find("CustomiseInfo").GetComponent<PlayerCustomiseScript>();
            numOfPlayers = playerCustomiseScript.numberOfHumanPlayers+playerCustomiseScript.numberOfAIPlayers;
            playerNames = playerCustomiseScript.playerNames;
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                playerNames.Add("Player "+i);
            }
        }
        
        foreach (Continent continent in continents)
        {
            
            foreach (Transform territoryObject in continent.transform)
            {
                
                Territory territory = territoryObject.AddComponent<Territory>();
                
                
                continent.territories.Add(territory);
            }
        }

        
        if(numOfPlayers > 6)
        {
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

    /// <summary>
    /// Checks for significant conditions such as player victory
    /// </summary>
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
        int numberOfHumanPlayers = numOfPlayers;
        int AISpeed = 300;
        if(playerCustomiseScript != null)
        {
            numberOfHumanPlayers = playerCustomiseScript.numberOfHumanPlayers;
            AISpeed = playerCustomiseScript.AISpeed;

        }
        
        for (int i = 0; i < numOfPlayers; i++)
        {
            GameObject playerObject = new GameObject("Player "+(i+1));
            Player player = playerObject.AddComponent<Player>();
            player.playerColor = playerColors[i];
            if (i >= numberOfHumanPlayers)
            {
                player.IsAI = true;
                player.aIBehavior = playerObject.AddComponent<AIBehavior>();
                if (AISpeed > 0 && playerCustomiseScript != null)
                {
                    player.aIBehavior.timeBetweenActions = playerCustomiseScript.AISpeed;
                }
                else if(AISpeed == 0)
                {
                    player.aIBehavior.timeBetweenActions = playerCustomiseScript.AISpeed+20;
                }
                else
                {
                    player.aIBehavior.timeBetweenActions= AISpeed;
                }
            }

            if (playerNames.Count > i && player.IsAI == false)
            {
                player.myName = playerNames[i];
            }
            else if (player.IsAI == true)
            {
                player.myName = "AI "+i;
            }
            else
            {
                player.myName = "Player "+(i+1);
            }

            if (playerCustomiseScript != null && playerCustomiseScript.chosenSprites.Count > i && player.IsAI == false)
            {
                player.sprite = playerCustomiseScript.chosenSprites[i];
            }
            else
            {
                player.sprite = playerSprites[i];
            }
            playerList.Add(player);
        }

        System.Random random = new System.Random();
        foreach (Player player in playerList)
        {
            int value = random.Next(0,missionCards.Count);
            player.missionCard = missionCards[value];
            missionCards.RemoveAt(value);
        }

        
        maxArmySize -= 5*(numOfPlayers-2);

        
        SetTerritories();

        
        PlaceArmies();

        
        SetNeighbours();
    }

    /// <summary>
    /// Assigns territories to players, distributing them evenly and setting initial control.
    /// </summary>
    void SetTerritories()
    {
        
        List<Territory> territoryList = new List<Territory>();

        
        foreach (Continent continent in continents)
        {
            
            foreach (Territory territory in continent.territories)
            {
                
                allTerritories.Add(territory);
                
                territoryList.Add(territory);
            }
        }


        
        int playerCount = playerList.Count;

        
        if (playerList.Count <= 3){
        
            playerCount = 3;
        }

        
        int territoriesPerPlayer = totalTerritories/playerCount;

        
        foreach (Player player in playerList)
        {
            
            for (int i = 0; i < territoriesPerPlayer; i++)
            {
                
                int randomNumber = Random.Range(0,territoryList.Count);
                Territory choice = territoryList[randomNumber];
                
                
                if(choice != null && choice.controlledBy == null)
                {
                    
                    choice.GetComponent<SpriteRenderer>().color = player.playerColor;

                    OnHoverHighlight highlight = choice.GetComponent<OnHoverHighlight>();
                    highlight.origionalColor = player.playerColor;
                    highlight.player = player;
                    highlight.currentTurn = turn;    

                    
                    player.controlledTerritories.Add(choice);
                    
                    
                    choice.controlledBy = player;
                    
                    
                    territoryList.Remove(choice);
                }
            }
        }
        
        foreach (Territory territory in territoryList.ToArray())
        {
            
            playerList[0].controlledTerritories.Add(territory);

            
            territory.GetComponent<SpriteRenderer>().color = playerList[0].playerColor;

            OnHoverHighlight highlight = territory.GetComponent<OnHoverHighlight>();
            highlight.origionalColor = playerList[0].playerColor;
            highlight.player = playerList[0];
            highlight.currentTurn = turn;

            territory.controlledBy = playerList[0];

            
            territoryList.Remove(territory);
        }

        
    }

    /// <summary>
    /// Places initial armies on territories controlled by players.
    /// </summary>
    void PlaceArmies()
    {
        
        foreach (Player player in playerList)
        {
            
            Color playerColor = player.playerColor;
            
            
            Color counterColor = new Color(playerColor.r * 0.8f, playerColor.g * 0.8f, playerColor.b * 0.8f);

            
            foreach (Territory territory in player.controlledTerritories)
            {
                
                GameObject counter = Instantiate(counterPrefab, transform, true);
                
                
                counter.name = "Counter: " + territory.name;
                
                
                counter.GetComponent<SpriteRenderer>().color = counterColor;
                
                
                counter.transform.position = new Vector3(territory.transform.position.x, territory.transform.position.y, -3);
                
                
                counter.transform.parent = territory.transform;
                
                
                Counter counterScript = counter.GetComponent<Counter>();
                
                
                counterScript.ownedBy = player;
                player.counters.Add(counterScript);
                
                
                counters.Add(counterScript);
            }
        }


        foreach (Player player in playerList)
        {
            player.unitCount = maxArmySize;
            int iterations = maxArmySize;
            foreach (Counter counter in player.counters)
            {
                
                counter.troopCount = 1;

                
                iterations -= 1;
            }
            

            while (iterations > 0)
            {
                
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
            GameObject territoryObject = GameObject.Find(entry.Key);
            if (territoryObject != null)
            {
                // Get the Territory script attached to the GameObject.
                Territory territoryScript = territoryObject.GetComponent<Territory>();
    
                if (territoryScript != null)
                {
                    // Create a list to hold references to neighboring territories.
                    List<Territory> neighbors = new List<Territory>();
                    foreach (string neighborName in entry.Value)
                    {
                        // Find the GameObject with the name of the neighboring territory.
                        GameObject neighborObject = GameObject.Find(neighborName);
                        if (neighborObject != null)
                        {   
                            // Get the Territory script attached to the neighboring GameObject.
                            Territory neighborScript = neighborObject.GetComponent<Territory>();
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

    /// <summary>
    /// Effectively eliminates a player from the game
    /// </summary>
    /// /// <param name="player">The player to be eliminated</param>
    public void EliminatePlayer(Player player)
    {
        playerList.Remove(player);
        turn.players.Remove(player);
    }
}
