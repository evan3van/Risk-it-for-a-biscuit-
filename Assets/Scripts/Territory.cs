using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Territory : MonoBehaviour
{
    public Player controlledBy = null;
    public Turn turn;
    public List<Territory> neighbourTerritories = new();   //Gonna need to set this
    public GameObject arrowPrefab;
    public bool isArrowsActive = false;
    public Counter counter;
    public GameObject upButton,downButton,deployButton;
    public ReinforcementScript arrowUp,arrowDown,deployButtonScript;
    public TextMeshPro errorText;
    private void Start() 
    {
        counter = transform.GetChild(0).GetComponent<Counter>();
        upButton = GameObject.Find("UpButton");
        downButton = GameObject.Find("DownButton");
        deployButton = GameObject.Find("DeployButton");
        arrowUp = upButton.GetComponent<ReinforcementScript>();
        arrowDown = downButton.GetComponent<ReinforcementScript>();
        deployButtonScript = deployButton.GetComponent<ReinforcementScript>();
        errorText = GameObject.Find("ErrorText").GetComponent<TextMeshPro>();
    }

    private void OnMouseDown() 
    {
        if(turn.myTurn == controlledBy)
        {
            Debug.Log(name);
            if(turn.turnMode == "Reinforcement")
            {
                downButton.transform.position = new Vector3(transform.position.x+30,transform.position.y-30,-6);
                upButton.transform.position = new Vector3(transform.position.x+30,transform.position.y+30,-6);
                deployButton.transform.position = new Vector3(transform.position.x+50,transform.position.y,-6);
                arrowUp.counter = counter;
                arrowDown.counter = counter;
                deployButtonScript.counter = counter;
                arrowUp.availableReinforcements = turn.deployableTroops;
                arrowDown.availableReinforcements = turn.deployableTroops;
                deployButtonScript.availableReinforcements = turn.deployableTroops;
                arrowUp.initialCounterNum = counter.troopCount;
                arrowDown.initialCounterNum = counter.troopCount;
                deployButtonScript.initialCounterNum = counter.troopCount;
                upButton.GetComponent<SpriteRenderer>().enabled = true;
                downButton.GetComponent<SpriteRenderer>().enabled = true;
                deployButton.GetComponent<SpriteRenderer>().enabled = true;
                if(turn.deployableTroops == 0){
                    errorText.gameObject.transform.position = new Vector3(transform.position.x,transform.position.y-50,-6);
                    errorText.text = "No more troops left!";
                    errorText.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            else if(turn.turnMode == "Attack")
            {
                if(!isArrowsActive)
                {
                    DrawArrows();
                    isArrowsActive = true;
                }
                else if(isArrowsActive)
                {
                    HideArrows();
                    isArrowsActive = false;
                }
            }
        }
    }

    private void DrawArrows()
    {
        
        int i = 1;
        foreach (Territory neighbour in neighbourTerritories)
        {
            GameObject arrow = Instantiate(arrowPrefab,transform.position,quaternion.identity,transform);
            Vector3 difference = neighbour.transform.position - transform.position;

            Vector3 arrowPosition = new Vector3(transform.position.x+(difference.x/2),transform.position.y+(difference.y/2),-3f);
            arrow.transform.localScale = new Vector3(0.1f,0.1f,0);
            arrow.transform.position = arrowPosition;
            arrow.name = "Arrow from: "+name+" to: "+neighbour.name;
            i++;
        }
        
    }

    private void HideArrows()
    {
        
    }
}
 