using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    
    public int numberOfAttackDice=1,numberOfDefenseDice=1;

    /// <summary>
    /// The number of troops assigned for an attack during the attack phase.
    /// </summary>
    public int attackTroops = 1;

    // References to UI elements related to reinforcements and attacks
    public GameObject arrowUp,arrowDown,deployButton,errorText,diceSelection;

    public List<GameObject> attackerDice,defenderDice,attackerDiceChoice,defenderDiceChoice;

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

    public int numberOfRolledDice;
    public UnityEngine.UI.Image defenseDice1,defenseDice2;
    public List<Sprite> diceSprites;
    public List<int> attackRolls;

    public void EndTurn()
{
     // Example implementation to transition to the next player's turn
        playerPointer = (playerPointer + 1) % players.Count; // Cycle through the players list
        myTurn = players[playerPointer];
        // Reset or update necessary states for the new turn
        turnMode = "Reinforcement"; // For example, starting a new turn with Reinforcement phase
        // Additional logic to update UI or game state as needed
}

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

        attackerDice[0].GetComponent<DiceRoller>().isRolled = false;
        attackerDice[1].GetComponent<DiceRoller>().isRolled = false;
        attackerDice[2].GetComponent<DiceRoller>().isRolled = false;
        errorText.GetComponent<TextMeshProUGUI>().enabled = true;

        foreach (GameObject item in attackerDice)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in defenderDice)
        {
            item.SetActive(false);
        }
        attackerDice[0].transform.parent.gameObject.SetActive(false);

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
        attackUI.SetActive(false);
        attackUIActive = false;
        diceSelection.SetActive(true);
        for (int i = 0; i < numberOfAttackDice; i++)
        {
            attackerDiceChoice[i].SetActive(true);
        }
        for (int i = 0; i < numberOfDefenseDice; i++)
        {
            defenderDiceChoice[i].SetActive(true);
        }
    }

    public void SetNumberOfAttackDice(int number){
        numberOfAttackDice = number;
    }
    public void SetNumberOfDefenseDice(int number){
        numberOfDefenseDice = number;
    }

    public void CheckIfDiceRolled(int rollNumber){
        attackRolls.Add(rollNumber);
        if(numberOfRolledDice >= numberOfAttackDice){
            System.Random rand = new System.Random();
            int defenderRoll1 = rand.Next(1, 7);
            int defenderRoll2 = rand.Next(1, 7);
            defenseDice1.sprite = diceSprites[defenderRoll1-1];
            defenseDice2.sprite = diceSprites[defenderRoll2-1];

            bool compareMultiple = true;
            if (numberOfDefenseDice == 1)
            {
                compareMultiple = false;
            }

            HandleWinOrLoss(defenderRoll1,defenderRoll2,compareMultiple);
        }
    }

    public void HandleWinOrLoss(int defenderRoll1, int defenderRoll2, bool compareMultiple)
    {
        int highestAttackRoll1 = GetHighestAttackRoll();
        int highestDefenseRoll1 = Mathf.Max(defenderRoll1, defenderRoll2);
        int highestDefenseRoll2 = Mathf.Min(defenderRoll1, defenderRoll2);

        if (highestAttackRoll1 > highestDefenseRoll1)
        {
            Debug.Log("Attacker wins the first comparison");
            if (attackRolls.Count > 1 && compareMultiple)
            {
                int highestAttackRoll2 = GetSecondHighestAttackRoll(highestAttackRoll1);
                if (highestAttackRoll2 > highestDefenseRoll2)
                {
                    Debug.Log("Defender loses 2 armies");
                    attackTarget.controlledBy.unitCount -= 2;
                }
                else if(highestAttackRoll2 < highestDefenseRoll2)
                {
                    Debug.Log("Defender and attacker lose 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                }
                else
                {
                    Debug.Log("Attacker and defender lose 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                    attacker.controlledBy.unitCount -= 1;
                }
            }
            else
            {
                Debug.Log("Defender loses 1 army");
                attackTarget.controlledBy.unitCount -= 1;
            }
        }
        else if (highestAttackRoll1 < highestDefenseRoll1)
        {
            Debug.Log("Attacker loses the first comparison");
            if (attackRolls.Count > 1 && compareMultiple)
            {
                int highestAttackRoll2 = GetSecondHighestAttackRoll(highestAttackRoll1);
                if (highestAttackRoll2 > highestDefenseRoll2)
                {
                    Debug.Log("Defender loses 1 army, attacker loses 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                    attacker.controlledBy.unitCount -= 1;
                }
                else if (highestAttackRoll2 < highestDefenseRoll2)
                {
                    Debug.Log("Attacker loses 2 armies");
                    attacker.controlledBy.unitCount -= 2;
                }
                else
                {
                    Debug.Log("Attacker loses 2 armies");
                    attacker.controlledBy.unitCount -= 2;
                }
            }
            else
            {
                Debug.Log("Attacker loses 1 army");
                attacker.controlledBy.unitCount -= 1;
            }
        }
        else
        {
            Debug.Log("Attacker and defender drew highest rolls");
            if (attackRolls.Count > 1 && compareMultiple)
            {
                int highestAttackRoll2 = GetSecondHighestAttackRoll(highestAttackRoll1);
                if (highestAttackRoll2 > highestDefenseRoll2)
                {
                    Debug.Log("Defender loses 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                }
                else if(highestAttackRoll2 < highestDefenseRoll2)
                {
                    Debug.Log("Attacker loses 2 armies");
                    attacker.controlledBy.unitCount -= 1;
                }
                else
                {
                    Debug.Log("Attacker and defender lose 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                    attacker.controlledBy.unitCount -= 1;
                }
            }
            else
            {
                Debug.Log("Attacker loses 1 army");
                attacker.controlledBy.unitCount -= 1;
            }
        }
    }

    private int GetHighestAttackRoll()
    {
        int highestAttackRoll = int.MinValue;
        foreach (int roll in attackRolls)
        {
            highestAttackRoll = Mathf.Max(highestAttackRoll, roll);
        }
        return highestAttackRoll;
    }

    private int GetSecondHighestAttackRoll(int highestRoll1)
    {
        int highestAttackRoll2 = int.MinValue;
        foreach (int roll in attackRolls)
        {
            if (roll != highestRoll1)
            {
                highestAttackRoll2 = Mathf.Max(highestAttackRoll2, roll);
            }
        }
        return highestAttackRoll2;
    }
}
