using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages all operations involving cards and the card menu.
/// All methods except start are triggered by buttons in the scene
/// </summary>
public class CardManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the turn script for general turn info
    /// </summary>
    public Turn turn;

    /// <summary>
    /// List of currently selected cards
    /// </summary>
    public List<Card> selectedCards = new();

    /// <summary>
    /// List of currently selected card displays
    /// </summary>
    public List<GameObject> selectedImages;

    /// <summary>
    /// Reference to the trade in button in the scene
    /// </summary>
    public GameObject tradeInButton;

    /// <summary>
    /// List of the deselect buttons
    /// </summary>
    public List<GameObject> deselectButtons;

    /// <summary>
    /// List of the select buttons
    /// </summary>
    public List<GameObject> selectButtons;

    /// <summary>
    /// List of all the mission cards
    /// </summary>
    public UnityEngine.UI.Image missionCard;

    /// <summary>
    /// Setting the card manager reference for each card in the game
    /// </summary>
    private void Start() 
    {
        foreach (GameObject card in cardList)
        {
            card.GetComponent<Card>().cardManager = this;
        }
    }

    /// <summary>
    /// Activated on pressing the card menu button. Displays cards visually and sets up the card menu functionality
    /// </summary>
    public void GoTocardMenu()
    {
       for (int i = 0; i < turn.myTurn.cards.Count; i++)
        {
            Debug.Log("Displaying card "+i);
            selectButtons[i].SetActive(true);
            displayCards[i].SetActive(true);
            displayCards[i].GetComponent<UnityEngine.UI.Image>().sprite = turn.myTurn.cards[i].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        missionCard.sprite = turn.myTurn.missionCard;
    }

    /// <summary>
    /// Hiding the card menu and returning to the main game scene
    /// </summary>
    public void GoTocanvasMenu()
    {
        foreach (GameObject image in displayCards)
        {
            image.SetActive(false);
        }
        foreach (GameObject button in selectButtons)
        {
            button.SetActive(false);
        }
    }

    /// <summary>
    /// List of all cards in the game
    /// </summary>
    public List <GameObject> cardList;

    /// <summary>
    /// List of display placeholders where the cards are displayed
    /// </summary>
    public List <GameObject> displayCards;

    /// <summary>
    /// Visually shows a card as selected and adds it to the selected cards list up to a max of 3 selected cards.
    /// Shows the trade in button if selected cards are eligible to be traded in
    /// </summary>
    /// /// <param name="image">The image to visually set to selected mode</param>
    public void CardSelect(UnityEngine.UI.Image image)
    {
        Sprite imageSprite = image.sprite;
        foreach (GameObject card in cardList)
        {
            if(card.GetComponent<SpriteRenderer>().sprite == imageSprite && selectedCards.Count < 3)
            {
                selectedImages.Add(image.gameObject);
                image.color = Color.red;
                selectedCards.Add(card.GetComponent<Card>());
                break;
            }
        }
        Debug.Log(selectedCards.Count);
        if (selectedCards.Count == 3)
        {
            Card card1 = selectedCards[0];
            Card card2 = selectedCards[1];
            Card card3 = selectedCards[2];
            Debug.Log($"{card1.type},{card2.type},{card3.type}");
            Debug.Log($"{card1.name},{card2.name},{card3.name}");
            if (card1.type == card2.type && card2.type == card3.type)
            {
                tradeInButton.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Removes cards from the selected card list and resets the display for each deselected card.
    /// </summary>
    /// <param name="image">The image to visually reset to unselected mode</param>
    public void CardDeselect(UnityEngine.UI.Image image)
    {
        Sprite imageSprite = image.sprite;
        Card cardToRemove = null;
        foreach (GameObject card in cardList)
        {
            if(card.GetComponent<SpriteRenderer>().sprite == imageSprite && selectedCards.Count > 0)
            {
                selectedImages.Remove(image.gameObject);
                image.color = Color.white;
                cardToRemove = card.GetComponent<Card>();
                break;
            }
        }
        selectedCards.Remove(cardToRemove);
        tradeInButton.SetActive(false);
    }

    /// <summary>
    /// Trades in the selected cards and removes them from the player's hand visually.
    /// </summary>
    public void TradeInPlayerCards()
    {
        Card card1 = selectedCards[0];
        Card card2 = selectedCards[1];
        Card card3 = selectedCards[2];
        List<Card> cardsToRemove = new List<Card>(selectedCards);
        foreach (Card card in cardsToRemove)
        {
            foreach (GameObject imageObject in displayCards)
            {
                bool isSelected = false;
                foreach (GameObject selected in selectedImages)
                {
                    if (selected == imageObject)
                    {
                        isSelected = true;
                        break;
                    }
                }
                UnityEngine.UI.Image image = imageObject.GetComponent<UnityEngine.UI.Image>();
                Sprite imageSprite = image.sprite;
                if(card.gameObject.GetComponent<SpriteRenderer>().sprite == imageSprite && isSelected)
                {
                    image.color = Color.white;
                    image.sprite = null;
                    imageObject.SetActive(false);
                    selectedCards.Remove(card);
                }
            }
        }
        turn.myTurn.TradeInCards(card1,card2,card3);
        selectedCards.Clear();
    }
    
    /// <summary>
    /// Resets the selected cards back to deselected
    /// </summary>
    public void ResetCardSelect()
    {
        foreach (GameObject image in displayCards)
        {
            image.GetComponent<UnityEngine.UI.Graphic>().color = Color.white;
        }
        for (int i = 0; i < selectButtons.Count; i++)
        {
            selectButtons[i].SetActive(true);
            deselectButtons[i].SetActive(false);
        }
        tradeInButton.SetActive(false);
        selectedCards.Clear();
    }
}
