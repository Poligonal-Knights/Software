using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;

public class Warden : Enemy
{
    List<PJ> enemiesInRangeList = new List<PJ>();
    private List<Enemy> myAllies;
    private PJ focusedEnemy = null;
    private Enemy protectedAlly = null;

    private bool Offense = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        myAllies = FindObjectsOfType<Enemy>().ToList();
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
    bool  IsMyTurn()
    {
        return realizandoTurno;
    }

    [Task]
    bool EnemiesInRange()
    {
        //Comprobar los enemigos que hay a rango de explotar y devolver false si no hay ninguno
        return false;
    }

    [Task]
    bool IsAllyChosen()
    {
        return protectedAlly != null && protectedAlly.health>0;
    }

    [Task]
    bool IsAnyoneAlive()
    {
        foreach (var ally in myAllies)
        {
            if (ally.health > 0 && ally is not Warden)
            {
                return true;
            }
        }
        return false;
    }

    [Task]
    bool AlliesInRange()
    {
        //Devolver si hay aliados en rango de movimiento
        //O quizás movBuff
        return false;
    }

    //COMPROBACIÓN NUEVO PROTEGIDO
    //Aquí se elige el nuevo protegido mirando si existe alguno de estos
    //en la lista y si es así, juntarlos e ir al más cercano
    [Task]
    bool IsThereAnyHealer()
    {
        return false;
    }
    [Task]
    bool IsThereAnyMage()
    {
        return false;
    }
    [Task]
    bool IsThereAnyArcher()
    {
        return false;
    }
    [Task]
    bool IsThereAnyTrashmob()
    {
        return false;
    }
    [Task]
    bool ChooseCloserAlly()
    {
        //Nombrar como protegido el aliado más cercano y ya.
        return true;
    }
    //FIN COMPROBACIÓN NUEVO PROTEGIDO
    [Task]
    bool ChangeToOffense()
    {
        Offense = true;
        this.damage += 3;
        this.defense = 0;
        this.maxMovement += 2;
        this.movement = maxMovement;
        return true;
    }

    [Task]
    bool EnemyInAllysRange()
    {
        //Devolver si existe a algún enemigo a 2 casillas del protegido 
        return true; 
    }

    [Task]
    bool FocusedEnemyInRange()
    {
        //Comprobar si alguno de los enemigos a rango del
        //aliado está a rango de ser golpeado por el guardian
        //y marcarlo como focusedEnemy
        
        return focusedEnemy;
    }
    
    [Task]
    bool GetCloserEnemyFocused()
    {
        return true;
    }
    
    [Task]
    bool GetCloserProtectedAlly()
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
    bool Attack()
    {
        focusedEnemy.DealDamage(this.damage);
        return true;
    }

    [Task]
    bool IsInOffense()
    {
        return Offense;
    }

    [Task]
    bool IsAllyInRange()
    {
        return false; 
    }

    [Task]
    bool DefenseBuff()
    {
        //Crear buffo de defensa para ProtectedAlly y el Warden
        return true;
    }
    [Task]
    bool MovementBuff()
    {
        this.movement += 3;
        return true;
    }
    //A partir de aquí se incluiran los metodos usados
    //por el comportamiento secundario
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
    bool InAttackRange()
    {
        if (focusedEnemy != null)
        {
            //Si la distancia entre focusedEnemy y Yo es =<1 -> ThisTask.Succeed()  DONE
            //Considero distancia Manhattan
            var vector = focusedEnemy.GetGridSpace().gridPosition - GetGridSpace().gridPosition;
            var distance = Mathf.Abs(vector.x) + Mathf.Abs(vector.y) + Mathf.Abs(vector.z);

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
    bool CanIMove()
    {
        return movement > 0;
    }
    
    [Task]
    bool ChooseCloserEnemy()
    {
        //Implementar lógica de elección de enemigo
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        HashSet<GridSpace> visitedSpaces = new HashSet<GridSpace>();
        bool goalFinded = false;
        foreach (var move in GetGridSpace().moves)
        {
            if (!goalFinded && !visitedSpaces.Contains(move))
            {
                if (move.GetEntity() is Ally ally)
                {
                    focusedEnemy = ally;
                    goalFinded = true;
                }
                else
                {
                    if (CanMoveThere(GetGridSpace(), move))
                    {
                        visitedSpaces.Add(move);
                        nodes.Enqueue(new BFS_Node(move, null, 1));
                    }
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
                    foreach (var submove in move.moves)
                    {
                        if (submove.GetEntity() is Ally ally)
                        {
                            focusedEnemy = ally;
                            goalFinded = true;
                        }
                        // else
                        //     nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                    }

                    if (!goalFinded)
                    {
                        visitedSpaces.Add(move);
                        nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                    }
                }
            }
        }
        return true;
    }
}