using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ThemeSwapper : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public List<Sprite> defaultSprites;
    public List<Sprite> cyberSprites;
    public SkinManager skinManager;

    public List<GameObject> list;

    public string Theme = "Normal";
    public GameObject background;
    public Sprite defaultBackground,cyberBackground;

    private void Start() {
        Theme = "Normal";
    }

    public void SwapTheme()
    {
        if (Theme == "Cyber")
        {
           int skinnumber = 0;
           Theme = "Normal";

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
            Theme = "Cyber";

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
