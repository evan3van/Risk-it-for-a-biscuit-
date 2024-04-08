using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages navigation between different game phases by loading corresponding scenes.
/// </summary>
public class Phase : MonoBehaviour
{

    /// <summary>
    /// Loads the scene associated with the game's play phase.
    /// </summary>
    public void GoToPlayPhase()
    {
        SceneManager.LoadScene("PlayPhase");
    }

    /// <summary>
    /// Loads the scene associated with the game's main reinforcement phase.
    /// </summary>
    public void GoToMainReinforcementPhase()
    {
        SceneManager.LoadScene("ReinforcementPhase");
    }

    /// <summary>
    /// Loads the scene associated with the game's attack phase.
    /// </summary>
    public void GoToAttackPhase()
    {
        SceneManager.LoadScene("AttackPhase");
    } 

    /// <summary>
    /// Loads the scene associated with the game's fortify phase.
    /// </summary>
    public void GoToFortifyPhase()
    {
        SceneManager.LoadScene("FortifyPhase");
    } 

}
