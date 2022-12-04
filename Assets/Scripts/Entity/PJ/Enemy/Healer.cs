using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;
using Unity.Mathematics;

public class Healer : Enemy
{
    PJ focusedEnemy = null;
    private Enemy focusedAlly = null;
    List<PJ> enemiesInRangeList = new List<PJ>();

    private List<Enemy> alliesInRangeList = new List<Enemy>();
    //El damage en este enemigo simboliza la sanación que hace
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //health = 5;
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
    bool  IsMyTurn()
    {
        return realizandoTurno;
    }
    
    [Task]
    bool IsAllyInRange()
    {
        //Marcar a los aliados a rango y meterlos en 
        //alliesInRangeList
        return false; 
    }

    [Task]
    bool MostDamagedAlly()
    {
        bool anyDamaged = false;
        float diffInHealth = math.INFINITY;
        foreach (var ally in alliesInRangeList)
        {
            int thisDiffInHealth = ally.maxHealth - ally.health;
            if (thisDiffInHealth != 0 && thisDiffInHealth > diffInHealth)
            {
                diffInHealth = thisDiffInHealth;
                focusedAlly = ally;
                anyDamaged = true;
            }   
        }
        return anyDamaged;
    }
    [Task]
    bool GetCloserFocusedAlly()
    {
        return true;
    }

    [Task]
    bool CanIAttack()
    {
        if (focusedEnemy && !getAttackPerformed())
        {
            return true;
        }
        else return false;
    }
    [Task]
    bool CanIHeal()
    {
        if (focusedAlly && !getAttackPerformed())
        {
            return true;
        }
        else return false;
    }

    [Task]
    bool Heal()
    {
        focusedAlly.Heal(damage);
        return true;
    }
    [Task]
    bool EndTurn()
    {
        realizandoTurno = false;
        EnemyManager.Instance.enemyTurnEnd();
        focusedEnemy = null;
        focusedAlly = null;
        return true;
    }
    [Task]
    public bool EnemiesInRange()
    {
        //Mirar si hay enemigos a rango
        return true;
    }

    [Task]
    bool ChooseCloserEnemy()
    {
        //lo de siempre, decidir enemigo más cercano
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
    bool GetCloser()
    {
        //Moverse hasta la distancia justa para Atacar.
        return true;
    }

    [Task]
    bool Attack()
    {
        //Activar Debuff de daño y defensa
        return true;
    }

    [Task]
    bool CanIMove()
    {
        return movement > 0;
    }
    [Task]
    bool ChooseCloserAlly()
    {
        //lo de siempre, decidir aliado más cercano y meterlo en focused ally
        return true;
    }
    [Task]
    bool IsAnyoneAlive()
    {
        foreach (var ally in FindObjectsOfType<Enemy>())
        {
            if (ally.health > 0)
            {
                return true;
            }
        }
        return false;
    }
}