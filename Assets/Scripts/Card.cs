using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Card : MonoBehaviour
{
    
    public Turn turn;
    public void GoTocardMenu()
    {
        SceneManager.LoadScene("cardMenu");
       
       for (int i = 0; i < displayCards.Count; i++)
       {
          displayCards[i].GetComponent<UnityEngine.UI.Image>().sprite = turn.myTurn.cards[i].gameObject.GetComponent<SpriteRenderer>().sprite;
       }
    }

    public void GoTocanvasMenu()
    {
        SceneManager.LoadScene("canvas");
    }


    public List <GameObject> cardList;

    public List <GameObject> displayCards;

    

    
    

}
