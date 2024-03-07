using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Phase : MonoBehaviour
{
   public void GoToPlayPhase()
    {
        SceneManager.LoadScene("PlayPhase");
    }

    public void GoToMainReinforcementPhase()
    {
        SceneManager.LoadScene("ReinforcementPhase");
    }


    public void GoToAttackPhase()
    {
        SceneManager.LoadScene("AttackPhase");
    } 


    public void GoToFortifyPhase()
    {
        SceneManager.LoadScene("FortifyPhase");
    } 

}
