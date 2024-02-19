using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int numOfPlayers = 2;
    public List<Player> playerList;
    private TextMesh textMesh;
    public int turnNumber;
    List<Continent> continents;
    public GameObject continentList;
    void Start()
    {
        foreach (Transform child in continentList.transform)
        {
            Continent continent = child.gameObject.GetComponent<Continent>();
            continents.Add(continent);
        }
        Debug.Log(continents.Count);

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
