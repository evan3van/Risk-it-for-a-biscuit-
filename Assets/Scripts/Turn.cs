using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    int playerPointer = 0;
    int turnNumber = 0;
    public Player nextPlayer;
    public List<Player> players;
    public Player myTurn;
    string turnMode;
    public void nextTurn(Player player)
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

    public void nextPhase(String previousPhase)
    {
        if(previousPhase == "Reinforcement")
        {
            turnMode = "Attack";
        }
        else if(previousPhase == "Attack")
        {
            turnMode = "Fortify";
        }
        else if(previousPhase == "Fortify")
        {
            turnMode = "Reinforcement";
            nextTurn(nextPlayer);
        }
        else{Debug.Log("Error with phase selection");}
    }

    public void ReinforcementPhase(Player player)
    {
        int reinforcementNum = player.controlledTerritories.Count / 3;
        player.GiveTroops(reinforcementNum);

    }
    
    void AttackPhase()
    {

    }

    void FortifyPhase()
    {

    }
}
