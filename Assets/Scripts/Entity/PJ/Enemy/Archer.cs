using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;

public class Archer : Enemy
{
    PJ focusedEnemy = null;
    GridSpace bestSpace = null;
    List<PJ> enemiesInRangeList = new List<PJ>();
    public int myDamage;

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
        //base.EnemyAI();
        realizandoTurno = true;
    }

    [Task]
    public bool EnemiesInRange()
    {
        //Implementar aquí la comprobación de si hay enemigos a distancia de ataque
        enemiesInRangeList.Clear();
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        HashSet<GridSpace> visitedSpaces = new HashSet<GridSpace>();
        foreach (var move in GetGridSpace().moves)
        {
            if (!visitedSpaces.Contains(move) && CanMoveThere(GetGridSpace(), move))
            {
                visitedSpaces.Add(move);
                nodes.Enqueue(new BFS_Node(move, null, 1));
            }
        }
        while (nodes.Any())
        {
            var currentNode = nodes.Dequeue();
            if (currentNode.distance < movement) //Cambiar por ActualMovement
            {
                foreach (var move in currentNode.space.moves)
                {
                    if (!visitedSpaces.Contains(move) && CanMoveThere(currentNode.space, move))
                    {
                        if (!(currentNode.distance + 1 == movement && move.GetEntity() is PJ))
                        {
                            visitedSpaces.Add(move);
                            nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                        }
                    }
                }
            }
        }

        foreach (var enemy in GameManager.Instance.allies)

        {
            foreach (var vSpace in visitedSpaces)
            {
                //if (ManhattanDistance(vSpace, enemy.GetGridSpace()) <= attackRange)
                if (vSpace.ManhattanDistance2D(enemy.GetGridSpace()) <= attackRange)
                    {
                    enemiesInRangeList.Add(enemy);
                    break;
                }
            }
        }
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
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        HashSet<GridSpace> visitedSpaces = new HashSet<GridSpace>();
        foreach (var move in GetGridSpace().moves)
        {
            if (!visitedSpaces.Contains(move) && CanMoveThere(GetGridSpace(), move))
            {
                visitedSpaces.Add(move);
                nodes.Enqueue(new BFS_Node(move, null, 1));
            }
        }
        while (nodes.Any())
        {
            var currentNode = nodes.Dequeue();
            if (currentNode.distance < movement) //Cambiar por ActualMovement
            {
                foreach (var move in currentNode.space.moves)
                {
                    if (!visitedSpaces.Contains(move) && CanMoveThere(currentNode.space, move))
                    {
                        if (!(currentNode.distance + 1 == movement && move.GetEntity() is PJ))
                        {
                            visitedSpaces.Add(move);
                            nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                        }
                    }
                }
            }
        }

        HashSet<GridSpace> candidateSpaces = new HashSet<GridSpace>();
        foreach (var vSpace in visitedSpaces)
        {
            //if (ManhattanDistance(vSpace, focusedEnemy.GetGridSpace()) <= attackRange)
            if (GridSpace.ManhattanDistance2D(vSpace, focusedEnemy.GetGridSpace()) <= attackRange)
                {
                candidateSpaces.Add(vSpace);
            }
        }
        int maxDistance = -1;
        foreach (var cSpace in candidateSpaces)
        {
            var sum = 0;
            foreach (var enemy in GameManager.Instance.allies)
            {
                sum += GridSpace.ManhattanDistance2D(cSpace, enemy.GetGridSpace());
            }

            if (sum > maxDistance)
            {
                maxDistance = sum;
                bestSpace = cSpace;
            }
        }
        return true;
    }

    [Task]
    bool GetIntoSafeRange()
    {
        //Implementar el GetCloser del arquero hacia la casilla segura
        if (CanIMove() && bestSpace is not null)
        {
            //La idea es que si tengo 0 de movimiento pues no haga nada
            //Así que eso, escribe el codigo aquí dentro
            MoveTo(bestSpace);
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
        if (focusedEnemy != null)
        {
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
        LineRendererManager.Instance.AddLine(this.GetGridSpace().GetWorldPosition(), focusedEnemy.GetGridSpace().GetWorldPosition());
        //Correr animación de ataque mirando al enemigo
        Debug.Log("Atacando a " + focusedEnemy + " con " + focusedEnemy.health);
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
        focusedEnemy = null;
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
        int minDistance = 5000;
        PJ bestEnemy = null;
        foreach (var enemy in GameManager.Instance.allies)
        {
            var distance = GridSpace.ManhattanDistance2D(space, enemy.GetGridSpace());
            if (distance < minDistance)
            {
                minDistance = distance;
                bestEnemy = enemy;
            }
        }
        if (bestEnemy != null)
        {
            focusedEnemy = bestEnemy;
        }
        Debug.Log(focusedEnemy);
        bool goalFinded = false;
        BFS_Node goalNode = null;

        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        HashSet<GridSpace> visitedSpaces = new HashSet<GridSpace>();
        foreach (var move in GetGridSpace().moves)
        {
            if (move.GetEntity() is not null && move.GetEntity().Equals(focusedEnemy))
            {
                goalFinded = true;
            }

            else if (!visitedSpaces.Contains(move) && CanMoveThere(GetGridSpace(), move))
            {
                visitedSpaces.Add(move);
                nodes.Enqueue(new BFS_Node(move, null, 1));
            }
        }
        while (nodes.Any() && !goalFinded)
        {
            var currentNode = nodes.Dequeue();
            foreach (var move in currentNode.space.moves)
            {
                if (move.GetEntity() is not null && move.GetEntity().Equals(focusedEnemy))
                {
                    goalFinded = true;
                    goalNode = currentNode;
                }
                if (!visitedSpaces.Contains(move) && CanMoveThere(currentNode.space, move))
                {
                    if (!(currentNode.distance + 1 == movement && move.GetEntity() is PJ))
                    {
                        visitedSpaces.Add(move);
                        nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                    }
                }
            }
        }
        if (goalNode is not null)
        {
            var node = goalNode;
            while ((node is not null) && (node.distance > movement || node.space.GetEntity() is PJ)) //Revisar dobles comprobaciones
            {
                node = node.parent;
            }
            Debug.Log(node);

            if (node is not null)
            {
                bestSpace = node.space;
            }
        }

        return true;
    }

    [Task]
    void IsMyTurn()
    {
        if (realizandoTurno)
            ThisTask.Succeed();
    }
}
