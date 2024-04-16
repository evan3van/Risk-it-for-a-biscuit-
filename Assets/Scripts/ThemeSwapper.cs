using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ThemeSwapper : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public List<Sprite> defaultSprites;
    public List<Sprite> cyberSprites;
    public SkinManager skinManager;

    public List<GameObject> list;

    public int Theme;
    public GameObject background;
    public Sprite defaultBackground,cyberBackground;

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

            background.GetComponent<UnityEngine.UI.Image>().sprite = defaultBackground;
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

            background.GetComponent<UnityEngine.UI.Image>().sprite = cyberBackground;

        }
    }
    
   
}
