using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int numOfPlayers = 3;
    void Start()
    {
        InstantiateWorld(numOfPlayers); //Instantiating the world prefabs will happen here (see unity instantiation documentation)
    }

    void FixedUpdate()
    {
        //Any checks for updates on things in the world will happen here
    }

    private void InstantiateWorld(int playerCount)
    {
        //method for instantiating the world initially taking into account the number of players
    }
}
