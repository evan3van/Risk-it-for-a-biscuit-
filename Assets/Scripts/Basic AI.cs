using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    private Player player;
    private Turn turnManager;

    void Start()
    {
        player = GetComponent<Player>();
        turnManager = FindObjectOfType<Turn>();
    }

    void Update()
    {
        if (turnManager.myTurn == player && player.IsAI)
        {
            PerformAITurn();
        }
    }

    void PerformAITurn()
    {
        switch (turnManager.turnMode)
        {
            case "Reinforcement":
                AIReinforce();
                break;
            case "Attack":
                AIAttack();
                break;
            case "Fortify":
                AIFortify();
                break;
        }
    }

   void AIReinforce()
{
    // Example: Distribute troops evenly among all controlled territories.
    int troopsPerTerritory = turnManager.deployableTroops / player.controlledTerritories.Count;
    foreach (var territory in player.controlledTerritories)
    {
        territory.counter.troopCount += troopsPerTerritory;
    }
    // Consider any remaining troops due to rounding.
    int remainingTroops = turnManager.deployableTroops % player.controlledTerritories.Count;
    if (remainingTroops > 0)
    {
        player.controlledTerritories[0].counter.troopCount += remainingTroops;
    }
}

    void AIAttack()
{
    // Example: Attack a random neighboring territory with fewer troops.
    foreach (var territory in player.controlledTerritories)
    {
        foreach (var neighbor in territory.neighbourTerritories)
        {
            if (neighbor.controlledBy != player && territory.counter.troopCount > neighbor.counter.troopCount)
            {
                // Simulate an attack...
                // This is a placeholder; you'd implement the logic to execute an attack here.
                return; // Exit after one attack for simplicity.
            }
        }
    }
}

    void AIFortify()
{
    // Example: Move some troops from the territory with the most troops to the one with the least.
    Territory mostTroops = player.controlledTerritories.OrderByDescending(t => t.counter.troopCount).FirstOrDefault();
    Territory leastTroops = player.controlledTerritories.OrderBy(t => t.counter.troopCount).FirstOrDefault();
    if (mostTroops != null && leastTroops != null && mostTroops.counter.troopCount > leastTroops.counter.troopCount + 1)
    {
        // Move a troop from mostTroops to leastTroops.
        mostTroops.counter.troopCount -= 1;
        leastTroops.counter.troopCount += 1;
    }
}
}
