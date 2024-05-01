using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Controlls the AI player
/// </summary>
public class AIBehavior : MonoBehaviour
{
    /// <summary>
    /// Time between AI actions in miliseconds
    /// </summary>
    public int timeBetweenActions = 500; 

    /// <summary>
    /// The player this AI script instance is associated with
    /// </summary>
    public Player player;

    /// <summary>
    /// Reference to the turn for general turn info
    /// </summary>
    public Turn turn;

    /// <summary>
    /// Task completion source to allow for the AI to wait on setting a task
    /// </summary>
    private TaskCompletionSource<bool> waitForDefenseDiceSelected;

    /// <summary>
    /// The AI difficulty
    /// </summary>
    public string mode = "Easy";

    /// <summary>
    /// Initialising turn and player objects
    /// </summary>
    void Start()
    {
        turn = FindObjectOfType<Turn>();
        player = gameObject.GetComponent<Player>();
    }

    /// <summary>
    /// Performs a specific action based on the current turn state
    /// </summary>
    public async void PerformAITurn()
    {
        switch (turn.turnMode)
        {
            case "Play":
                await Play();
                break;
            case "Reinforcement":
                await AIReinforce();
                break;
            case "Attack":
                await AIAttack();
                break;
            case "Fortify":
                await AIFortify();
                break;
        }
    }

    /// <summary>
    /// Press the play button
    /// </summary>
    async Task Play()
    {
        await Task.Delay(timeBetweenActions);
        turn.reinforceButton.onClick.Invoke();
    }

    /// <summary>
    /// Perform an AI reinforce turn
    /// </summary>
    async Task AIReinforce()
    {
        while(turn.deployableTroops > 0){
            await Task.Delay(timeBetweenActions);
            System.Random random = new System.Random();
            int territorySelect = random.Next(0,player.controlledTerritories.Count);
            Territory chosenTerritory = player.controlledTerritories[territorySelect];
            chosenTerritory.OnMouseDown();
            chosenTerritory.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

            
            int numberOfTerritoriesToDeploy = random.Next(1,turn.deployableTroops);
            for (int i = 0; i < numberOfTerritoriesToDeploy; i++)
            {
                await Task.Delay(timeBetweenActions/2);
                turn.arrowUp.GetComponent<ReinforcementScript>().OnMouseDown();
            }

            
            await Task.Delay(timeBetweenActions);
            turn.deployButton.GetComponent<ReinforcementScript>().OnMouseDown();
            chosenTerritory.gameObject.GetComponent<SpriteRenderer>().color = player.playerColor;
        }

       
        await Task.Delay(timeBetweenActions);
        turn.attackButton.onClick.Invoke();
    }

    /// <summary>
    /// Perform an AI attack turn
    /// </summary>
    async Task AIAttack()
    {
        for (int i = 0; i < player.unitCount/2; i++)
        {
            System.Random random = new System.Random();
            int counterValue = 0;
            Territory chosenTerritory = null;
            bool canSelect = false;
            List<Territory> possibleTerritories = new();
            foreach (Territory territory in player.controlledTerritories)
            {
                foreach (Territory neighbour in territory.neighbourTerritories)
                {
                    if (neighbour.controlledBy != player && territory.counter.troopCount > 1)
                    {
                        canSelect = true;
                        if(!possibleTerritories.Contains(territory))
                        {
                            possibleTerritories.Add(territory);
                        }
                    }
                }
            }
            while (counterValue <= 1 && canSelect)
            {
                int territorySelect = random.Next(0,possibleTerritories.Count-1);
                chosenTerritory = possibleTerritories[territorySelect];
                counterValue = chosenTerritory.counter.troopCount;
                foreach (Territory territory in chosenTerritory.neighbourTerritories)
                {
                    if (territory.controlledBy != player && chosenTerritory.counter.troopCount > 1)
                    {
                        canSelect = false;
                        break;
                    }
                }
            }
            if(chosenTerritory != null)
            {
                await Task.Delay(timeBetweenActions);
                chosenTerritory.OnMouseDown();

                
                List<Territory> options = new List<Territory>();
                foreach (Territory neighbour in chosenTerritory.neighbourTerritories)
                {
                    if(neighbour.controlledBy != player)
                    {
                        options.Add(neighbour);
                    }
                }
                int chosenNeighbour = random.Next(0,options.Count-1);
                Territory target = options[chosenNeighbour];
                await Task.Delay(timeBetweenActions);
                target.OnMouseDown();

                await Task.Delay(timeBetweenActions);
                turn.attackButtonUI.onClick.Invoke();
                
                int diceNumChoice = random.Next(1,turn.numberOfAttackDice);
                await Task.Delay(timeBetweenActions);
                turn.attackDiceNumbers[diceNumChoice].onClick.Invoke();

                if (turn.attackTarget.controlledBy.IsAI)
                {
                    waitForDefenseDiceSelected = new();
                    int diceNumChoice2 = random.Next(1,turn.numberOfDefenseDice);
                    turn.defenseDiceNumbers[diceNumChoice2].onClick.Invoke();
                }
                else
                {
                    waitForDefenseDiceSelected = new();
                    await waitForDefenseDiceSelected.Task;
                    Debug.Log("after");

                    for (int j = 0; j < turn.numberOfAttackDice; j++)
                    {
                        await Task.Delay(timeBetweenActions);
                        turn.attackerDice[j].GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                    }

                    await Task.Delay(timeBetweenActions);
                    turn.attackAgainButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                }
            }
        }

        await Task.Delay(timeBetweenActions);
        turn.fortifyButton.onClick.Invoke();
    }

    /// <summary>
    /// Perform an AI fortify turn
    /// </summary>
    async Task AIFortify()
    {
        await Task.Delay(timeBetweenActions);
        turn.endTurnButton.onClick.Invoke();
    }

    /// <summary>
    /// Setting the task completion source to true to allow for the defence dice to be selected by user
    /// </summary>
    public void StartWaitingForDefenseDiceSelected()
    {
        waitForDefenseDiceSelected.SetResult(true);
    }
}
