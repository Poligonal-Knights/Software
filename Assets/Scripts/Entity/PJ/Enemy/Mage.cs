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
    bool IsMyTurn()
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
        var spaceAtRange = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere,
            s => s.ManhattanDistance2D(optimalSpace) <= attackRange);
        var goalNode = spaceAtRange.node;
        Debug.Log(goalNode);

        if (goalNode is not null)
        {
            var node = goalNode;
            while ((node is not null) && (node.distance > movement || node.space.GetEntity() is PJ))
            {
                node = node.parent;
            }

            if (node is not null)
            {
                Debug.Log("me estoy moviendo sir");
                MoveTo(node.space);
            }
            else Debug.Log("TREMENDO");
        }
        return true;
    }

    [Task]
    bool IsTargetFocused()
    {
        return false;
        //return optimalSpace != null;
    }

    [Task]
    bool ChooseCloserEnemy()
    {
        if (!focusedEnemy)
        {
            return false;
        }
        Debug.Log("ChooseCloserEnemy");
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
    bool EnemiesInRange()
    {
        //Comprobar los enemigos que hay a rango de explotar y devolver false si no hay ninguno
        List<GridSpace> spacesInMovementRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        var spacesInAttackRange = GridManager.SpacesAtManhattanRange(spacesInMovementRange.ToHashSet(), attackRange);
        HashSet<GridSpace> enemySpaces = new HashSet<GridSpace>();
        foreach (var enemy in GameManager.Instance.allies)
            enemySpaces.Add(enemy.GetGridSpace());
        enemySpaces = GridManager.SpacesAtManhattanRange2D(enemySpaces, explosionRange);
        spacesInAttackRange.IntersectWith(enemySpaces);
        //spacesInAttackRange.RemoveWhere(s => !s.IsPassable());
        return spacesInAttackRange.Any();
    }

    [Task]
    bool ChooseOptimalSpace()
    {
        List<GridSpace> spacesInMovementRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        var spacesInAttackRange = GridManager.SpacesAtManhattanRange(spacesInMovementRange.ToHashSet(), attackRange);

        GridSpace _optimalSpace = null;
        int enemiesAffected, maxEnemiesAffected = 0;
        foreach (var space in spacesInAttackRange)
        {
            enemiesAffected = 0;
            var spacesAtExplosionRange = GridManager.SpacesAtManhattanRange2D(space, explosionRange);
            foreach (var s in spacesAtExplosionRange)
                if (s.GetEntity() is Ally)
                    enemiesAffected++;
            if (maxEnemiesAffected < enemiesAffected)
            {
                maxEnemiesAffected = enemiesAffected;
                _optimalSpace = space;
            }
        }
        optimalSpace = _optimalSpace;
        Debug.Log(optimalSpace);
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
        var spacesAtExplosionRange = GridManager.SpacesAtManhattanRange2D(optimalSpace, explosionRange);
        foreach (var s in spacesAtExplosionRange)
            if (s.GetEntity() is Ally enemy)
            {
                enemy.DealDamage(damage, this);
                Debug.Log("MAGO TRUCO");

            }
        return true;
    }

    [Task]
    bool SpaceInAttackRange()
    {
        //Devolver si la casilla optima está a menos de las casillas que tiene el mago de rango
        var distance = space.ManhattanDistance2D(optimalSpace);
        return distance <= attackRange;
    }
}
