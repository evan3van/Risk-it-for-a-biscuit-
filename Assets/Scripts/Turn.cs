using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Turn : MonoBehaviour
{
    int playerPointer = 0;
    int turnNumber = 0;
    public Player nextPlayer;
    public List<Player> players;
    public Player myTurn;
    public string turnMode;
    public TextMesh playerUIName;
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

        ReinforcementPhase(player);
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
