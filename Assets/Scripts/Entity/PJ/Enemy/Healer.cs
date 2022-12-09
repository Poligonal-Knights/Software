using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;
using Unity.Mathematics;
using System.Runtime.InteropServices.WindowsRuntime;

public class Healer : Enemy
{
    PJ focusedEnemy = null;
    private Enemy focusedAlly = null;
    List<PJ> enemiesInRange = new List<PJ>();
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
                    alliesInRange.Add(ally);
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
                Debug.Log("FOCUSED ALLY");
                focusedAlly = ally;
                Debug.Log(focusedAlly);
                anyDamaged = true;
            }
        }
        return anyDamaged;
    }
    [Task]
    bool GetCloserFocusedAlly()
    {
        var goalSpace = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere, candidate =>
        {
            if (candidate.GetEntity() is null)
            {
                var distance = candidate.ManhattanDistance2D(focusedAlly.GetGridSpace());
                if (distance <= attackRange)
                    return true;
            }
            return false;
        });
        var goalNode = goalSpace.node;
        if (goalNode is not null)
        {
            var node = goalNode;
            while ((node is not null) && (node.distance > movement || node.space.GetEntity() is PJ))
            {
                node = node.parent;
            }

            if (node is not null)
            {
                MoveTo(node.space);
            }
        }
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
        //var goalSpace = BFS.GetGoalGridSpace(space, movement, CanMoveThere, candidate =>
        //{
        //    foreach (var enemy in GameManager.Instance.allies)
        //    {
        //        var distance = GridSpace.ManhattanDistance2D(candidate, enemy.GetGridSpace());
        //        if (distance <= attackRange)
        //        {
        //            focusedEnemy = enemy;
        //            return true;
        //        }
        //    }
        //    return false;
        //});
        //if(goalSpace is not null)
        //{
        //    MoveTo(goalSpace);
        //}
        //else
        //{

        //}
        //return enemiesInRange.Any();
    }

    [Task]
    bool ChooseCloserEnemy()
    {
        //lo de siempre, decidir enemigo más cercano
        //var goalSpace = BFS.GetGoalGridSpace(space, movement, CanMoveThere, candidate =>
        //{
        //    if (candidate.GetEntity() is null)
        //    {
        //        var distance = candidate.ManhattanDistance2D(focusedEnemy.GetGridSpace());
        //        if (distance <= attackRange)
        //            return true;
        //    }
        //    return false;
        //});
        focusedEnemy = enemiesInRange[0];
        return true;
    }

    [Task]
    bool BattleCryActive()
    {
        foreach (PJ enemy in enemiesInRange)
            if (enemy is Knight knight && knight.UsingGritoDeBatalla())
            {
                focusedEnemy = knight;
                return true;
            }
        return false;
    }

    [Task]
    bool GetCloser()
    {
        //Moverse hasta la distancia justa para Atacar.
        var goalSpace = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere, candidate =>
        {
            if (candidate.GetEntity() is null)
            {
                var distance = candidate.ManhattanDistance2D(focusedEnemy.GetGridSpace());
                if (distance <= attackRange)
                    return true;
            }
            return false;
        });
        MoveTo(goalSpace);
        return true;
    }

    [Task]
    bool Attack()
    {
        //Activar Debuff de daño y defensa
        new Curse(focusedEnemy);
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
        var spaceWithAllyInRange = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere,
            //candidate => candidate.GetEntity() is Ally);
            candidate =>
            {
                if (candidate.GetEntity() is null)
                {
                    foreach (var ally in GameManager.Instance.enemies)
                    {
                        var distance = candidate.ManhattanDistance2D(ally.GetGridSpace());
                        if (distance <= attackRange)
                            return true;
                    }
                }
                return false;
            });

        var goalNode = spaceWithAllyInRange?.node;
        if (goalNode is not null)
        {
            var node = goalNode;
            while ((node is not null) && (node.distance > movement || node.space.GetEntity() is PJ))
            {
                node = node.parent;
            }

            if (node is not null)
            {
                MoveTo(node.space);
            }
            else return false;
        }
        else return false;

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