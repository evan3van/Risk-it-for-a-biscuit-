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
    public List<Territory> neighbourTerritories = new();
    public GameObject arrowPrefab;
    public bool isArrowsActive = false;
    public Counter counter;
    public GameObject upButton,downButton,deployButton;
    public ReinforcementScript arrowUp,arrowDown,deployButtonScript;
    public TextMeshPro errorText;
    public List<GameObject> arrows;
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

        arrows = new List<GameObject>();
        DrawArrows();
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

                if(turn.deployableTroops == 0)
                {
                    errorText.gameObject.transform.position = new Vector3(transform.position.x,transform.position.y-50,-6);
                    errorText.text = "No more troops left!";
                    errorText.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            else if(turn.turnMode == "Attack")
            {
                turn.previousSelected = turn.selected;
                turn.selected = this;

                if(turn.previousSelected != this & turn.selected == this & isArrowsActive == false)
                {
                    ShowArrows();
                    ToggleAttackView();
                }
                else if(turn.previousSelected != this & turn.selected == this & isArrowsActive == true)
                {
                    HideArrows();
                    ToggleAttackView();
                }
                else if(turn.previousSelected == this & turn.selected == this & isArrowsActive == false)
                {
                    ShowArrows();
                    ToggleAttackView();
                }
                else if(turn.previousSelected == this & turn.selected == this & isArrowsActive == true)
                {
                    HideArrows();
                    ToggleAttackView();
                }

                if(turn.previousSelected != this & turn.previousSelected != null)
                {
                    turn.previousSelected.HideArrows();
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
            Vector3 direction = neighbour.transform.position - transform.position;

            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            float angleDegrees = angleRadians * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees-45);

            Vector3 arrowPosition = new Vector3(transform.position.x+(direction.x/2),transform.position.y+(direction.y/2),-3f);

            arrow.transform.localScale = new Vector3(0.1f,0.1f,0);

            arrow.transform.position = arrowPosition;

            arrow.name = "Arrow from: "+name+" to: "+neighbour.name;

            arrow.GetComponent<SpriteRenderer>().color = new Color(255,255,255,0.9f);

            arrow.gameObject.SetActive(false);

            arrows.Add(arrow);
            i++;
        }
        
    }

    public void HideArrows()
    {
        Debug.Log("Inactive");
        foreach (GameObject arrow in arrows)
        {
            arrow.gameObject.SetActive(false);
        }
        isArrowsActive = false;
    }

    public void ShowArrows()
    {
        Debug.Log("Active");
        foreach (GameObject arrow in arrows)
        {
            arrow.gameObject.SetActive(true);
        }
        isArrowsActive = true;
    }

    public void ToggleAttackView()
    {
        if(isArrowsActive)
        {
            foreach (Territory neighbour in neighbourTerritories)
            {
                if(neighbour.controlledBy != turn.myTurn)
                {
                    neighbour.EnableAttack();
                }
            }
        }
        else if(!isArrowsActive)
        {
            foreach (Territory neighbour in neighbourTerritories)
            {
                if(neighbour.controlledBy != turn.myTurn)
                {
                    neighbour.DisableAttack();
                }
            }
        }
    }

    public void EnableAttack()
    {

    }

    public void DisableAttack()
    {

    }
}
 