using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using TMPro;

public class Territory : MonoBehaviour
{
    public Player controlledBy = null;
    public Turn turn;
    public List<Territory> neighbourTerritories = new();   //Gonna need to set this
    public GameObject arrowPrefab;
    public bool isArrowsActive = false;
    public Counter counter;
    public GameObject upButton,downButton,deployButton;
    public ReinforcementScript arrowUp,arrowDown;
    private void Start() 
    {
        counter = transform.GetChild(0).GetComponent<Counter>();
        upButton = GameObject.Find("UpButton");
        downButton = GameObject.Find("DownButton");
        deployButton = GameObject.Find("DeployButton");
        arrowUp = upButton.GetComponent<ReinforcementScript>();
        arrowDown = downButton.GetComponent<ReinforcementScript>();
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
                upButton.gameObject.SetActive(true);
                downButton.gameObject.SetActive(true);
                deployButton.gameObject.SetActive(true);
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
        foreach (Transform arrow in transform)
        {
            arrow.gameObject.SetActive(false);
        }
    }
}
 