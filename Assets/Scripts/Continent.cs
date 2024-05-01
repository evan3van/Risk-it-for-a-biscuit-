using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Represents a continent comprised of multiple territories
/// </summary>
public class Continent : MonoBehaviour
{

    /// <summary>
    /// A list of all the territories that belong to this continent
    /// </summary>
    public List<Territory> territories; 
}
