using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;

public class TrashMob : Enemy
{
    PJ focusedEnemy;
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
        base.EnemyAI();
    }


    [Task]
    void IsEnemyFocused()
    {
        if (focusedEnemy != null) ThisTask.Succeed();
        else ThisTask.Fail();
    }
    [Task]
    void IsEnemyAlive()
    {
        if (focusedEnemy.health > 0) ThisTask.Succeed();
        else ThisTask.Fail();
    }
    [Task]
    void EnemiesInRange()
    {
        //Implementar comprobación de enemigos a rango
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
            if (currentNode.distance < maxMovement)
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
        if (enemiesInRangeList.Any()) ThisTask.Succeed();
        else ThisTask.Fail();

    }
    [Task]
    void ChooseCloserEnemy()
    {
        //Implementar lógica de elección de enemigo
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

        ThisTask.Succeed();
    }
    [Task]
    void ChooseInjured()
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
    }
    [Task]
    void BattleCryActive()
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
        if (worked == false) ThisTask.Fail();
    }

    [Task]
    void CanIAttack()
    {
        if (focusedEnemy && !getAttackPerformed())
        {
            ThisTask.Succeed();
        }
        else ThisTask.Fail();
    }

    [Task]
    void InAttackRange()
    {
        //Si la distancia entre focusedEnemy y Yo es =<1 -> ThisTask.Succeed()  DONE
        //Considero distancia Manhattan
        var vector = focusedEnemy.GetGridSpace().gridPosition - GetGridSpace().gridPosition;
        var distance = Mathf.Abs(vector.x) + Mathf.Abs(vector.y) + Mathf.Abs(vector.z);

        if (distance > 1)//En lugar de 1, enemigo deberia tener su variable rango
        {
            ThisTask.Succeed();
        }
        else ThisTask.Fail();
    }
    [Task]
    void Attack()
    {
        //Correr animación de ataque mirando al enemigo
        focusedEnemy.health -= damage;
        ThisTask.Succeed();
    }

    [Task]
    void CanIMove()
    {
        if (movement > 0) ThisTask.Succeed();
        else ThisTask.Fail();
    }
    [Task]
    void GetCloser()
    {
        //Implementar camino con el máximo movimiento posible hasta el enemigo DONE?
        //Lo mejor sería implementar un A*, me falta tiempo
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

        ThisTask.Succeed();
    }

    [Task]
    void EndTurn()
    {
        EnemyManager.Instance.enemyTurnEnd();
        ThisTask.Succeed();
    }

    [Task]
    void IsMyTurn()
    {
        if (realizandoTurno)
            ThisTask.Succeed();
    }
}
