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
    bool EndTurn()
    {
        realizandoTurno = false;
        EnemyManager.Instance.enemyTurnEnd();
        focusedEnemy = null;
        return true;
    }

    [Task]
    bool IsMyTurn()
    {
        return realizandoTurno;
    }

    [Task]
    bool EnemiesInRange()
    {
        List<GridSpace> spacesInRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        foreach (var enemy in GameManager.Instance.allies)
        {
            foreach (var s in spacesInRange)
            {
                if (s.gridPosition.y == enemy.GetGridSpace().gridPosition.y &&
                                s.ManhattanDistance2D(enemy.GetGridSpace()) <= attackRange)
                {
                    enemiesInRangeList.Add(enemy);
                    break;
                }
            }
        }
        return enemiesInRangeList.Any();
    }

    [Task]
    bool ChooseCloserEnemy()
    {
        //Elegir enemigo más cercano para ir moviendose si no hay nadie a rango de explotar
        GridSpace closerEnemySpace = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere, (GridSpace candidate) =>
        {
            if (candidate.GetEntity() is null)
            {
                foreach (var move in candidate.moves)
                {
                    if (move.GetEntity() is Ally)
                    {
                        return true;
                    }
                }
            }
            return false;
        });
        focusedEnemy = closerEnemySpace.GetEntity() as PJ;
        optimalSpace = closerEnemySpace;
        return true;
    }

    [Task]
    bool ChooseOptimalSpace()
    {
        //Elegir espacio donde más enemigos son afectados
        GridSpace _OptimalSpace = null;
        //SpacesInRange quiza puede estar en un miembro para ahorrar una busqueda
        List<GridSpace> spacesInMovementRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        var enemies = GameManager.Instance.allies;
        int enemiesAffected, maxEnemiesAffected = 0;
        foreach (var space in spacesInMovementRange)
        {
            enemiesAffected = 0;
            foreach (var enemy in enemies)
            {
                if (space.gridPosition.y == enemy.GetGridSpace().gridPosition.y &&
                    space.ManhattanDistance2D(enemy.GetGridSpace()) <= attackRange)
                {
                    enemiesAffected++;
                }
            }
            if (maxEnemiesAffected < enemiesAffected)
            {
                maxEnemiesAffected = enemiesAffected;
                _OptimalSpace = space;
            }
        }
        optimalSpace = _OptimalSpace;
        return true;
    }
    [Task]
    bool BattleCryActive()
    {
        foreach (PJ enemy in enemiesInRangeList)
            if (enemy is Knight knight && knight.UsingGritoDeBatalla())
            {
                focusedEnemy = knight;
                optimalSpace = focusedEnemy.GetGridSpace();
                return true;
            }
        return false;
    }

    [Task]
    bool InOptimalSpace()
    {
        return this.space == optimalSpace;
    }

    [Task]
    bool Explosion()
    {
        //Implementar daño a los enemigos en rango de la explosión
        foreach (var enemy in GameManager.Instance.allies)
        {
            if (space.gridPosition.y == enemy.GetGridSpace().gridPosition.y &&
                space.ManhattanDistance2D(enemy.GetGridSpace()) <= attackRange)
            {
                enemy.DealDamage(damage);
            }
        }
        this.DealDamage(health);
        Debug.Log("THINGS GOT BOOMBY BOOMBY");
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
        var goalNode = optimalSpace.node;
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
}
