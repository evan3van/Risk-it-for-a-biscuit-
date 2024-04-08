using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the color of a GameObject's sprite when the mouse hovers over it. 
/// The color change depends on the current player's turn, the ownership of the GameObject, and the specified mode.
/// </summary>

public class OnHoverHighlight : MonoBehaviour
{
    /// <summary>
    /// The SpriteRenderer component of the GameObject.
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// The original color of the sprite.
    /// The color to change the sprite to when the mouse hovers over it.
    /// </summary>
    public Color origionalColor,newColor;

    /// <summary>
    /// The player who owns this GameObject.
    /// </summary>
    public Player player;

    /// <summary>
    /// Reference to the current turn manager.
    /// </summary>
    public Turn currentTurn;

    /// <summary>
    /// The mode which determines how the color change behaves. Can be "Default", "Attack", or "Dice".
    /// </summary>
    public string mode = "Default";
    void Start()
    {
        newColor = Color.red;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Changes the sprite's color to <see cref="newColor"/> when the mouse enters the sprite's area, 
    /// based on the current mode and turn conditions.
    /// </summary>
    private void OnMouseEnter() 
    {
        if(player == currentTurn.myTurn & mode == "Default")
        {
            spriteRenderer.color = newColor;
        }
        else if(player != currentTurn.myTurn & mode == "Attack")
        {
            spriteRenderer.color = newColor;
        }
        else if(mode == "Dice")
        {
            spriteRenderer.color = newColor;
        }
    }

    /// <summary>
    /// Resets the sprite's color to <see cref="origionalColor"/> when the mouse leaves the sprite's area.
    /// </summary>
    private void OnMouseExit() 
    {
        spriteRenderer.color = origionalColor;
    }
}
