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
    /// Values for tracking the state of the counters/deployables
    /// </summary>
    
    public int availableReinforcements,initialCounterNum,deployedTroops;

    /// <summary>
    /// Reference to the current turn manager.
    /// </summary>
    public Turn turn;

    /// <summary>
    /// The GameObjects representing the button to increase, decrease and deploy reinforcements.
    /// </summary>
    public GameObject upButton,downButton,deployButton;

    /// <summary>
    /// Sets the up, down and deploy buttons
    /// </summary>
    private void Start() 
    {
        upButton = GameObject.Find("UpButton");
        downButton = GameObject.Find("DownButton");
        deployButton = GameObject.Find("DeployButton");
    }

    /// <summary>
    /// Handles click events on this GameObject, performing different actions based on the object's name.
    /// Actions include incrementing or decrementing troop count, and deploying troops.
    /// Performs similarly for if the turn mode is "Reinforcement" or "Fortify", however additional checks are made if in the fortify phase
    /// </summary>
    public void OnMouseDown() 
    {
        if (turn.turnMode == "Reinforcement" | turn.turnMode == "Fortify" && turn.deployableTroops > 0)
        {
            
            turn.chooseSender.SetActive(false);
            turn.sendTo.SetActive(false);
            turn.territoryInteractToggle = false;
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
                turn.reinforcementUINumber.text = turn.deployableTroops.ToString();

                if (turn.turnMode == "Fortify")
                {
                    turn.previousSelected.counter.troopCount -= deployedTroops;
                    turn.previousSelected.counter.UpdateCount(turn.previousSelected.counter.troopCount);
                }
                turn.territoryInteractToggle = true;
                if (turn.turnMode == "Fortify")
                {
                    turn.territoryInteractToggle = false;
                }
                if (turn.turnMode == "Reinforcement" && turn.deployableTroops == 0)
                {
                    turn.territoryInteractToggle = false;
                }
                deployedTroops = 0;
            }
            else{Debug.Log("Button not found");}
        }
    }
}
