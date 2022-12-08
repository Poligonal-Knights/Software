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
    HashSet<PJ> enemiesInRange = new HashSet<PJ>();
    private HashSet<Enemy> alliesInRange = new HashSet<Enemy>();
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
    bool IsMyTurn()
    {
        return realizandoTurno;
    }

    [Task]
    bool IsAllyInRange()
    {
        //Marcar a los aliados a rango y meterlos en 
        var spacesInMovementRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        foreach (var ally in GameManager.Instance.enemies)
        {
            foreach (var s in spacesInMovementRange)
            {
                var distance = GridSpace.ManhattanDistance2D(s, ally.GetGridSpace());
                if (distance <= attackRange)
                {
                    enemiesInRange.Add(ally);
                    break;
                }
            }
        }
        return alliesInRange.Any();
    }

    [Task]
    bool MostDamagedAlly()
    {
        bool anyDamaged = false;
        int diffInHealth = int.MinValue;
        foreach (var ally in alliesInRange)
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
        //BFS.GetGoalGridSpace()


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
        var spacesInMovementRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        foreach (var enemy in GameManager.Instance.allies)
        {
            foreach (var s in spacesInMovementRange)
            {
                var distance = GridSpace.ManhattanDistance2D(s, enemy.GetGridSpace());
                if (distance <= attackRange)
                {
                    enemiesInRange.Add(enemy);
                    break;
                }
            }
        }
        return enemiesInRange.Any();
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
        foreach (PJ enemy in enemiesInRange)
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
        foreach (var ally in GameManager.Instance.enemies)
        {
            if (ally.health > 0)
            {
                return true;
            }
        }
        return false;
    }
}