using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continent : Territory
{
    public List<Territory> territories;   //List of all the territory objects in the continent (found in the hierarchy)
    private void Start() {
        foreach (Transform child in transform)
        {
            Territory territory = child.GetComponent<Territory>();
            territories.Add(territory);
        }
    }
}
