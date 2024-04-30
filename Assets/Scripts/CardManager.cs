using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CardManager : MonoBehaviour
{
    
    public Turn turn;
    public List<Card> selectedCards = new();
    public List<GameObject> selectedImages;
    public GameObject tradeInButton;
    public List<GameObject> deselectButtons;
    public List<GameObject> selectButtons;
    private void Start() 
    {
        foreach (GameObject card in cardList)
        {
            card.GetComponent<Card>().cardManager = this;
        }
    }
    public void GoTocardMenu()
    {
       for (int i = 0; i < turn.myTurn.cards.Count; i++)
        {
            Debug.Log("Displaying card "+i);
            selectButtons[i].SetActive(true);
            displayCards[i].SetActive(true);
            displayCards[i].GetComponent<UnityEngine.UI.Image>().sprite = turn.myTurn.cards[i].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
       
    }

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


    public List <GameObject> cardList;

    public List <GameObject> displayCards;

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
                    selectedCards.Remove(card);
                }
            }
        }
        turn.myTurn.TradeInCards(card1,card2,card3);
        selectedCards.Clear();
    }
    
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
