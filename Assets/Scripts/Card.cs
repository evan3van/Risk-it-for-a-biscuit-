using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Card : MonoBehaviour
{

    public void GoTocardMenu()
    {
        SceneManager.LoadScene("cardMenu");
    }

    public void GoTocanvasMenu()
    {
        SceneManager.LoadScene("canvas");
    }


    public List <GameObject> cardList;

    public List <GameObject> displayCards;

    


}
