using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DiceRoller : MonoBehaviour
{
    public List<Sprite> diceSprites;
    public GameObject dice;
    public Turn turn;
    public Player attacker,defender;
    public bool isRolled = false;
    private void Start() 
    {
        turn = GameObject.Find("Turn").GetComponent<Turn>();
    }
    public void RollDice(int numberOfDice)
    {
        if(!isRolled)
        {
            System.Random rand = new System.Random();
            int total = 0;
            for (int i = 0; i < numberOfDice; i++)
            {
                int value = rand.Next(1, 7);
                total += value;
                SetDiceSprite(value);
                Debug.Log($"Random int: {total}");
            }
        }
    }

    public void ResolveAttack(int attackerDice, int defenderDice)
    {
        int[] attackerRolls = new int[attackerDice];
        int[] defenderRolls = new int[defenderDice];
        
        for (int i = 0; i < attackerDice; i++)
        {
            //attackerRolls[i] = RollDice(1);
        }
        
        for (int i = 0; i < defenderDice; i++)
        {
            //defenderRolls[i] = RollDice(1);
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

    public void SetDiceSprite(int rollValue)
    {
        if(!isRolled)
        {
            dice.GetComponent<Image>().sprite = diceSprites[rollValue-1];
            isRolled = true;
        }
    }
}

