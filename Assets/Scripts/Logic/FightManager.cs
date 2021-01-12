using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;

    public void Start()
    {
        SetupFight();
    }

    public void SetupFight()
    {
        player.SetupPlayer();
        //enemy.SetupEnemy();
        
        player.OnTurnStart();
    }
}
