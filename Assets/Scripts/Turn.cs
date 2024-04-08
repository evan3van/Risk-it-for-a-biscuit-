using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages turn-based gameplay, including tracking the current player, turn number, and game phase. 
/// Also handles UI updates and interactions related to each phase of the game.
/// </summary>
public class Turn : MonoBehaviour
{
    /// <summary>
    /// Index of the current player in the players list.
    /// </summary>
    int playerPointer = 0;
    
    /// <summary>
    /// The current turn number in the game.
    /// </summary>
    int turnNumber = 0;

    /// <summary>
    /// The player who will take the next turn.
    /// </summary>
    public Player nextPlayer;

    /// <summary>
    /// List of all players in the game.
    /// </summary>
    public List<Player> players;

    /// <summary>
    /// The player whose turn it is currently.
    /// </summary>
    public Player myTurn;

    /// <summary>
    /// The current phase of the turn (e.g., "Play", "Reinforcement", "Attack", "Fortify").
    /// </summary>
    public string turnMode;

    /// <summary>
    /// UI element that displays the name of the player whose turn it is.
    /// UI element for managing attacks.
    /// </summary>
    public GameObject playerUIName,attackUI;

    /// <summary>
    /// The number of troops available for deployment during the reinforcement phase.
    /// </summary>
    public int deployableTroops;

    /// <summary>
    /// The number of troops assigned for an attack during the attack phase.
    /// </summary>
    public int attackTroops = 1;

    // References to UI elements related to reinforcements and attacks
    public GameObject arrowUp,arrowDown,deployButton,errorText,dice;

    /// <summary>
    /// The previously selected territory.
    /// The currently selected territory.
    /// </summary>
    public Territory previousSelected,selected = null;

    /// <summary>
    /// The attacking territory during the attack phase.
    /// The target territory during the attack phase.
    /// </summary>
    public Territory attacker,attackTarget;

    /// <summary>
    /// Indicates whether the attack UI is currently active.
    /// Indicates whether the attack highlight is currently active.
    /// </summary>
    public bool attackUIActive,isAttackHighlighted = false;

    /// <summary>
    /// UI text element that displays the target of an attack.
    /// UI text element that displays the attack button text.
    /// </summary>
    public TextMeshProUGUI attackTargetText,attackButtonText;

    /// <summary>
    /// Transitions the game to the Play phase.
    /// </summary>
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

    /// <summary>
    /// Transitions the game to the Reinforcement phase, calculating and displaying available reinforcements.
    /// </summary>
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

    /// <summary>
    /// Transitions the game to the Attack phase, preparing UI and game state for attacking.
    /// </summary>
    public void AttackPhase()
    {
        Debug.Log("AttackPhase");
        turnMode = "Attack";

        arrowUp.GetComponent<SpriteRenderer>().enabled = false;
        arrowDown.GetComponent<SpriteRenderer>().enabled = false;
        deployButton.GetComponent<SpriteRenderer>().enabled = false;
        errorText.GetComponent<TextMeshProUGUI>().enabled = true;

        
    }

    /// <summary>
    /// Transitions the game to the Fortify phase, allowing players to move troops between territories.
    /// </summary>
    public void FortifyPhase()
    {
        
        Debug.Log("FortifyPhase");
        turnMode = "Fortify";

        errorText.GetComponent<TextMeshProUGUI>().enabled = true;

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

        if(attackUIActive == true)
        {
            attackUIActive = false;
            attackUI.SetActive(false);
        }
    }

    /// <summary>
    /// Increments the number of troops assigned to an attack.
    /// </summary>
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

    /// <summary>
    /// Decrements the number of troops assigned to an attack.
    /// </summary>
    public void DecrementAttackTroops()
    {
        attacker.attackTroops--;
        if (attacker.attackTroops <= 1)
        {
            attacker.attackTroops = 1;
        }
        attackUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = attacker.attackTroops.ToString();
    }

    /// <summary>
    /// Initiates an attack from the selected territory to the target territory.
    /// </summary>
    public void TriggerAttack()
    {
        if (attackTarget==null)
        {
            errorText.GetComponent<TextMeshProUGUI>().text = "No target!";
            return;
        }
        else if (selected==null)
        {
            errorText.GetComponent<TextMeshProUGUI>().text = "No territory selected!";
            return;
        }
        
        dice.SetActive(true);
    }
}
