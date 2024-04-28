using System;
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
    public List<GameObject> cards = new();

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
    public CardManager cardManager;
    public Turn turn;
    public bool IsAI = false;
    public int extraReinforcements = 0;
    public AIBehavior aIBehavior;

    private void Start() 
    {
        turn = GameObject.Find("Turn").GetComponent<Turn>();
        cardManager = turn.cardManager;
        if(playerColor == Color.magenta)
        {
            IsAI = true;
            aIBehavior = gameObject.AddComponent<AIBehavior>();
        }
    }

    /// <summary>
    /// Adds a card to the player's hand.
    /// </summary>
    public void GiveCard()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(0,cardManager.cardList.Count);
        if (cards.Count < 6)
        {
            cards.Add(cardManager.cardList[randomNumber]);
        }
        else
        {
            ForceTradeInCards();
        }
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

    public void ForceTradeInCards()
    {
        bool canTradeInCards = false;
        List<int> cardTypeCount = new()
        {
            0,
            0,
            0,
            0
        };
        List<string> types = new()
        {
            "Infantry",
            "Cavalry",
            "Artillery",
            "Wild"
        };
        foreach (GameObject card in cards)
        {
            Card cardScript = card.GetComponent<Card>();
            if (cardScript.type == types[0])
            {
                cardTypeCount[0]++;
            }
            else if (cardScript.type == types[1])
            {
                cardTypeCount[1]++;
            }
            else if (cardScript.type == types[2])
            {
                cardTypeCount[2]++;
            }
            else if (cardScript.type == types[3])
            {
                cardTypeCount[3]++;
            }
        }
        string tradeInType = "";
        for (int i = 0; i < 4; i++)
        {
            if (cardTypeCount[i] >= 3)
            {
                canTradeInCards=true;
                tradeInType = types[i];
                break;
            }
        }

        if (canTradeInCards)
        {
            List<Card> tradeInCards = new();
            foreach (GameObject card in cards)
            {
                if((card.GetComponent<Card>().type == tradeInType | card.GetComponent<Card>().type == types[3]) && tradeInCards.Count < 3)
                {
                    tradeInCards.Add(card.GetComponent<Card>());
                }
            }
            Debug.Log("Cards traded in");
            TradeInCards(tradeInCards[0],tradeInCards[1],tradeInCards[2]);
        }
        else
        {
            Debug.Log("Removing last card");
            cards.Remove(cards[5]);
        }
    }

    public void TradeInCards(Card card1,Card card2,Card card3)
    {
        turn.numOfTradedInSets++;
        cards.Remove(card1.gameObject);
        cards.Remove(card2.gameObject);
        cards.Remove(card3.gameObject);
        if(turn.numOfTradedInSets <= 6)
        {
            extraReinforcements += turn.tradedInSetReinforcements[turn.numOfTradedInSets];
        }
        else if(turn.numOfTradedInSets > 6)
        {
            extraReinforcements += 15+((turn.numOfTradedInSets-6)*5);
        }
    }
}
