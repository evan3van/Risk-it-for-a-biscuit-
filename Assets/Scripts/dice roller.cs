using System;
using System.Linq;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public int RollDice(int numberOfDice)
    {
        System.Random rand = new System.Random();
        int total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += rand.Next(1, 7);
            Debug.Log($"Random int: {total}");
        }
        return total;
    }

    public void ResolveAttack(int attackerDice, int defenderDice)
    {
        int[] attackerRolls = new int[attackerDice];
        int[] defenderRolls = new int[defenderDice];
        
        for (int i = 0; i < attackerDice; i++)
        {
            attackerRolls[i] = RollDice(1);
        }
        
        for (int i = 0; i < defenderDice; i++)
        {
            defenderRolls[i] = RollDice(1);
        }
        
        Array.Sort(attackerRolls);
        Array.Sort(defenderRolls);
        Array.Reverse(attackerRolls);
        Array.Reverse(defenderRolls);
        
        int comparisonCount = Math.Min(attackerDice, defenderDice);
        
        int attackerLosses = 0;
        int defenderLosses = 0;
        
        for (int i = 0; i < comparisonCount; i++)
        {
            if (attackerRolls[i] > defenderRolls[i])
            {
                defenderLosses++;
            }
            else
            {
                attackerLosses++;
            }
        }
        
        Debug.Log($"Attacker losses: {attackerLosses} | Defender losses: {defenderLosses}");
    }
}

