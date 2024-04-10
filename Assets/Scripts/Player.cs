using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a player in the game, including their units, controlled territories, and available cards.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// The current count of units that the player has.
    /// </summary>
    public int unitCount;
    
    /// <summary>
    /// The cards currently held by the player.
    /// </summary>
    public List<Card> cards;

    /// <summary>
    /// The territories currently controlled by the player.
    /// </summary>
    public List<Territory> controlledTerritories = new List<Territory>();

    /// <summary>
    /// The color associated with the player, used for UI and game object identification.
    /// </summary>
    public Color playerColor;

    /// <summary>
    /// Indicates whether it is currently this player's turn.
    /// </summary>
    public bool isMyTurn = false;

    /// <summary>
    /// Counters associated with the player, representing armies or other markers on the game board.
    /// </summary>
    public List<Counter> counters = new();

    /// <summary>
    /// The sprite used to represent the player visually in the game.
    /// </summary>
    public Sprite sprite;

    /// <summary>
    /// Adds a card to the player's hand.
    /// </summary>
    /// <param name="card">The card to be given to the player.</param>
    public void GiveCard(Card card)
    {
        //Some code to give the player a card
    }
    
    /// <summary>
    /// Increases the player's unit count by a specified number.
    /// </summary>
    /// <param name="number">The number of units to be added.</param>
    public void GiveTroops(int number)
    {
        unitCount += number;
        //More code to make something visually happen here
    }
    
    /// <summary>
    /// Decreases the player's unit count by a specified number.
    /// </summary>
    /// <param name="number">The number of units to be removed.</param>
    public void RemoveTroops(int number)
    {
        unitCount -= number;
        //More code to make something visually happen here
    }

    public bool IsAI = false;
}
