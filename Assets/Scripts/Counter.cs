using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public Player ownedBy;
    public int troopCount;
    public TextMeshPro textMesh;

    private void Start() 
    {
        textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        //Debug.Log(transform.GetChild(0).name);
    }
    public void UpdateCount(int num)
    {
        textMesh.text = num.ToString();
    }
}
