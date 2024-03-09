using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReinforcementScript : MonoBehaviour
{
    public Counter counter;
    public int availableReinforcements;
    public Turn turn;
    public int initialCounterNum;
    public GameObject upButton,downButton,deployButton;
    private void Start() 
    {
        upButton = GameObject.Find("UpButton");
        downButton = GameObject.Find("DownButton");
        deployButton = GameObject.Find("DeployButton");
        availableReinforcements = turn.deployableTroops;
    }

    private void OnMouseDown() 
    {
        if(name == "UpButton")
        {
            if(turn.deployableTroops > 0){
                counter.troopCount++;
                counter.textMesh.text = counter.troopCount.ToString();
                turn.deployableTroops--;
            }
        }
        else if(name == "DownButton")
        {
            if(counter.troopCount > initialCounterNum){
                counter.troopCount--;
                counter.textMesh.text = counter.troopCount.ToString();
                turn.deployableTroops++;
            }
        }
        else if(name == "DeployButton")
        {
            upButton.gameObject.SetActive(false);
            downButton.gameObject.SetActive(false);
            deployButton.gameObject.SetActive(false);
        }
        else{Debug.Log("Button not found");}
    }
}
