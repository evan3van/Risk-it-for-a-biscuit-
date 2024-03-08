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
    public int deployableTroops;
    public void PlayPhase()
    {
        Debug.Log("PlayPhase");
        Debug.Log(players.Count);
        turnMode = "Play";
        turnNumber++;
        playerPointer++;

        myTurn = players[playerPointer-1];

        playerUIName.GetComponent<TextMeshProUGUI>().text = players[playerPointer-1].name;

        if(playerPointer>=players.Count)
        {
            playerPointer = 0;
            Debug.Log("Full round complete");
        }
        nextPlayer = players[playerPointer];
    }

    public void ReinforcementPhase()
    {
        Debug.Log("ReinforcementPhase");
        turnMode = "Reinforcement";

        TextMeshProUGUI reinforcementUINumber = GameObject.Find("ReinforceTroopNumber").GetComponent<TextMeshProUGUI>();

        int reinforcementNum = myTurn.controlledTerritories.Count / 3;
        deployableTroops = reinforcementNum;
        reinforcementUINumber.text = reinforcementNum.ToString();
        myTurn.GiveTroops(reinforcementNum);

    }
    
    public void AttackPhase()
    {
        Debug.Log("AttackPhase");
        turnMode = "Attack";
    }

    public void FortifyPhase()
    {
        Debug.Log("FortifyPhase");
        turnMode = "Fortify";
    }

    
}
