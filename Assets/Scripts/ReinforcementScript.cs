using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages the reinforcement actions for counters during the reinforcement phase of the game. 
/// This includes adding, removing, and deploying reinforcements to and from a counter.
/// </summary>
public class ReinforcementScript : MonoBehaviour
{
    /// <summary>
    /// The counter object to which reinforcements will be added or removed.
    /// </summary>
    public Counter counter;

    /// <summary>
    /// The number of reinforcements available to be deployed to counters.
    /// The initial number of troops in the counter at the start of the reinforcement phase.
    /// The total number of deployed troops during the reinforcement phase.
    /// </summary>
    
    public int availableReinforcements,initialCounterNum,deployedTroops;

    /// <summary>
    /// Reference to the current turn manager.
    /// </summary>
    public Turn turn;

    /// <summary>
    /// The GameObject representing the button to increase, decrease and deploy reinforcements.
    /// </summary>
    public GameObject upButton,downButton,deployButton;
    private void Start() 
    {
        upButton = GameObject.Find("UpButton");
        downButton = GameObject.Find("DownButton");
        deployButton = GameObject.Find("DeployButton");
    }

     /// <summary>
    /// Handles click events on this GameObject, performing different actions based on the object's name.
    /// Actions include incrementing or decrementing troop count, and deploying troops.
    /// </summary>
    private void OnMouseDown() 
    {
        if (turn.turnMode == "Reinforcement")
        {
            if(name == "UpButton")
            {
                if(availableReinforcements > 0){
                    counter.troopCount++;
                    counter.textMesh.text = counter.troopCount.ToString();
                    availableReinforcements--;
                    downButton.GetComponent<ReinforcementScript>().availableReinforcements --;
                    deployButton.GetComponent<ReinforcementScript>().availableReinforcements --;

                    deployedTroops++;
                    downButton.GetComponent<ReinforcementScript>().deployedTroops++;
                    deployButton.GetComponent<ReinforcementScript>().deployedTroops++;
                }
            }
            else if(name == "DownButton")
            {
                if(counter.troopCount > initialCounterNum){
                    counter.troopCount--;
                    counter.textMesh.text = counter.troopCount.ToString();
                    availableReinforcements++;
                    upButton.GetComponent<ReinforcementScript>().availableReinforcements ++;
                    deployButton.GetComponent<ReinforcementScript>().availableReinforcements ++;

                    deployedTroops--;
                    upButton.GetComponent<ReinforcementScript>().deployedTroops--;
                    deployButton.GetComponent<ReinforcementScript>().deployedTroops--;
                }
            }
            else if(name == "DeployButton")
            {
                upButton.GetComponent<SpriteRenderer>().enabled = false;
                downButton.GetComponent<SpriteRenderer>().enabled = false;
                deployButton.GetComponent<SpriteRenderer>().enabled = false;
                
                turn.deployableTroops -= deployedTroops;
                deployedTroops = 0;
            }
            else{Debug.Log("Button not found");}
        }
    }
}
