using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Referencing : MonoBehaviour
{
    public SpriteRenderer spriteR1;
    public SpriteRenderer spriteR2;
    public Sprite old;
    public Sprite cyber;
    public int test;

    void swap(){
        spriteR1.sprite = old;
    }
}
