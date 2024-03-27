using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;

public class Territory : MonoBehaviour
{
    public Player controlledBy = null;
    public Turn turn;
    public List<Territory> neighbourTerritories = new();
    public GameObject arrowPrefab,attackUI;
    public bool isArrowsActive,attackUIIsActive = false;
    public Counter counter;
    public GameObject upButton,downButton,deployButton;
    public ReinforcementScript arrowUp,arrowDown,deployButtonScript;
    public TextMeshPro errorText;
    public List<GameObject> arrows;
    public int attackTroops;
    public Territory attackTarget;
    private TextMeshProUGUI attackTargetText;
    public Color oldColor;
    private void Start() 
    {
        counter = transform.GetChild(0).GetComponent<Counter>();
        upButton = turn.arrowUp;
        downButton = turn.arrowDown;
        deployButton = turn.deployButton;
        arrowUp = upButton.GetComponent<ReinforcementScript>();
        arrowDown = downButton.GetComponent<ReinforcementScript>();
        deployButtonScript = deployButton.GetComponent<ReinforcementScript>();
        errorText = GameObject.Find("ErrorText").GetComponent<TextMeshPro>();
        oldColor = GetComponent<OnHoverHighlight>().origionalColor;
        attackUI = turn.attackUI;


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
                attackTarget = turn.attackTarget;

                if(turn.previousSelected != this & turn.selected == this & isArrowsActive == false)
                {
                    ShowArrows();
                    EnableAttackHighlight();
                }
                else if(turn.previousSelected != this & turn.selected == this & isArrowsActive == true)
                {
                    HideArrows();
                    DisableAttackHighlight();
                    turn.selected = null;
                    if(turn.attackUIActive)
                    {
                        ToggleAttackUI();
                        turn.attackUIActive = false;
                    }
                }
                else if(turn.previousSelected == this & turn.selected == this & isArrowsActive == false)
                {
                    ShowArrows();
                    EnableAttackHighlight();
                }
                else if(turn.previousSelected == this & turn.selected == this & isArrowsActive == true)
                {
                    HideArrows();
                    DisableAttackHighlight();
                    turn.selected = null;
                    if(turn.attackUIActive)
                    {
                        ToggleAttackUI();
                        turn.attackUIActive = false;
                    }
                }

                if(turn.previousSelected != this & turn.previousSelected != null)
                {
                    turn.previousSelected.HideArrows();
                    turn.previousSelected.DisableAttackHighlight();
                }
            }
        }
        
        if (turn.selected != null & turn.turnMode == "Attack")
        {
            foreach (Territory neighbour in turn.selected.neighbourTerritories)
            {
                if (neighbour == this)
                {
                    Debug.Log($"Attack: {this}");
                    turn.attackTarget = neighbour;
                    if(turn.isAttackHighlighted == false)
                    {
                        gameObject.GetComponent<OnHoverHighlight>().origionalColor = Color.red;
                        turn.isAttackHighlighted = true;
                    }
                    else
                    {
                        gameObject.GetComponent<OnHoverHighlight>().origionalColor = oldColor;
                        turn.isAttackHighlighted = false;
                    }
                    attackTarget = turn.attackTarget;
                    string attackTargetName = attackTarget.name;
                    attackTargetText = turn.attackTargetText;
                    if(attackUIIsActive == false)
                    {
                        ToggleAttackUI();
                    }
                    turn.attacker = turn.selected;
                    attackTargetText.text = "Target = "+attackTargetName;
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
        //Debug.Log("Inactive");
        foreach (GameObject arrow in arrows)
        {
            arrow.gameObject.SetActive(false);
        }
        isArrowsActive = false;
    }

    public void ShowArrows()
    {
        //Debug.Log("Active");
        foreach (GameObject arrow in arrows)
        {
            arrow.gameObject.SetActive(true);
        }
        isArrowsActive = true;
    }

    public void EnableAttackHighlight()
    {
        foreach (Territory neighbour in neighbourTerritories)
        {
            neighbour.gameObject.GetComponent<OnHoverHighlight>().mode = "Attack";
        }
    }

    public void DisableAttackHighlight()
    {
        foreach (Territory neighbour in neighbourTerritories)
        {
            neighbour.gameObject.GetComponent<OnHoverHighlight>().mode = "Default";
        }
    }

    public void ToggleAttackUI()
    {
        if (attackUI.activeSelf == true)
        {
            attackUI.SetActive(false);
            turn.attackUIActive = false;
        }
        else
        {
            attackUI.SetActive(true);
            turn.attackUIActive = true;
        }
    }
}
 