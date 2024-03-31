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
    public int attackerDiceNum = 1,defenderDiceNum = 1;
    private void Start() 
    {
        turn = GameObject.Find("Turn").GetComponent<Turn>();
    }
    public int RollDice(int numberOfDice)
    {
        int total = 0;
        System.Random rand = new System.Random();

        for (int i = 0; i < numberOfDice; i++)
        {
            int value = rand.Next(1, 7);
            total += value;
            if(!isRolled)
            {
                SetDiceSprite(value);
            }
            
        }
        return total;
    }

    public void ResolveAttack()
    {
        if(!isRolled)
        {
            int[] attackerRolls = new int[attackerDiceNum];
            int[] defenderRolls = new int[defenderDiceNum];
            
            for (int i = 0; i < attackerDiceNum; i++)
            {
                attackerRolls[i] = RollDice(1);
            }
            
            for (int i = 0; i < defenderDiceNum; i++)
            {
                defenderRolls[i] = RollDice(1);
            }
            
            Array.Sort(attackerRolls);
            Array.Sort(defenderRolls);
            Array.Reverse(attackerRolls);
            Array.Reverse(defenderRolls);
            
            int comparisonCount = Math.Min(attackerDiceNum, defenderDiceNum);
            
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

            int result1 = attackerRolls[0];
            int result2 = defenderRolls[0];
            
            Debug.Log($"Attacker: {result1} , Defender: {result2}");
            Debug.Log($"Attacker losses: {attackerLosses} | Defender losses: {defenderLosses}");

            if(defenderLosses > attackerLosses)
            {
                //Defender loses
            }
            else
            {
                //Attacker loses
            }
        }
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

