using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReinforcementScript : MonoBehaviour
{
    public Counter counter;
    public int availableReinforcements,initialCounterNum,deployedTroops;
    public Turn turn;
    public GameObject upButton,downButton,deployButton;
    private void Start() 
    {
        upButton = GameObject.Find("UpButton");
        downButton = GameObject.Find("DownButton");
        deployButton = GameObject.Find("DeployButton");
    }

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
