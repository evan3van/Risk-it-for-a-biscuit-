using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
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
    /// Reference to the game manager script
    /// </summary>
    public GameManager gameManager;

    /// <summary>
    /// Index of the current player in the players list.
    /// </summary>
    public int playerPointer = 0;
    
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
    /// The current phase of the turn ("Play", "Reinforcement", "Attack", "Fortify").
    /// </summary>
    public string turnMode;

    /// <summary>
    /// UI elements
    /// </summary>
    public GameObject playerUIName,attackUI;

    /// <summary>
    /// The number of troops available for deployment during the reinforcement/fortify phase.
    /// </summary>
    public int deployableTroops;

    /// <summary>
    /// The number of attack and defense dice available to be rolled
    /// </summary>
    public int numberOfAttackDice=1,numberOfDefenseDice=1;

    /// <summary>
    /// The number of troops assigned for an attack during the attack phase.
    /// </summary>
    public int attackTroops = 1;

    /// <summary>
    /// UI elements related to reinforcing, attacking or fortifying
    /// </summary>
    public GameObject arrowUp,arrowDown,deployButton,errorText,diceSelection,diceMenu,attackAgainButton;

    /// <summary>
    /// Lists of dice/dice choices
    /// </summary>
    public List<GameObject> attackerDice,defenderDice,attackerDiceChoice,defenderDiceChoice;

    /// <summary>
    /// The currently/previously selected territories
    /// </summary>
    public Territory previousSelected,selected = null;

    /// <summary>
    /// The relevent territories involved in an attack
    /// </summary>
    public Territory attacker,attackTarget;

    /// <summary>
    /// Indicates whether the attack UI is currently active.
    /// </summary>
    public bool attackUIActive,isAttackHighlighted = false;

    /// <summary>
    /// UI text elements to signal important game mechanics to the player.
    /// </summary>
    public TextMeshProUGUI attackTargetText,attackButtonText,reinforcementUINumber;

    /// <summary>
    /// Number of rolled dice so far this turn
    /// </summary>
    public int numberOfRolledDice;

    /// <summary>
    /// References to defense dice UI images
    /// </summary>
    public UnityEngine.UI.Image defenseDice1,defenseDice2;

    /// <summary>
    /// List of all dice numbered sprites
    /// </summary>
    public List<Sprite> diceSprites;

    /// <summary>
    /// List of current attack rolls this turn
    /// </summary>
    public List<int> attackRolls;

    /// <summary>
    /// Toggle for the player to interact with the map or not
    /// </summary>
    public bool territoryInteractToggle = true;

    /// <summary>
    /// UI elements for fortify phase
    /// </summary>
    public GameObject chooseSender,sendTo,resetButton,fortifyUI;

    /// <summary>
    /// Player UI image in the scene
    /// </summary>
    public UnityEngine.UI.Image playerCharacter;

    /// <summary>
    /// Reference to the card manager
    /// </summary>
    public CardManager cardManager;

    /// <summary>
    /// Total number of traded in sets this game
    /// </summary>
    public int numOfTradedInSets = 0;

    /// <summary>
    /// Base number of reinforcements to recieve from trading in
    /// </summary>
    public List<int> tradedInSetReinforcements = new(){4,6,8,10,12,15};

    /// <summary>
    /// Buttons for changing the turn mode
    /// </summary>
    public UnityEngine.UI.Button reinforceButton,attackButton,fortifyButton,endTurnButton;

    /// <summary>
    /// Attack phase attack button
    /// </summary>
    public UnityEngine.UI.Button attackButtonUI;

    /// <summary>
    /// Buttons for choosing the number of dice for the attacker
    /// </summary>
    public List<UnityEngine.UI.Button> attackDiceNumbers;

    /// <summary>
    /// Buttons for choosing the number of dice for the defender
    /// </summary>
    public List<UnityEngine.UI.Button> defenseDiceNumbers;

    /// <summary>
    /// Toggle for if the dice have been chosen by the defender or not
    /// </summary>
    public bool isDefenseDiceSelected = false;

    /// <summary>
    /// Gets the next player in line
    /// </summary>
    public Player GetNextPlayer()
    {
        return nextPlayer;
    }

    /// <summary>
    /// Transitions the game to the Play phase.
    /// </summary>
    public void PlayPhase()
    {
        Debug.Log("PlayPhase");
        turnMode = "Play";
        turnNumber++;
        playerPointer++;

        myTurn = players[playerPointer-1];
        playerCharacter.sprite = myTurn.sprite;

        if (players[playerPointer-1].myName != null)
        {
            playerUIName.GetComponent<TextMeshProUGUI>().text = players[playerPointer-1].myName;
        }
        else{playerUIName.GetComponent<TextMeshProUGUI>().text = players[playerPointer-1].name;}
        playerUIName.GetComponent<TextMeshProUGUI>().color = myTurn.playerColor;

        if(playerPointer>=players.Count)
        {
            playerPointer = 0;
            Debug.Log("Full round complete");
        }
        nextPlayer = players[playerPointer];

        territoryInteractToggle = true;
        chooseSender.SetActive(true);
        sendTo.SetActive(false);

        foreach (Territory territory in gameManager.allTerritories)
        {
            territory.DisableAttackHighlight();
            if (turnNumber > 1)
            {
                territory.HideArrows();
            }
        }

        cardManager.selectedCards.Clear();

        if (myTurn.IsAI)
        {
            myTurn.aIBehavior.PerformAITurn();
        }
    }

    /// <summary>
    /// Transitions the game to the Reinforcement phase, calculating and displaying available reinforcements.
    /// </summary>
    public void ReinforcementPhase()
    {
        Debug.Log("ReinforcementPhase");
        turnMode = "Reinforcement";

        int reinforcementNum = myTurn.controlledTerritories.Count / 3;
        if(reinforcementNum < 3)
        {
            reinforcementNum = 3;
        }
        reinforcementNum += myTurn.extraReinforcements;
        myTurn.extraReinforcements = 0;
        deployableTroops = reinforcementNum;
        reinforcementUINumber.text = reinforcementNum.ToString();
        myTurn.GiveTroops(reinforcementNum);

        if (myTurn.IsAI)
        {
            myTurn.aIBehavior.PerformAITurn();
        }
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

        territoryInteractToggle = true;

        if (myTurn.IsAI)
        {
            myTurn.aIBehavior.PerformAITurn();
        }
    }

    /// <summary>
    /// Transitions the game to the Fortify phase, allowing players to move troops between territories.
    /// </summary>
    public void FortifyPhase()
    {
        
        Debug.Log("FortifyPhase");
        turnMode = "Fortify";

        attackTroops = 0;

        if(selected != null)
        {
            selected.gameObject.GetComponent<OnHoverHighlight>().mode = "Default";
            foreach (Territory neighbour in selected.neighbourTerritories)
            {
                neighbour.gameObject.GetComponent<OnHoverHighlight>().mode = "Default";
            }
            foreach (GameObject arrow in selected.arrows)
            {
                arrow.SetActive(false);
            }
            selected = null;
        }

        numberOfRolledDice = 0;
        numberOfAttackDice = 1;
        numberOfDefenseDice = 1;

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

        attackAgainButton.SetActive(true);
        territoryInteractToggle = true;

        if (myTurn.IsAI)
        {
            myTurn.aIBehavior.PerformAITurn();
        }
    }

    /// <summary>
    /// Increments the number of troops assigned to an attack.
    /// </summary>
    public void IncrementAttackTroops()
    {
        attacker.attackTroops++;
        if (attacker.attackTroops >= attacker.counter.troopCount-1)
        {
            attacker.attackTroops = attacker.counter.troopCount-1;
            if(attacker.attackTroops <= 0)
            {
                attacker.attackTroops = 1;
            }
        }
        attackTroops++;
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
        attackTroops--;
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
        territoryInteractToggle = false;
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

    /// <summary>
    /// Sets the number of attack dice
    /// </summary>
    /// <param name="number">Number of attack dice</param>
    public void SetNumberOfAttackDice(int number)
    {
        numberOfAttackDice = number;
    }

    /// <summary>
    /// Sets the numbe of defense dice
    /// </summary>
    /// <param name="number">Number of defense dice</param>
    public void SetNumberOfDefenseDice(int number)
    {
        numberOfDefenseDice = number;
    }

    /// <summary>
    /// Toggles the <see cref="isDefenseDiceSelected"/> field
    /// </summary>
    public void SetIsDefenseDiceActive()
    {
        isDefenseDiceSelected = true;
    }

    /// <summary>
    /// Checks if all the dice have been rolled and auto rolls defender dice
    /// </summary>
    /// <param name="rollNumber">Attacker roll to add to the list</param>
    public void CheckIfDiceRolled(int rollNumber)
    {
        attackRolls.Add(rollNumber);
        if(numberOfRolledDice >= numberOfAttackDice){
            System.Random rand = new System.Random();
            int defenderRoll1 = rand.Next(1, 7);
            int defenderRoll2 = rand.Next(1, 7);
            defenseDice1.sprite = diceSprites[defenderRoll1-1];
            defenseDice2.sprite = diceSprites[defenderRoll2-1];

            bool compareMultiple = numberOfDefenseDice == 2;

            attacker.HideArrows();
            attackTarget.GetComponent<OnHoverHighlight>().origionalColor = attackTarget.controlledBy.playerColor;
            attackTarget.GetComponent<OnHoverHighlight>().mode = "Default";
            HandleWinOrLoss(defenderRoll1,defenderRoll2,compareMultiple);
        }
    }

    /// <summary>
    /// Handles a win or loss event for the attack based on specifications in the rules
    /// </summary>
    /// <param name="defenderRoll1">Defender's first roll</param>
    /// <param name="defenderRoll2">Defender's second roll</param>
    /// <param name="compareMultiple">Is there multiple defender dice?</param>
    public void HandleWinOrLoss(int defenderRoll1, int defenderRoll2, bool compareMultiple)
    {

        int highestAttackRoll1 = GetHighestAttackRoll();
        int highestDefenseRoll1 = Mathf.Max(defenderRoll1, defenderRoll2);
        int highestDefenseRoll2 = Mathf.Min(defenderRoll1, defenderRoll2);
        if (!compareMultiple)
        {
            highestDefenseRoll1 = defenderRoll1;
            highestDefenseRoll2 = 0;
        }
        bool defenderLosesTerritory = false;
        bool attackerLosesTerritory = false;
        Debug.Log("Highest attack roll: "+highestAttackRoll1+" Highest defense roll: "+highestDefenseRoll1);
        

        if (highestAttackRoll1 > highestDefenseRoll1)
        {
            Debug.Log("Attacker wins the first comparison");
            if (attackRolls.Count > 1 && compareMultiple)
            {
                int highestAttackRoll2 = GetSecondHighestAttackRoll(highestAttackRoll1);
                if (highestAttackRoll2 > highestDefenseRoll2)
                {
                    if (attackTarget.counter.troopCount <= 2)
                    {
                        defenderLosesTerritory = true;
                    }
                    Debug.Log("Defender loses 2 armies");
                    attackTarget.controlledBy.unitCount -= 2; 
                    attackTarget.counter.UpdateCount(attackTarget.counter.troopCount-2);
                    attackTarget.counter.troopCount -= 2;
                }
                else if(highestAttackRoll2 < highestDefenseRoll2)
                {
                    if (attackTarget.counter.troopCount <= 1)
                    {
                        defenderLosesTerritory = true;
                    }
                    Debug.Log("Defender and attacker lose 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                    attackTarget.counter.UpdateCount(attackTarget.counter.troopCount-1);
                    attackTarget.counter.troopCount -= 1;
                }
                else
                {
                    if (attackTarget.counter.troopCount <= 1)
                    {
                        defenderLosesTerritory = true;
                    }
                    if (attacker.counter.troopCount <= 1)
                    {
                        attackerLosesTerritory = true;
                    }
                    Debug.Log("Attacker and defender lose 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                    attacker.controlledBy.unitCount -= 1;
                    attacker.counter.UpdateCount(attacker.counter.troopCount-1);
                    attackTarget.counter.UpdateCount(attackTarget.counter.troopCount-1);
                    attacker.counter.troopCount -= 1;
                    attackTarget.counter.troopCount -= 1;
                }
            }
            else
            {
                if (attackTarget.counter.troopCount <= 1)
                {
                    defenderLosesTerritory = true;
                }
                Debug.Log("Defender loses 1 army");
                attackTarget.controlledBy.unitCount -= 1;
                attackTarget.counter.UpdateCount(attackTarget.counter.troopCount-1);
                attackTarget.counter.troopCount -= 1;
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
                    if (attackTarget.counter.troopCount <= 1)
                    {
                        defenderLosesTerritory = true;
                    }
                    if (attackTarget.counter.troopCount <= 1)
                    {
                        defenderLosesTerritory = true;
                    }
                    Debug.Log("Defender loses 1 army, attacker loses 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                    attacker.controlledBy.unitCount -= 1;
                    attacker.counter.UpdateCount(attacker.counter.troopCount-1);
                    attackTarget.counter.UpdateCount(attackTarget.counter.troopCount-1);
                    attacker.counter.troopCount -= 1;
                    attackTarget.counter.troopCount -= 1;
                }
                else if (highestAttackRoll2 < highestDefenseRoll2)
                {
                    if (attacker.counter.troopCount <= 2)
                    {
                        attackerLosesTerritory = true;
                    }
                    Debug.Log("Attacker loses 2 armies");
                    attacker.controlledBy.unitCount -= 2;
                    attacker.counter.UpdateCount(attacker.counter.troopCount-2);
                    attacker.counter.troopCount -= 2;
                }
                else
                {
                    if (attacker.counter.troopCount <= 2)
                    {
                        attackerLosesTerritory = true;
                    }
                    Debug.Log("Attacker loses 2 armies");
                    attacker.controlledBy.unitCount -= 2;
                    attacker.counter.UpdateCount(attacker.counter.troopCount-2);
                    attacker.counter.troopCount -= 2;
                }
            }
            else
            {
                if (attacker.counter.troopCount <= 1)
                {
                    attackerLosesTerritory = true;
                }
                Debug.Log("Attacker loses 1 army");
                attacker.controlledBy.unitCount -= 1;
                attacker.counter.UpdateCount(attacker.counter.troopCount-1);
                attacker.counter.troopCount -= 1;
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
                    if (attackTarget.counter.troopCount <= 1)
                    {
                        defenderLosesTerritory = true;
                    }
                    Debug.Log("Defender loses 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                    attackTarget.counter.UpdateCount(attackTarget.counter.troopCount-1);
                    attackTarget.counter.troopCount -= 1;
                }
                else if(highestAttackRoll2 < highestDefenseRoll2)
                {
                    if (attacker.counter.troopCount <= 2)
                    {
                        attackerLosesTerritory = true;
                    }
                    Debug.Log("Attacker loses 2 armies");
                    attacker.controlledBy.unitCount -= 2;
                    attacker.counter.UpdateCount(attacker.counter.troopCount-2);
                    attacker.counter.troopCount -= 2;
                }
                else
                {
                    if (attackTarget.counter.troopCount <= 1)
                    {
                        defenderLosesTerritory = true;
                    }
                    if (attacker.counter.troopCount <= 1)
                    {
                        attackerLosesTerritory = true;
                    }
                    Debug.Log("Attacker and defender lose 1 army");
                    attackTarget.controlledBy.unitCount -= 1;
                    attacker.controlledBy.unitCount -= 1;
                    attacker.counter.UpdateCount(attacker.counter.troopCount-1);
                    attackTarget.counter.UpdateCount(attackTarget.counter.troopCount-1);
                    attacker.counter.troopCount -= 1;
                    attackTarget.counter.troopCount -= 1;
                    
                }
            }
            else
            {
                if (attacker.counter.troopCount <= 1)
                {
                    attackerLosesTerritory = true;
                }
                Debug.Log("Attacker loses 1 army");
                attacker.controlledBy.unitCount -= 1;
                attacker.counter.UpdateCount(attacker.counter.troopCount-1);
                attacker.counter.troopCount -= 1;
            }
        }


        Debug.Log("attackerLosesTerritory: "+attackerLosesTerritory+ " defenderLosesTerritory: "+defenderLosesTerritory);
        if(attackerLosesTerritory)
        {
            HandleTerritoryCapture(attackTarget,attacker);
        }
        else if(defenderLosesTerritory)
        {
            HandleTerritoryCapture(attacker,attackTarget);
        }
        else if(attackerLosesTerritory && defenderLosesTerritory)
        {
            HandleDraw(attacker,attackTarget);
        }
    }

    /// <summary>
    /// Gets the higest attack roll in the attack rolls list
    /// </summary>
    /// <returns>The higest attack roll</returns>
    private int GetHighestAttackRoll()
    {
        int highestAttackRoll = int.MinValue;
        foreach (int roll in attackRolls)
        {
            highestAttackRoll = Mathf.Max(highestAttackRoll, roll);
        }
        return highestAttackRoll;
    }

    /// <summary>
    /// Gets the next highest attack roll in the attack rolls list
    /// </summary>
    /// <param name="highestRoll1">The highest roll</param>
    /// <returns>The next highest roll</returns>
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

    /// <summary>
    /// Handles if a player loses their territory during an attack to their opponent (either attack or defense)
    /// </summary>
    /// <param name="winner">The winning territory</param>
    /// <param name="loser">The losing territory</param>
    public void HandleTerritoryCapture(Territory winner,Territory loser)
    {
        Player winnerPlayer = winner.controlledBy;
        Player loserPlayer = loser.controlledBy;
        loser.controlledBy = winnerPlayer;

        loser.counter.troopCount = 1;
        loser.counter.UpdateCount(loser.counter.troopCount);
        loser.counter.gameObject.GetComponent<SpriteRenderer>().color = new Color(winnerPlayer.playerColor.r * 0.8f, winnerPlayer.playerColor.g * 0.8f, winnerPlayer.playerColor.b * 0.8f);
        attacker.counter.UpdateCount(attacker.counter.troopCount-1);
        attacker.counter.troopCount -= 1;
        loserPlayer.unitCount -= 1;
        loserPlayer.counters.Remove(loser.counter);
        loserPlayer.controlledTerritories.Remove(loser);
        loser.gameObject.GetComponent<SpriteRenderer>().color = winnerPlayer.playerColor;
        loser.gameObject.GetComponent<OnHoverHighlight>().origionalColor = winnerPlayer.playerColor;
        loser.gameObject.GetComponent<OnHoverHighlight>().player = winnerPlayer;

        winnerPlayer.controlledTerritories.Add(loser);
        winnerPlayer.counters.Add(loser.counter);
    }

    public void HandleDraw(Territory territory1, Territory territory2)
    {
        Player player1 = territory1.controlledBy;
        Player player2 = territory2.controlledBy;
        territory1.counter.UpdateCount(1);
        territory2.counter.UpdateCount(1);
        player1.GiveTroops(1);
        player2.GiveTroops(1);
        territory1.counter.troopCount = 1;
        territory2.counter.troopCount = 1;
    }

    /// <summary>
    /// A method to allow for another attack to occur, by reseting the attack phase
    /// </summary>
    public void AttackAgain()
    {
        SetNumberOfAttackDice(1);
        SetNumberOfDefenseDice(1);

        foreach (GameObject attackDice in attackerDice)
        {
            attackDice.SetActive(false);
            attackDice.GetComponent<DiceRoller>().isRolled = false;
            attackDice.GetComponent<DiceRoller>().isChecked = false;
            attackDice.GetComponent<UnityEngine.UI.Image>().sprite = diceSprites[0];
        }
        foreach (GameObject defenseDice in defenderDice)
        {
            defenseDice.SetActive(false);
            defenseDice.GetComponent<UnityEngine.UI.Image>().sprite = diceSprites[0];
        }

        attackTarget.gameObject.GetComponent<SpriteRenderer>().color = attackTarget.controlledBy.playerColor;
        diceSelection.SetActive(false);
        diceMenu.SetActive(false);
        numberOfRolledDice = 0;
        attackRolls = new List<int>();
    }

    /// <summary>
    /// Toggles the territory interact
    /// </summary>
    public void TriggerTerritoryInteract()
    {
        territoryInteractToggle = true;
        selected = null;
        attacker = null;
        attackTarget = null;
    }

    /// <summary>
    /// Resets the fortify phase (for debugging)
    /// </summary>
    public void ResetFortify()
    {
        if (selected != null)
        {
            selected.HideArrows();
        }
        if (previousSelected != null)
        {
            previousSelected.HideArrows();
        }
        selected = null;
        previousSelected = null;
    }
    
    /// <summary>
    /// Triggers the AI to continue after the player has selected a defense dice option
    /// </summary>
    public void AwaitPlayerSelection()
    {
        if (myTurn.IsAI)
        {
            myTurn.GetComponent<AIBehavior>().StartWaitingForDefenseDiceSelected();
        }
    }
}
