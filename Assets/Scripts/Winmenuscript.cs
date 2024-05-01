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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
