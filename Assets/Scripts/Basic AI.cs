using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    public int timeBetweenActions = 3000; //(Milisecs)
    public Player player;
    public Turn turn;
    public ReinforcementScript reinforcementScript;
    public string mode = "Easy";

    void Start()
    {
        turn = FindObjectOfType<Turn>();
        player = gameObject.GetComponent<Player>();
    }

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

    async Task Play()
    {
        //Skip play phase
        await Task.Delay(timeBetweenActions);
        turn.reinforceButton.onClick.Invoke();
    }
    async Task AIReinforce()
    {
        while(turn.deployableTroops > 0){
            //Select a random territory
            await Task.Delay(timeBetweenActions);
            System.Random random = new System.Random();
            int territorySelect = random.Next(0,player.controlledTerritories.Count);
            Territory chosenTerritory = player.controlledTerritories[territorySelect];
            chosenTerritory.OnMouseDown();
            chosenTerritory.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

            //Deploy troops onto selected territory
            int numberOfTerritoriesToDeploy = random.Next(1,turn.deployableTroops);
            for (int i = 0; i < numberOfTerritoriesToDeploy; i++)
            {
                await Task.Delay(timeBetweenActions/2);
                turn.arrowUp.GetComponent<ReinforcementScript>().OnMouseDown();
            }

            //Select deploy button
            await Task.Delay(timeBetweenActions);
            turn.deployButton.GetComponent<ReinforcementScript>().OnMouseDown();
            chosenTerritory.gameObject.GetComponent<SpriteRenderer>().color = player.playerColor;
        }

        //Press attack phase button
        await Task.Delay(timeBetweenActions);
        turn.attackButton.onClick.Invoke();
    }

    async Task AIAttack()
    {
        //Select a random territory with army value larger than 1 and neighbours not controlled by this
        System.Random random = new System.Random();
        int counterValue = 0;
        Territory chosenTerritory = null;
        bool canSelect = false;
        foreach (Territory territory in player.controlledTerritories)
        {
            foreach (Territory neighbour in territory.neighbourTerritories)
            {
                if (neighbour.controlledBy != player && territory.counter.troopCount > 1)
                {
                    canSelect = true;
                }
            }
        }
        while (counterValue <= 1 && canSelect)
        {
            int territorySelect = random.Next(0,player.controlledTerritories.Count);
            chosenTerritory = player.controlledTerritories[territorySelect];
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

            //Choose a territory to attack and select it
            List<Territory> options = new List<Territory>();
            foreach (Territory neighbour in chosenTerritory.neighbourTerritories)
            {
                if(neighbour.controlledBy != player)
                {
                    options.Add(neighbour);
                }
            }
            int chosenNeighbour = random.Next(0,options.Count);
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
                int diceNumChoice2 = random.Next(1,turn.numberOfDefenseDice);
                turn.defenseDiceNumbers[diceNumChoice2].onClick.Invoke();
            }
            else
            {
                //while(!turn.isDefenseDiceSelected){}
                Debug.Log("after");
            }
        }

        await Task.Delay(timeBetweenActions);
        turn.fortifyButton.onClick.Invoke();
    }

    async Task AIFortify()
    {
        await Task.Delay(timeBetweenActions);
        turn.endTurnButton.onClick.Invoke();
    }

}
