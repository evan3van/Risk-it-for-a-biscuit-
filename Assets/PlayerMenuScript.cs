using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  
public class PlayerMenuScript : MonoBehaviour
{
    public Slider playerCountSlider;  
    public int numOfPlayers;  

    
    public void ValueChangeCheck()
    {
        numOfPlayers = (int)playerCountSlider.value;  
        Debug.Log("Number of Players: " + numOfPlayers);  
    }

}
