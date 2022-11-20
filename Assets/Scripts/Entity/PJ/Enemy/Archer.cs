using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;

public class Archer : Enemy
{
    PJ focusedEnemy = null;
    List<PJ> enemiesInRangeList = new List<PJ>();
    public int myDamage;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 5;
        damage = myDamage;
        attackRange = 3;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void EnemyAI()
    {
     //base.EnemyAI();
     realizandoTurno = true;
    }

    [Task]
    public bool EnemiesInRange()
    {
        //Implementar aquí la comprobación de si hay enemigos a distancia de ataque
        
        //Añadir a enemiesInRangeList
        return enemiesInRangeList.Any();
    }
    [Task]
    bool ChooseInjured()
    {
        var healthMostInjured = Mathf.Infinity;
        foreach (PJ enemy in enemiesInRangeList)
        {
            if (enemy.health < healthMostInjured)
            {
                focusedEnemy = enemy;
                healthMostInjured = enemy.health;
            }
        }
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

        if (worked == false) return false;
        else return true;
    }

    [Task]
    bool IdentifySafeMove()
    {
        //Elegir casilla a máxima distnacia de focusedEnemy
        //para poder disparar desde esta
        return true;
    }

    [Task]
    bool GetIntoSafeRange()
    {
        //Implementar el GetCloser del arquero hacia la casilla segura
        if (CanIMove())
        {
            //La idea es que si tengo 0 de movimiento pues no haga nada
            //Así que eso, escribe el codigo aquí dentro
        }
        return true;
    }

    [Task]
    bool CanIAttack()
    {
        return (focusedEnemy && InAttackRange() && !getAttackPerformed());
    }
    
    bool InAttackRange()
    {
        if(focusedEnemy!=null){
            //Si la distancia entre focusedEnemy y Yo es =<1 -> ThisTask.Succeed()  DONE
            //Considero distancia Manhattan
            var vector = focusedEnemy.GetGridSpace().gridPosition - GetGridSpace().gridPosition;
            var distance = Mathf.Abs(vector.x) + Mathf.Abs(vector.y) + Mathf.Abs(vector.z);

            if (distance <= attackRange)//Incluído attackRange en Enemy
            {
                return true;
            }
            else return false;
        }
        else
        {
            return false;
        }
    }
    
    [Task]
    bool Attack()
    {
        //Correr animación de ataque mirando al enemigo
        focusedEnemy.DealDamage(damage);
        setAttackPerformed(true);
        return true;
    }

    [Task]
    bool StillAlive()
    {
        if (focusedEnemy)
            return focusedEnemy.health <= 0;
        else return false;
    }
    
    [Task]
    bool EndTurn()
    {
        realizandoTurno = false;
        EnemyManager.Instance.enemyTurnEnd();
        return true;
    }
    
    [Task]
    bool CanIMove()
    {
        return movement > 0;
    }

    [Task]
    bool IdentifyClosestEnemy()
    {
        //Encontrar enemigo más cercano
        //Marcar a dicho enemigo como focusedEnemy 
        return true;
    }
    
    [Task]
    void IsMyTurn()
    {
        if (realizandoTurno)
            ThisTask.Succeed();
    }
    //Esto puede ser un desastre, mañana veremos.
}
