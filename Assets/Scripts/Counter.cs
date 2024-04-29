using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Represents a counter that tracks and displays the number of troops owned by a player.
/// </summary>
public class Counter : MonoBehaviour
{
    /// <summary>
    /// The player who owns this counter.
    /// </summary>
    public Player ownedBy;

    /// <summary>
    /// The current count of troops.
    /// </summary>
    public int troopCount;

    /// <summary>
    /// The TextMeshPro component used to display the troop count.
    /// </summary>
    
    public TextMeshPro textMesh;

    /// <summary>
    /// Initializes the counter by finding the TextMeshPro component on its first child.
    /// </summary>
    private void Start() 
    {
        textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        
    }

    private void Update() 
    {
        if (troopCount <= 0)
        {
            troopCount = 1;
            UpdateCount(1);
        }
    }

    /// <summary>
    /// Updates the displayed troop count to the specified number.
    /// </summary>
    /// <param name="num">The new troop count to display.</param>
    public void UpdateCount(int num)
    {
        textMesh.text = num.ToString();
    }
}
