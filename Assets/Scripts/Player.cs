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

    /// <summary>
    /// Reference to the card manager
    /// </summary>
    public CardManager cardManager;

    /// <summary>
    /// Reference to the turn
    /// </summary>
    public Turn turn;

    /// <summary>
    /// Sets a check for if the player is AI
    /// </summary>
    public bool IsAI = false;

    /// <summary>
    /// Any added on reinforcements from trading in cards
    /// </summary>
    public int extraReinforcements = 0;

    /// <summary>
    /// This player's AI behaviour script (if it is an AI)
    /// </summary>
    public AIBehavior aIBehavior;

    /// <summary>
    /// This player's mission card
    /// </summary>
    public Sprite missionCard;

    /// <summary>
    /// The player's customised name
    /// </summary>
    public string myName;

    /// <summary>
    /// Initialising the turn and card manager references
    /// </summary>
    private void Start() 
    {
        turn = GameObject.Find("Turn").GetComponent<Turn>();
        cardManager = turn.cardManager;
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
    }

    /// <summary>
    /// Forces the player to trade in their cards if possible, if not removes a card from their list
    /// </summary>
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

    /// <summary>
    /// Trades in the player's cards and removes them from their list of cards.
    /// </summary>
    /// <param name="card1">Card to be removed</param>
    /// <param name="card2">Card to be removed</param>
    /// <param name="card3">Card to be removed</param>
    public void TradeInCards(Card card1,Card card2,Card card3)
    {
        turn.numOfTradedInSets++;
        cards.Remove(card1.gameObject);
        cards.Remove(card2.gameObject);
        cards.Remove(card3.gameObject);
        cardManager.selectedCards.Clear();
        if(turn.numOfTradedInSets <= 6)
        {
            extraReinforcements += turn.tradedInSetReinforcements[turn.numOfTradedInSets];
        }
        else if(turn.numOfTradedInSets > 6)
        {
            extraReinforcements += 15+((turn.numOfTradedInSets-6)*5);
        }
    }

    public void CompleteMission(){}
}
