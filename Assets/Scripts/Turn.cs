using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    int turnNumber;
    public Player myTurn;
    public void nextTurn(Player player)
    {
        myTurn = player;
        ReinforcementPhase(player);
    }

    void ReinforcementPhase(Player player)
    {
        int reinforcementNum = player.controlledTerritories.Count / 3;
        
        player.GiveTroops(reinforcementNum);

        
    }
    
    void AttackPhase()
    {

    }

    void FortifyPhase()
    {

    }
}
