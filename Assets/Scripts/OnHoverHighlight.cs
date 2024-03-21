using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoverHighlight : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color origionalColor,newColor;
    public Player player;
    public Turn currentTurn;
    public string mode = "Default";
    void Start()
    {
        newColor = Color.red;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
    private void OnMouseExit() 
    {
        spriteRenderer.color = origionalColor;
    }
}
