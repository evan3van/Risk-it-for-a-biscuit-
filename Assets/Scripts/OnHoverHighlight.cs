using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoverHighlight : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color origionalColor;
    public Color newColor;
    public Player player;
    public Turn currentTurn;
    void Start()
    {
        newColor = Color.red;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter() 
    {
        if(player == currentTurn.myTurn)
        {
            spriteRenderer.color = newColor;
        }
    }
    private void OnMouseExit() 
    {
        spriteRenderer.color = origionalColor;
    }
}
