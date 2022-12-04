using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;

public class Mage : Enemy
{
    List<PJ> enemiesInRangeList = new List<PJ>();
    private PJ focusedEnemy = null;
    private GridSpace optimalSpace = null;
    public int explosionRange = 2;  //Marca el rango de la explosión en sí.
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    public override void EnemyAI()
    {
        realizandoTurno = true;
    }
    
    [Task]
    bool EndTurn()
    {
        realizandoTurno = false;
        EnemyManager.Instance.enemyTurnEnd();
        focusedEnemy = null;
        return true;
    }

    [Task]
    bool  IsMyTurn()
    {
        return realizandoTurno;
    }
    
    [Task]
    bool BattleCryActive()
    {
        bool worked = false;
        foreach (PJ enemy in enemiesInRangeList)
        {
            if (enemy is Knight)
            {
                if ((enemy as Knight).UsingGritoDeBatalla())
                {
                    focusedEnemy = enemy;
                    ThisTask.Succeed();
                    worked = true;
                }
            }
        }
        // if (worked == false) return false;
        // else return true;
        return worked;
    }
    [Task]
    bool CanIMove()
    {
        return movement > 0;
    }

    [Task]
    bool GetCloser()
    {
        //Moverse hacia la casilla hasta estar a rango
        return true;
    }

    [Task]
    bool IsTargetFocused()
    {
        return optimalSpace!=null;
    }
    
    [Task]
    bool ChooseCloserEnemy()
    {
        //Elegir enemigo más cercano para ir moviendose si no hay nadie a rango de explotar
        return false;
    }
    [Task]
    bool EnemiesInRange()
    {
        //Comprobar los enemigos que hay a rango de explotar y devolver false si no hay ninguno
        return false;
    }

    [Task]
    bool ChooseOptimalSpace()
    {
        return true;
    }

    
    [Task]
    bool CanIAttack()
    {
        // if (optimalSpace != null && !getAttackPerformed())
        // {
        //     return true;
        // }
        // else return false;
        return optimalSpace != null && !getAttackPerformed();
    }
    [Task]
    bool Attack()
    {
        setAttackPerformed(true);
        //Realizar daño a los enemigos a rango de la explosión donde haya explotado que será en OptimalSpace
        return true;
    }

    [Task]
    bool SpaceInAttackRange()
    {
        //Devolver si la casilla optima está a menos de las casillas que tiene el mago de rango
        return false; 
    }
}
