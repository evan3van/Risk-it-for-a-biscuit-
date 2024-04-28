using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        //Select a random territory
        while(turn.deployableTroops > 0){
            await Task.Delay(timeBetweenActions);
            System.Random random = new System.Random();
            int territorySelect = random.Next(0,player.controlledTerritories.Count);
            Territory chosenTerritory = player.controlledTerritories[territorySelect];
            chosenTerritory.OnMouseDown();

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
        }

        //Press attack phase button
        await Task.Delay(timeBetweenActions);
        turn.attackButton.onClick.Invoke();
    }

    async Task AIAttack()
    {
        await Task.Delay(timeBetweenActions);
        turn.fortifyButton.onClick.Invoke();
    }

    async Task AIFortify()
    {
        await Task.Delay(timeBetweenActions);
        turn.endTurnButton.onClick.Invoke();
    }
}
