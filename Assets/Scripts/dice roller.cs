using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Controls the dice rolling logic for attacks within the game, including visual representation and outcome determination.
/// </summary>
public class DiceRoller : MonoBehaviour
{
    /// <summary>
    /// A list of sprites representing the faces of a dice.
    /// </summary>
    public List<Sprite> diceSprites;
    
    /// <summary>
    /// The GameObject that visually represents the dice.
    /// </summary>
    public GameObject dice;

    /// <summary>
    /// Reference to the current turn manager.
    /// </summary>
    public Turn turn;

    /// <summary>
    /// The attacking player.
    /// The defending player.
    /// </summary>
    public Player attacker,defender;

    /// <summary>
    /// Indicates if a roll has been made.
    /// </summary>
    public bool isRolled = false;

    /// <summary>
    /// The number of dice the attacker rolls.
    /// The number of dice the defender rolls.
    /// </summary>
    public int attackerDiceNum = 1,defenderDiceNum = 1;

    /// <summary>
    /// Initializes the turn by finding the relevant GameObject in the scene.
    /// </summary>
    private void Start() 
    {
        turn = GameObject.Find("Turn").GetComponent<Turn>();
    }

    /// <summary>
    /// Rolls a specified number of dice and returns the total value.
    /// </summary>
    /// <param name="numberOfDice">The number of dice to roll.</param>
    /// <returns>The total value rolled across all dice.</returns>
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

    /// <summary>
    /// Resolves an attack between the attacker and defender, determining losses based on dice rolls.
    /// </summary> 

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

    /// <summary>
    /// Updates the visual representation of the dice based on the roll value.
    /// </summary>
    /// <param name="rollValue">The value rolled, used to select the corresponding dice sprite.</param>

    public void SetDiceSprite(int rollValue)
    {
        if(!isRolled)
        {
            dice.GetComponent<Image>().sprite = diceSprites[rollValue-1];
            isRolled = true;
        }
    }
}

