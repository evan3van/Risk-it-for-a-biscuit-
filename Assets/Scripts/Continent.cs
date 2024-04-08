using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Represents a continent comprised of multiple territories.
/// This class is responsible for managing the collection of territories
/// that make up a continent in the game.
/// </summary>
public class Continent : MonoBehaviour
{

    /// <summary>
    /// A list of all the territories that belong to this continent.
    /// Territories are game objects in the scene hierarchy that represent distinct areas or regions within the continent.
    /// </summary>
    public List<Territory> territories; 
}
