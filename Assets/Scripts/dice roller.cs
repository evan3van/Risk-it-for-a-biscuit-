using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Controls the dice rolling logic for attacks within the game, including visual representation and outcome determination.
/// This script is attatched to each attacker dice object in the scene
/// </summary>
public class DiceRoller : MonoBehaviour
{
    /// <summary>
    /// A list of sprites representing the faces of a dice
    /// </summary>
    public List<Sprite> diceSprites;

    /// <summary>
    /// Reference to the current turn manager
    /// </summary>
    public Turn turn;

    /// <summary>
    /// The attacking player
    /// The defending player
    /// </summary>
    public Player attacker,defender;

    /// <summary>
    /// Indicates if this object has been rolled
    /// </summary>
    public bool isRolled = false;

    /// <summary>
    /// The number of dice the attacker can roll
    /// The number of dice the defender can roll
    /// </summary>
    public int attackerDiceNum = 1,defenderDiceNum = 1;

    /// <summary>
    /// Indicates if this dice has been iterated over
    /// </summary>
    public bool isChecked = false;

    /// <summary>
    /// Initializes the turn by finding the relevant GameObject in the scene
    /// </summary>
    private void Start() 
    {
        turn = GameObject.Find("Turn").GetComponent<Turn>();
    }

    /// <summary>
    /// Rolls a specified number of dice and returns the total value
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
        }
        return total;
    }

    /// <summary>
    /// Resolves an attack between the attacker and defender, determining losses based on dice rolls
    /// </summary> 
    public void ResolveAttack()
    {
        if(!isRolled)
        {
            int result1 = RollDice(1);
            SetDiceSprite(result1);

            Debug.Log($"Attacker: {result1}");
        }
    }

    /// <summary>
    /// Updates the visual representation of the dice based on the roll value.
    /// </summary>
    /// <param name="roll">The value rolled, used to select the corresponding dice sprite</param>
    public void SetDiceSprite(int roll)
    {
        if(!isRolled)
        {
            GetComponent<Image>().sprite = diceSprites[roll-1];
            isRolled = true;
            turn.numberOfRolledDice++;
            turn.CheckIfDiceRolled(roll);
        }
    }

    /// <summary>
    /// Set the boolean isRolled value via external method call
    /// </summary>
    public void SetIsRolled(bool Rolled)
    {
        isRolled = Rolled;
    }
}

