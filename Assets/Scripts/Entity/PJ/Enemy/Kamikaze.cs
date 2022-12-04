using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;

public class Kamikaze : Enemy
{
    List<PJ> enemiesInRangeList = new List<PJ>();
    private PJ focusedEnemy = null;
    private GridSpace optimalSpace = null;
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
    bool EnemiesInRange()
    {
        //Comprobar los enemigos que hay a rango de explotar y devolver false si no hay ninguno
        return false;
    }

    [Task]
    bool ChooseCloserEnemy()
    {
        //Elegir enemigo más cercano para ir moviendose si no hay nadie a rango de explotar
        return false;
    }

    [Task]
    bool ChooseOptimalSpace()
    {
        //Elegir espacio donde más enemigos son afectados
        return true;
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
    bool InOptimalSpace()
    {
        return this.space == optimalSpace;
    }
    [Task]
    bool Explosion()
    {
        this.DealDamage(999);
        //Implementar daño a los enemigos en rango de la explosión
        return false;
    }
    [Task]
    bool CanIMove()
    {
        return movement > 0;
    }

    [Task]
    bool GetCloser()
    {
        //Moverse a la casilla seleccionada como optima
        return true;
    }
}
