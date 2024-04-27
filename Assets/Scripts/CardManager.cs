using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CardManager : MonoBehaviour
{
    
    public Turn turn;
    public List<Card> selectedCards = new();
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
}
