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
    public GameObject playerUIName,attackUI;
    public int deployableTroops;
    public int attackTroops = 1;
    public GameObject arrowUp,arrowDown,deployButton,errorText;
    public Territory previousSelected,selected = null;
    public Territory attacker,attackTarget;
    public bool attackUIActive;

    public void PlayPhase()
    {
        Debug.Log("PlayPhase");
        //Debug.Log(players.Count);
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

        arrowUp.GetComponent<SpriteRenderer>().enabled = false;
        arrowDown.GetComponent<SpriteRenderer>().enabled = false;
        deployButton.GetComponent<SpriteRenderer>().enabled = false;
        errorText.GetComponent<MeshRenderer>().enabled = false;

        
    }
    public void FortifyPhase()
    {
        Debug.Log("FortifyPhase");
        turnMode = "Fortify";

        if(selected != null)
        {
            selected.HideArrows();
            selected.DisableAttackHighlight();
        }
        if (previousSelected != null)
        {
            previousSelected.DisableAttackHighlight();
            previousSelected.HideArrows();
        }
        selected = null;
        previousSelected = null;
    }

    public void IncrementAttackTroops()
    {
        attacker.attackTroops++;
        Debug.Log(attacker.counter.troopCount);
        if (attacker.attackTroops >= attacker.counter.troopCount-1)
        {
            attacker.attackTroops = attacker.counter.troopCount-1;
            if(attacker.attackTroops <= 0)
            {
                attacker.attackTroops = 1;
            }
        }
        attackUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = attacker.attackTroops.ToString();
    }

    public void DecrementAttackTroops()
    {
        attacker.attackTroops--;
        if (attacker.attackTroops <= 1)
        {
            attacker.attackTroops = 1;
        }
        attackUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = attacker.attackTroops.ToString();
    }
}
