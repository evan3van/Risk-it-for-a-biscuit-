using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Territory : MonoBehaviour
{
    public int troops = 0;
    public Player controlledBy = null;
    public Turn turn;
    public List<Territory> neighbourTerritories;   //Gonna need to set this
    public GameObject arrowPrefab;
    public bool isArrowsActive = false;

    private void OnMouseDown() 
    {
        if(turn.myTurn == controlledBy)
        {
            //Activate arrows
            Debug.Log(name);
            if(turn.turnMode == "Reinforcement")
            {
                if(!isArrowsActive)
                {
                    DrawArrows();
                    isArrowsActive = true;
                }
                else if(isArrowsActive)
                {
                    isArrowsActive = false;
                }
            }
        }
    }

    private void DrawArrows()
    {
        for (int i = 0; i < neighbourTerritories.Count; i++)
        {
            Instantiate(arrowPrefab,transform.position,quaternion.identity,transform);
        }
    }
    
}
 