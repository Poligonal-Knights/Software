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

    HashSet<GridSpace> BFS(GridSpace start = null, int rangeToUse = 0)
    {
        //var range = rangeToUse;
        var range = rangeToUse == 0 ? movement : rangeToUse;
        var initialSpace = start == null ? space : start;
        HashSet<GridSpace> spaces = new HashSet<GridSpace>();
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        foreach (var move in initialSpace.moves)
        {
            if (range > 0 && !spaces.Contains(move) && CanMoveThere(space, move))
            {
                spaces.Add(move);
                nodes.Enqueue(new BFS_Node(move, null, 1));
            }
        }
        while (nodes.Any())
        {
            var currentNode = nodes.Dequeue();
            if (currentNode.distance < range)
            {
                foreach (var move in currentNode.space.moves)
                {
                    if (!spaces.Contains(move) && CanMoveThere(currentNode.space, move))
                    {
                        if (!(currentNode.distance + 1 == range && move.GetEntity() is PJ))
                        {
                            spaces.Add(move);
                            nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                        }
                    }
                }
            }
        }
        return spaces;
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
        var spacesInRange = BFS(rangeToUse: movement + attackRange);
        foreach (var s in spacesInRange)
        {
            if (s.GetEntity() is Ally ally)
            {
                enemiesInRangeList.Add(ally);
            }
        }
        return enemiesInRangeList.Any();
    }

    [Task]
    bool ChooseCloserEnemy()
    {
        //Elegir enemigo más cercano para ir moviendose si no hay nadie a rango de explotar
        foreach (var enemy in GameManager.Instance.enemies)
        {

        }
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
