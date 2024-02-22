using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class OnHoverHighlight : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color origionalColor = new(251,216,139,255);
    public Color newColor;
    void Start()
    {
        newColor = Color.red;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = origionalColor;
    }

    private void OnMouseEnter() {
        spriteRenderer.color = newColor;
    }
    private void OnMouseExit() {
        spriteRenderer.color = origionalColor;
    }
}
