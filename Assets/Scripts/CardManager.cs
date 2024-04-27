using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CardManager : MonoBehaviour
{
    
    public Turn turn;
    public List<Card> selectedCards = new();
    public List<TextMeshProUGUI> selectedCardsText;
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
    }


    public List <GameObject> cardList;

    public List <GameObject> displayCards;

    public void CardSelect(UnityEngine.UI.Image image)
    {
        Sprite imageSprite = image.sprite;
        bool select = true;
        foreach (GameObject card in cardList)
        {
            if(card.GetComponent<SpriteRenderer>().sprite == imageSprite && selectedCards.Count < 3)
            {
                foreach (Card selectedCard in selectedCards)
                {
                    if (selectedCard == card.GetComponent<Card>())
                    {
                        select = false;
                    }
                }
                if (select)
                {
                    image.color = Color.red;
                    selectedCards.Add(card.GetComponent<Card>());
                }
                else
                {
                    image.color = Color.white;
                    selectedCards.Remove(card.GetComponent<Card>());
                }
            }
            else if (selectedCards.Count >=3)
            {
                image.color = Color.red;
                selectedCards.RemoveAt(2);
                selectedCards.Add(card.GetComponent<Card>());
            }
        }
    }
}
