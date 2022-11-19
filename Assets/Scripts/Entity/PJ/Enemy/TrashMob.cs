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
    public int myDamage;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 5;
        damage = myDamage;
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
        //Implementar comprobaci�n de enemigos a rango
        //enemiesInRangeList.Clear();//Por si acaso
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        HashSet<GridSpace> visitedSpaces = new HashSet<GridSpace>();
        foreach (var move in GetGridSpace().moves)
        {
            if (move.GetEntity() is Ally ally)
            {
                enemiesInRangeList.Add(ally);
            }
            if (!visitedSpaces.Contains(move) && CanMoveThere(GetGridSpace(), move))
            {
                visitedSpaces.Add(move);
                nodes.Enqueue(new BFS_Node(move, null, 1));
            }
        }
        while (nodes.Any())
        {
            var currentNode = nodes.Dequeue();
            if (currentNode.distance < maxMovement) //Cambiar por ActualMovement
            {
                foreach (var move in currentNode.space.moves)
                {
                    if (!visitedSpaces.Contains(move) && CanMoveThere(currentNode.space, move))
                    {
                        if (move.GetEntity() is Ally ally)
                        {
                            enemiesInRangeList.Add(ally);
                        }
                        if (!(currentNode.distance + 1 == maxMovement && move.GetEntity() is PJ))
                        {
                            visitedSpaces.Add(move);
                            nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
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
        //Implementar l�gica de elecci�n de enemigo
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        HashSet<GridSpace> visitedSpaces = new HashSet<GridSpace>();
        bool goalFinded = false;
        foreach (var move in GetGridSpace().moves)
        {
            if (!goalFinded && !visitedSpaces.Contains(move) && CanMoveThere(GetGridSpace(), move))
            {
                if (move.GetEntity() is Ally ally)
                {
                    focusedEnemy = ally;
                    goalFinded = true;
                }
                else
                {
                    visitedSpaces.Add(move);
                    nodes.Enqueue(new BFS_Node(move, null, 1));
                }
            }
        }
        while (nodes.Any() && !goalFinded)
        {
            var currentNode = nodes.Dequeue();

            foreach (var move in currentNode.space.moves)
            {
                if (!goalFinded && !visitedSpaces.Contains(move) && CanMoveThere(currentNode.space, move))
                {
                    if (move.GetEntity() is Ally ally)
                    {
                        focusedEnemy = ally;
                        goalFinded = true;
                    }
                    else
                        nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                }
            }
        }
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
        //Si la distancia entre focusedEnemy y Yo es =<1 -> ThisTask.Succeed()  DONE
        //Considero distancia Manhattan
        var vector = focusedEnemy.GetGridSpace().gridPosition - GetGridSpace().gridPosition;
        var distance = Mathf.Abs(vector.x) + Mathf.Abs(vector.y) + Mathf.Abs(vector.z);

        if (distance > 1)//En lugar de 1, enemigo deberia tener su variable rango
        {
            return true;
        }
        else return false;
    }
    [Task]
    bool Attack()
    {
        //Correr animaci�n de ataque mirando al enemigo
        focusedEnemy.health -= damage;
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
        //Implementar camino con el m�ximo movimiento posible hasta el enemigo DONE?
        //Lo mejor ser�a implementar un A*, me falta tiempo
        //Implemento BFS hasta encontrar meta

        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        HashSet<GridSpace> visitedSpaces = new HashSet<GridSpace>();
        bool goalFinded = false;
        BFS_Node goalNode = null;
        foreach (var move in GetGridSpace().moves)
        {
            if (!goalFinded && !visitedSpaces.Contains(move) && CanMoveThere(GetGridSpace(), move))
            {
                if (move.GetEntity().Equals(focusedEnemy))
                {
                    goalFinded = true;
                }
                else
                {
                    visitedSpaces.Add(move);
                    nodes.Enqueue(new BFS_Node(move, null, 1));
                }
            }
        }
        while (nodes.Any() && !goalFinded)
        {
            var currentNode = nodes.Dequeue();

            foreach (var move in currentNode.space.moves)
            {
                if (!goalFinded && !visitedSpaces.Contains(move) && CanMoveThere(currentNode.space, move))
                {
                    if (move.GetEntity().Equals(focusedEnemy))
                    {
                        goalFinded = true;
                        goalNode = currentNode;
                    }
                    else
                        nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                }
            }
        }
        if (goalNode is not null)
        {
            var node = goalNode;
            while ((node.distance > movement || node.space.GetEntity() is PJ) && (node is not null)) //Revisar dobles comprobaciones
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
    bool EndTurn()
    {
        EnemyManager.Instance.enemyTurnEnd();
        return true;
    }

    [Task]
    void IsMyTurn()
    {
        if (realizandoTurno)
            ThisTask.Succeed();
    }
}
