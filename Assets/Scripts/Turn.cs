using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    int playerPointer = 0;
    int turnNumber = 0;
    public Player nextPlayer;
    public List<Player> players;
    public Player myTurn;
    public string turnMode;
    public GameObject playerUIName;
    public void NextTurn(Player player)
    {
        turnNumber++;
        playerPointer++;

        myTurn = player;

        if(playerPointer>players.Count)
        {
            playerPointer = 0;
        }
        nextPlayer = players[playerPointer];
    }

    public void ReinforcementPhase(Player player)
    {
        int reinforcementNum = player.controlledTerritories.Count / 3;
        player.GiveTroops(reinforcementNum);

        turnMode = "Reinforcement";

    }
    
    void AttackPhase()
    {

    }

    void FortifyPhase()
    {
        NextTurn(nextPlayer);
    }
}
