using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Territory : MonoBehaviour
{
    public int troops = 0;
    public Player controlledBy = null;
    public Turn turn;
    public List<Territory> neighbourTerritories = new();   //Gonna need to set this
    public GameObject arrowPrefab;
    public bool isArrowsActive = false;

    private void OnMouseDown() 
    {
        if(turn.myTurn == controlledBy)
        {
            //Activate arrows
            Debug.Log(name);
            if(turn.turnMode == "Reinforce")
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
 