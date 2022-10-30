using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : PJ
{
    public bool realizandoTurno = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    public override void Init()
    {
        base.Init();
        maxMovement = 3;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (realizandoTurno && !MovementsToDo.Any() && !IsMoving)
        {
            //Turno finalizado
            realizandoTurno = false;
            gameManager.enemyManager.enemyTurnEnd();
        }
    }

    public virtual void EnemyAI()
    {
        movementAI();
        attackAI();
        // gameManager.enemyManager.enemyTurnEnd();
    }

    protected virtual void movementAI2()
    {
        BFS();
        var posibilities = FindObjectOfType<GridManager>().visitedSpaces;
        int chosenMove = Random.Range(0, posibilities.Count);
        MoveTo(posibilities[chosenMove]);
        realizandoTurno = true;
    }

    protected virtual void movementAI()
    {
        List<GridSpace> possibleMoves = new List<GridSpace>();
        GridSpace chosenMove = space;
        List<GridSpace> movimientosEnOrden = new List<GridSpace>();
        for (int i = 0; i < maxMovement; i++)
        {
            var directions = new[] { "left", "right", "forward", "back" };

            foreach(var direction in directions)
            {
                if (chosenMove.neighbors[direction].IsPassable() && chosenMove.neighbors[direction].GetEntity() is not Ally)
                {
                    possibleMoves.Add(chosenMove.neighbors[direction]);
                }
            }
            if (possibleMoves.Any())
            {
                chosenMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
                // movimientosEnOrden.Add(chosenMove);
                if(i != maxMovement - 1 || chosenMove.IsEmpty())
                MovementsToDo.Enqueue(chosenMove);
                possibleMoves.Clear();
            }
        }

        // for (int i = 0; i < movimientosEnOrden.Count; i++)
        // {
        //     MovementsToDo.Enqueue(movimientosEnOrden[i]);
        // }

        realizandoTurno = true;
    }

    protected virtual void attackAI()
    {

    }

    public virtual void BePushed(Vector3Int direction, int pushback, int extraDamage)
    {
        bool bumped = false;
        int i = 0;
        Debug.Log("oh no oh no stop it pls nooo");
        while (!bumped && i <= pushback)
        {
            i++;
            var pushedInto = gridManager.GetGridSpace(space.gridPosition + direction * i);
            if (pushedInto.IsEmpty() || pushedInto.HasTrap())
            {
                MovementsToDo.Enqueue(pushedInto);
                if (!pushedInto.IsPassable())
                {
                    Debug.Log("Intentando iniciar caida");
                    bumped = true;
                    CalculateFallFrom(pushedInto);
                }
            }
            else
            {
                Debug.Log("He sufrido " + extraDamage + " y mi vida es " + health);
                health -= extraDamage;
                Debug.Log("Mi vida final es " + health);
                var crashed = pushedInto.GetEntity();
                if (pushedInto.GetEntity() is Enemy)
                {
                    var enemyBumped = pushedInto.GetEntity() as Enemy;
                    enemyBumped.health -= extraDamage;
                }
                bumped = true;
            }
        }
    }

    public override void Die()
    {
        base.Die();
        if(realizandoTurno)gameManager.enemyManager.enemyTurnEnd();
        realizandoTurno = false;
        IsMoving = false;
        IsDying = true;
        gameManager.enemyManager.enemyList.Remove(this);
    }
}

