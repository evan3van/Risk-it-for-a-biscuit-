using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Referencing : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Example component to change
    public List<Sprite> defaultSprites;
    public List<Sprite> cyberSprites;
    // Method to switch themes
    public SkinManager skinManager;

    public List<GameObject> list;

    public int Theme;

    public void SwapTheme()
    {
        if (Theme == 1)
        {
           int skinnumber = 0;
           Theme = 2;

            foreach (Sprite item in defaultSprites)
            {
                skinManager.skins[skinnumber] = item;
                list[skinnumber].GetComponent<SpriteRenderer>().sprite = item;
                skinnumber ++;
            }
        }
        else 
        {
            int skinnumber = 0;
            Theme = 1;

            foreach (Sprite item in cyberSprites)
            {
                skinManager.skins[skinnumber] = item;
                list[skinnumber].GetComponent<SpriteRenderer>().sprite = item;
                skinnumber ++;
            }     
        }
    }
    
   
}