using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;
using Unity.VisualScripting;

/// <summary>
/// Represents a territory in the game, managing its state, interactions, and UI elements related to attack and reinforcement phases.
/// </summary>
public class Territory : MonoBehaviour
{
    /// <summary>
    /// The player that currently controls this territory.
    /// </summary>
    public Player controlledBy = null;

    /// <summary>
    /// Reference to the current turn manager.
    /// </summary>
    public Turn turn;

    /// <summary>
    /// List of neighboring territories to this one, enabling attack and reinforcement strategies.
    /// </summary>
    public List<Territory> neighbourTerritories = new();

    /// <summary>
    /// Prefab for the arrow indicator used during the attack phase to show possible attack directions.
    /// UI element for managing attack actions.
    /// </summary>
    public GameObject arrowPrefab,attackUI;

    /// <summary>
    /// Indicates if arrow indicators for attacking are currently active.
    /// </summary>
    public bool isArrowsActive,attackUIIsActive = false;

    /// <summary>
    /// The counter associated with this territory, representing the number of troops or units.
    /// </summary>
    public Counter counter;
    // UI elements for reinforcement actions
    public GameObject upButton,downButton,deployButton;
    // Scripts attached to the UI elements for easier access
    public ReinforcementScript arrowUp,arrowDown,deployButtonScript;

    /// <summary>
    /// Text displayed for errors, such as when no more troops can be deployed.
    /// </summary>
    public TextMeshPro errorText;

    /// <summary>
    /// Collection of arrows instantiated to indicate possible attack directions.
    /// </summary>
    public List<GameObject> arrows;

    /// <summary>
    /// Number of troops allocated for an attack from this territory.
    /// </summary>
    public int attackTroops;

    /// <summary>
    /// The target territory for an attack.
    /// </summary>
    public Territory attackTarget;

    // Text elements in the UI for displaying attack targets and instructions
    private TextMeshProUGUI attackTargetText,attackButtonText;

    /// <summary>
    /// The original color of the territory before any highlight changes.
    /// </summary>
    public Color oldColor;

    /// <summary>
    /// Initialises references to the buttons and UI in each mode and creates arrows for this territory
    /// </summary>
    private void Start() 
    {
        counter = transform.GetChild(0).GetComponent<Counter>();
        upButton = turn.arrowUp;
        downButton = turn.arrowDown;
        deployButton = turn.deployButton;
        arrowUp = upButton.GetComponent<ReinforcementScript>();
        arrowDown = downButton.GetComponent<ReinforcementScript>();
        deployButtonScript = deployButton.GetComponent<ReinforcementScript>();
        oldColor = GetComponent<OnHoverHighlight>().origionalColor;
        attackUI = turn.attackUI;
        attackButtonText = turn.attackButtonText;

        errorText = turn.errorText.GetComponent<TextMeshPro>();
        arrows = new List<GameObject>();
        DrawArrows();
    }

    /// <summary>
    /// Performs an action based on the turn mode and if the territories can be interacted with or not.
    /// If the mode is <see cref="Reinforcement"/>, the reinforcement UI is set active and troops can be deployed onto the territory.
    /// If the mode is <see cref="Attack"/>, the arrows are drawn from this territory if it has been initially selected as the attacker and an
    /// attack target can be selected, if this is the target, the attack button UI is toggled to trigger an attack.
    /// If the mode is <see cref="Fortify"/>, checks are performed to determine if the territory is able to fortify or not
    /// and if so arrows are drawn to neighbours which can then recieve troops depending on the available troops on this territory
    /// </summary>
    public void OnMouseDown() 
    {
        if(turn.territoryInteractToggle)
        {
            if(turn.myTurn == controlledBy)
            {
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
                    turn.territoryInteractToggle = false;

                    if(turn.deployableTroops == 0)
                    {
                        Debug.Log("No troops remain");
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
                else if(turn.turnMode == "Fortify")
                {
                    turn.sendTo.GetComponent<TextMeshProUGUI>().text = "Choose a territory to send troops to";
                    if (turn.selected != this && counter.troopCount > 1)
                    {
                        turn.previousSelected = turn.selected;
                        turn.selected = this;
                    }
                    if (turn.selected != null && turn.selected.counter.troopCount > 1)
                    {
                        foreach (Territory neighbour in turn.selected.neighbourTerritories)
                        {
                            if (neighbour == this)
                            {
                                turn.deployableTroops = turn.selected.counter.troopCount-1;
                                turn.previousSelected = turn.selected;
                                turn.previousSelected.HideArrows();
                                turn.selected = this;
                                turn.fortifyUI.SetActive(true);
                                turn.territoryInteractToggle=false;
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
                            }
                        }
                    }
                    if (!turn.fortifyUI.activeSelf)
                    {
                        turn.chooseSender.SetActive(false);
                        turn.sendTo.SetActive(true);

                        if (counter.troopCount > 1)
                        {
                            turn.selected = this;
                            int possibleTerritories = 0;
                            foreach (Territory neighbour in turn.selected.neighbourTerritories)
                            {
                                if(neighbour.controlledBy == controlledBy)
                                {
                                    foreach (GameObject arrow in arrows)
                                    {
                                        if(arrow.name == neighbour.name)
                                        {
                                            arrow.SetActive(true);
                                        }
                                    }
                                    possibleTerritories++;
                                }
                            }
                            if (possibleTerritories == 0)
                            {
                                Debug.Log("No possible territories");
                                turn.sendTo.GetComponent<TextMeshProUGUI>().text = "No possible territories";
                                StartCoroutine(Wait5());
                            }
                        }
                        else
                        {
                            turn.sendTo.GetComponent<TextMeshProUGUI>().text = "Not enough troops";
                            StartCoroutine(Wait5());
                        }
                    }
                    turn.fortifyUI.SetActive(false);
                }
            }
            
            if (turn.selected != null & turn.turnMode == "Attack")
            {
                foreach (Territory neighbour in turn.selected.neighbourTerritories)
                {
                    if (neighbour == this)
                    {
                        //Debug.Log($"Attack: {this}");
                        attackButtonText.text = $"Attack: {this.gameObject.name}";
                        attackButtonText.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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

                        if(turn.attacker.counter.troopCount == 2){
                            turn.SetNumberOfAttackDice(1);
                        }else if(turn.attacker.counter.troopCount == 3){
                            turn.SetNumberOfAttackDice(2);
                        }else if(turn.attacker.counter.troopCount > 3){
                            turn.SetNumberOfAttackDice(3);
                        }else{
                            Debug.Log("Too few troops to attack with");
                            attackUIIsActive = false;
                            ToggleAttackUI();
                            turn.territoryInteractToggle = true;
                        }

                        if(neighbour!=null){
                            if(neighbour.counter.troopCount <= 2){
                                turn.SetNumberOfDefenseDice(1);
                            }else if(neighbour.counter.troopCount > 2){
                                turn.SetNumberOfDefenseDice(2);
                            }else{Debug.Log("Should not be here");}
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draws arrow indicators between this territory and its neighbors.
    /// </summary>
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

            Vector3 arrowPosition = new Vector3(transform.position.x+(direction.x/2),transform.position.y+(direction.y/2),-7f);

            arrow.transform.localScale = new Vector3(0.1f,0.1f,0);

            arrow.transform.position = arrowPosition;

            arrow.name = neighbour.name;

            arrow.GetComponent<SpriteRenderer>().color = new Color(255,255,255,0.9f);

            arrow.gameObject.SetActive(false);

            arrows.Add(arrow);
            i++;
        }
        
    }

    /// <summary>
    /// Hides the arrow indicators.
    /// </summary>
    public void HideArrows()
    {
        
        foreach (GameObject arrow in arrows)
        {
            arrow.gameObject.SetActive(false);
        }
        isArrowsActive = false;
    }

    /// <summary>
    /// Shows the arrow indicators.
    /// </summary>
    public void ShowArrows()
    {
        
        foreach (Territory neighbour in neighbourTerritories)
        {
            if (neighbour.controlledBy!=controlledBy)
            {
                foreach (GameObject arrow in arrows)
                {
                    if (arrow.name == neighbour.name)
                    {
                        arrow.gameObject.SetActive(true);
                    }
                }
            }
        }
        isArrowsActive = true;
    }

    /// <summary>
    /// Enables attack highlighting mode for neighboring territories.
    /// </summary>
    public void EnableAttackHighlight()
    {
        foreach (Territory neighbour in neighbourTerritories)
        {
            neighbour.gameObject.GetComponent<OnHoverHighlight>().mode = "Attack";
        }
    }

    /// <summary>
    /// Disables attack highlighting mode, returning to default state.
    /// </summary>
    public void DisableAttackHighlight()
    {
        foreach (Territory neighbour in neighbourTerritories)
        {
            neighbour.gameObject.GetComponent<OnHoverHighlight>().mode = "Default";
        }
    }

    /// <summary>
    /// Toggles the visibility of the attack UI.
    /// </summary>
    public void ToggleAttackUI()
    {
        turn.attackAgainButton.SetActive(true);
        if (attackUI.activeSelf == true)
        {
            turn.attackerDice[0].SetActive(false);
            turn.defenderDice[0].SetActive(false);
            turn.attackerDice[1].SetActive(false);
            turn.defenderDice[1].SetActive(false);
            turn.attackerDice[2].SetActive(false);
            attackUI.SetActive(false);
            turn.attackUIActive = false;
            turn.territoryInteractToggle = false;
        }
        else
        {
            attackUI.SetActive(true);
            turn.attackUIActive = true;
            turn.territoryInteractToggle = false;
        }
    }

    /// <summary>
    /// Waits 5 seconds
    /// </summary>
    /// <returns>5 second wait</returns>
    IEnumerator Wait5()
    {
        turn.territoryInteractToggle = false;
        yield return new WaitForSeconds(5);
        turn.territoryInteractToggle = true;
        turn.sendTo.GetComponent<TextMeshProUGUI>().text = "Choose a territory to send troops to";
        turn.sendTo.SetActive(false);
        turn.chooseSender.SetActive(true);
    }
}
 