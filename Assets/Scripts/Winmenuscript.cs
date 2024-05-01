using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Winmenuscript : MonoBehaviour
{
    /// <summary>
    /// Goes back to the main menu scene
    /// </summary>
    public void GoToMainMenu()
    {
        GameObject customise = GameObject.Find("Customise");
        Destroy(customise);
        GameObject themeswapper = GameObject.Find("ThemeSwapper");
        Destroy(themeswapper);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
