using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Card : MonoBehaviour
{
    public string type;
    public Player owner;
    public CardManager cardManager;
    private void Start() 
    {
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(2.1f,3.3f);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.05f,0.05f);
    }
    
    private void OnMouseDown() 
    {
        if (cardManager.selectedCards.Count >= 3)
        {
            cardManager.selectedCards.Remove(cardManager.selectedCards[2]);
        }
        cardManager.selectedCards.Add(this);
    }
}
