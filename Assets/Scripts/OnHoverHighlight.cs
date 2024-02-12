using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class OnHoverHighlight : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color origionalColor;
    public Color newColor;
    void Start()
    {
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
