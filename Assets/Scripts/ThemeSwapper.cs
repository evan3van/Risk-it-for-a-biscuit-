using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;

/// <summary>
/// Manages the theme change from the main menu
/// </summary>
public class ThemeSwapper : MonoBehaviour
{
    /// <summary>
    /// The origional sprite designs
    /// </summary>
    public List<Sprite> defaultSprites;

    /// <summary>
    /// The cyber theme sprites
    /// </summary>
    public List<Sprite> cyberSprites;

    /// <summary>
    /// A reference to the skin manager
    /// </summary>
    public SkinManager skinManager;

    /// <summary>
    /// A list of skinnable objects
    /// </summary>
    public List<GameObject> list;

    /// <summary>
    /// The set theme
    /// </summary>
    public string Theme = "Normal";

    /// <summary>
    /// The background object
    /// </summary>
    public GameObject background;

    /// <summary>
    /// A background sprite
    /// </summary>
    public Sprite defaultBackground,cyberBackground;

    /// <summary>
    /// List of the player sprites
    /// </summary>
    public List<Sprite> playerSprites;

    /// <summary>
    /// Sets the theme
    /// </summary>
    private void Start() {
        Theme = "Normal";
    }

    /// <summary>
    /// Swaps the theme depending on the set theme, and sets all the sprites to be of that theme
    /// </summary>
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
