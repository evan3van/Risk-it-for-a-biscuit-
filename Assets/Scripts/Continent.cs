using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Continent : Territory
{
    public List<Territory> territories;   //List of all the territory objects in the continent (found in the hierarchy)
    private void Awake() {
        foreach (Transform child in transform)
        {
            Territory territory = child.AddComponent<Territory>();
            territories.Add(territory);
        }
    }
}
