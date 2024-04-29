using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Include the UI namespace to use UI components
public class PlayerMenuScript : MonoBehaviour
{
    public Slider playerCountSlider;  // Reference to the slider
    public int numOfPlayers;  // To store the number of players

    // This function is called whenever the slider value changes
    public void ValueChangeCheck()
    {
        numOfPlayers = (int)playerCountSlider.value;  // Cast the slider value to an integer
        Debug.Log("Number of Players: " + numOfPlayers);  // Output the number of players
    }

}
