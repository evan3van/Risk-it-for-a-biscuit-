using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int unitCount;
    public List<Card> cards;
    public List<Territory> controlledTerritories = new List<Territory>();
    public Color playerColor;
    public bool isMyTurn = false;
    public List<Counter> counters = new();
    public Sprite sprite;
    public void GiveCard(Card card)
    {
        //Some code to give the player a card
    }

    public void GiveTroops(int number)
    {
        unitCount += number;
        //More code to make something visually happen here
    }

    public void RemoveTroops(int number)
    {
        unitCount -= number;
        //More code to make something visually happen here
    }
}
