using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;

public class TrashMob : Enemy
{
    PJ focusedEnemy = null;
    List<PJ> enemiesInRangeList = new List<PJ>();

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();
    }

    public override void EnemyAI()
    {
        realizandoTurno = true;
    }

    [Task]
    bool IsEnemyFocused()
    {
        return focusedEnemy;
    }

    [Task]
    bool IsEnemyAlive()
    {
        if (focusedEnemy != null && focusedEnemy.health > 0) return true;
        else return false;
    }

    [Task]
    bool EnemiesInRange()
    {
        var spacesInRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        foreach(var s in spacesInRange)
        {
            if(s.GetEntity() is null)
            {
                foreach(var move in s.moves)
                {
                    if(move.GetEntity() is Ally enemy)
                    {
                        var distance = s.ManhattanDistance(move);
                        if(distance < 3)
                        {
                            enemiesInRangeList.Add(enemy);
                        }
                    }
                }
            }
        }
        return enemiesInRangeList.Any();
    }

    [Task]
    bool ChooseCloserEnemy()
    {
        var CloserEnemySpace = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere, candidate =>
        {
            if (candidate.GetEntity() is not null) return false;
            foreach (var move in candidate.moves)
            {
                if (move.GetEntity() is Ally enemy)
                    if (candidate.ManhattanDistance(move) < 3)
                    {
                        focusedEnemy = enemy;
                        return true;
                    }
            }
            return false;
        });
        return true;
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
    bool CanIAttack()
    {
        if (focusedEnemy && !getAttackPerformed())
        {
            return true;
        }
        else return false;
    }

    [Task]
    bool InAttackRange()
    {
        if (focusedEnemy != null)
        {
            //Si la distancia entre focusedEnemy y Yo es =<1 -> ThisTask.Succeed()  DONE
            //Considero distancia Manhattan
            var vector = focusedEnemy.GetGridSpace().gridPosition - GetGridSpace().gridPosition;
            //var distance = Mathf.Abs(vector.x) + Mathf.Abs(vector.y) + Mathf.Abs(vector.z);
            var distance = Mathf.Abs(vector.x) + Mathf.Abs(vector.z);

            if (distance <= 1)//En lugar de 1, enemigo deberia tener su variable rango
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
        if (!getAttackPerformed())
            LineDrawer.DrawLine(this.GetGridSpace().GetWorldPosition(), focusedEnemy.GetGridSpace().GetWorldPosition());
        //Correr animaci�n de ataque mirando al enemigo
        focusedEnemy.DealDamage(damage, this);
        setAttackPerformed(true);
        return true;
    }

    [Task]
    bool CanIMove()
    {
        return movement > 0;
    }

    [Task]
    bool GetCloser()
    {
        if (!focusedEnemy) return false;

        var goalSpace = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere, candidate =>
        {
            if (candidate.GetEntity() is not null) return false;
            foreach (var move in candidate.moves)
            {
                if (move.GetEntity() == focusedEnemy)
                    if (candidate.ManhattanDistance(move) < 3) return true;
            }
            return false;
        });

        var goalNode = goalSpace?.node;
        if (goalNode is null) return false;
        var node = goalNode;
        while (node is not null && (node.distance > movement || node.space.GetEntity() is PJ)) //Revisar dobles comprobaciones
        {
            node = node.parent;
        }
        if (node is null)
        {
            Debug.Log(this);
            Debug.Log(focusedEnemy);
            return false;
        }
        MoveTo(node.space);
        return true;
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
    void IsMyTurn()
    {
        if (realizandoTurno)
            ThisTask.Succeed();
    }
}
