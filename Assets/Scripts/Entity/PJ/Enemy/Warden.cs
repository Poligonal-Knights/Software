using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;
using System.Linq;

public class Warden : Enemy
{
    List<PJ> enemiesInRangeList = new List<PJ>();
    List<PJ> alliesInRangeList = new List<PJ>();
    //private List<Enemy> myAllies;
    private PJ focusedEnemy = null;
    private Enemy protectedAlly = null;

    private bool Offense = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //myAllies = FindObjectsOfType<Enemy>().ToList();
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
        focusedEnemy = null;
        enemiesInRangeList.Clear();
        alliesInRangeList.Clear();
        //protectedAlly = null;
        EnemyManager.Instance.enemyTurnEnd();
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
        //Comprobar los enemigos que hay a rango de explotar y devolver false si no hay ninguno
        var spacesInMovementRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        foreach (var enemy in GameManager.Instance.allies)
        {
            foreach (var s in spacesInMovementRange)
            {
                var distance = GridSpace.ManhattanDistance2D(s, enemy.GetGridSpace());
                if (distance <= attackRange)
                {
                    enemiesInRangeList.Add(enemy);
                    break;
                }
            }
        }
        return false;
    }

    [Task]
    bool IsAllyChosen()
    {
        return protectedAlly != null && protectedAlly.health > 0;
    }

    [Task]
    bool IsAnyoneAlive()
    {
        foreach (var myAlly in GameManager.Instance.enemies)
        {
            if (myAlly.health > 0 && myAlly is not Warden)
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
        var spacesInRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        foreach (var s in spacesInRange)
        {
            foreach (var ally in GameManager.Instance.enemies)
            {
                var distance = GridSpace.ManhattanDistance2D(s, ally.GetGridSpace());
                if (distance <= attackRange)
                {
                    alliesInRangeList.Add((ally));
                    return true;
                }
            }
        }
        return false;
    }

    //COMPROBACIÓN NUEVO PROTEGIDO
    //Aquí se elige el nuevo protegido mirando si existe alguno de estos
    //en la lista y si es así, juntarlos e ir al más cercano
    [Task]
    bool IsThereAnyHealer()
    {
        List<PJ> allies = new List<PJ>();
        foreach (var ally in alliesInRangeList)
            if (ally is Healer) allies.Add(ally);

        if (!allies.Any()) return false;
        protectedAlly = SelectCloser(allies);
        return true;
    }
    [Task]
    bool IsThereAnyMage()
    {
        List<PJ> allies = new List<PJ>();
        foreach (var ally in alliesInRangeList)
            if (ally is Mage) allies.Add(ally);

        if (!allies.Any()) return false;
        protectedAlly = SelectCloser(allies);
        return true;
    }
    [Task]
    bool IsThereAnyArcher()
    {
        List<PJ> allies = new List<PJ>();
        foreach (var ally in alliesInRangeList)
            if (ally is Archer) allies.Add(ally);

        if (!allies.Any()) return false;
        protectedAlly = SelectCloser(allies);
        return true;
    }
    [Task]
    bool IsThereAnyTrashmob()
    {
        List<PJ> allies = new List<PJ>();
        foreach (var ally in alliesInRangeList)
            if (ally is TrashMob) allies.Add(ally);

        if (!allies.Any()) return false;
        protectedAlly = SelectCloser(allies);
        return true;
    }

    Enemy SelectCloser(List<PJ> allies)
    {
        PJ closerAlly = null;
        var goalSpace = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere, candidate =>
        {
            if(candidate.GetEntity() is null)
            {
                foreach (var ally in allies)
                {
                    var distance = GridSpace.ManhattanDistance2D(candidate, ally.GetGridSpace());
                    if (distance <= attackRange)
                    {
                        closerAlly = ally;
                        return true;
                    }
                }
            }
            return false;
        });
        return closerAlly as Enemy;
    }

    [Task]
    bool ChooseCloserAlly()
    {
        //CUANDO NO LLEGO A NINGUNO
        //Nombrar como protegido el aliado más cercano y ya.
        BFS.GetGoalGridSpace(space, int.MinValue, CanMoveThere, candidate =>
        {
            if (candidate.GetEntity() is null)
            {
                foreach (var ally in GameManager.Instance.enemies)
                {
                    var distance = GridSpace.ManhattanDistance2D(candidate, ally.GetGridSpace());
                    if (distance <= attackRange)
                    {
                        protectedAlly = ally;
                        return true;
                    }
                }
            }
            return false;
        });

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
        foreach (var enemy in GameManager.Instance.allies)
        {
            var distance = GridSpace.ManhattanDistance2D(protectedAlly.GetGridSpace(), enemy.GetGridSpace());
            if (distance <= 2) return true;
        }

        return false;
    }

    [Task]
    bool FocusedEnemyInRange()
    {
        //Comprobar si alguno de los enemigos a rango del
        //aliado está a rango de ser golpeado por el guardian
        //y marcarlo como focusedEnemy
        var espaciosARangoDelAliado = BFS.GetSpacesInRange(protectedAlly.GetGridSpace(), protectedAlly.movement, protectedAlly.CanMoveThere);
        List<PJ> enemigosARangoDelAliado = new List<PJ>();
        foreach (var enemy in GameManager.Instance.allies)
        {
            foreach (var space in espaciosARangoDelAliado)
            {
                var distance = GridSpace.ManhattanDistance2D(space, enemy.GetGridSpace());
                if (distance <= 2) //Comprobar más adelante
                {
                    enemigosARangoDelAliado.Add(enemy);
                    break;
                }
            }
        }
        var espaciosEnRangoPropio = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        var done = false;
        foreach (var space in espaciosEnRangoPropio)
        {
            foreach (var enemy in enemigosARangoDelAliado)
            {
                var distance = GridSpace.ManhattanDistance2D(space, enemy.GetGridSpace());
                if (distance <= attackRange)
                {
                    focusedEnemy = enemy;
                    done = true;
                    break;
                }
            }
            if (done) break;
        }
        return focusedEnemy;
    }

    [Task]
    bool GetCloserEnemyFocused()
    {
        if (!focusedEnemy)
        {
            return false;
        }
        var goalSpace = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere, candidate =>
        {
            if (candidate.GetEntity() is null)
            {
                var distance = GridSpace.ManhattanDistance2D(candidate, focusedEnemy.GetGridSpace());
                return distance <= attackRange;
            }
            return false;
        });

        var goalNode = goalSpace?.node;
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
        return goalNode is null ? false : true;
    }

    [Task]
    bool GetCloserProtectedAlly()
    {
        var goalSpace = BFS.GetGoalGridSpace(space, int.MaxValue, CanMoveThere, candidate =>
        {
            if (candidate.GetEntity() is null)
            {
                var distance = GridSpace.ManhattanDistance2D(candidate, protectedAlly.GetGridSpace());
                return distance <= attackRange;
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
    protected override bool Attack()
    {
        if (!getAttackPerformed())
            StartCoroutine(createDelayedLine());
        focusedEnemy.DealDamage(this.damage, this);
        base.Attack();
        return true;
    }

    IEnumerator createDelayedLine()
    {
        var _focusedEnemy = focusedEnemy.GetGridSpace().GetWorldPosition();
        var _gridSpace = this.GetGridSpace().GetWorldPosition();
        yield return new WaitForSeconds(0.25f);
        LineDrawer.DrawLine(_gridSpace, _focusedEnemy);
    }

    [Task]
    bool IsInOffense()
    {
        return Offense;
    }

    [Task]
    bool IsAllyInRange()
    {
        var spacesInRange = BFS.GetSpacesInRange(space, movement, CanMoveThere);
        foreach (var s in spacesInRange)
        {
            var distance = GridSpace.ManhattanDistance2D(s, protectedAlly.GetGridSpace());
            if (distance <= attackRange) return true;
        }

        return false;
    }

    [Task]
    bool DefenseBuff()
    {
        //Crear buffo de defensa para ProtectedAlly y el Warden
        new Tough(this);
        new Tough(protectedAlly);
        return true;
    }
    [Task]
    bool MovementBuff()
    {
        this.movement += 3;
        AudioManager.Instance.Play("Buff");
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
        foreach (PJ enemy in enemiesInRangeList)
            if (enemy is Knight knight && knight.UsingGritoDeBatalla())
            {
                focusedEnemy = enemy;
                return true;
            }
        return false;
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