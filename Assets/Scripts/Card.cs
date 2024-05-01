using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Card : MonoBehaviour
{
    /// <summary>
    /// The type of card (cavalry/infantry/artillery)
    /// </summary>
    public string type;

    /// <summary>
    /// A reference to the card manager script
    /// </summary>
    public CardManager cardManager;

    /// <summary>
    /// Setting a size for the box collider of the card
    /// </summary>
    private void Start() 
    {
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(2.1f,3.3f);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.05f,0.05f);
    }
}
