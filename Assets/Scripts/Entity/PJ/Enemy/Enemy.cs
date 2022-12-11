using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : PJ
{
    public bool realizandoTurno = false;

    public bool beingPushed = false;

    public int comboed = 0;
    public int attackRange;

    public bool weak;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    public override void Init()
    {
        base.Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!MovementsToDo.Any() && !IsMoving)
        {
            beingPushed = false;
            comboed = 0;
        }
        /*if (realizandoTurno && !MovementsToDo.Any() && !IsMoving)
        {
            //Turno finalizado
            realizandoTurno = false;
            EnemyManager.Instance.enemyTurnEnd();
        }*/
    }

    public virtual void EnemyAI()
    {
        movementAI();
        // gameManager.enemyManager.enemyTurnEnd();
    }

    protected virtual void movementAI()
    {
        List<GridSpace> possibleMoves = new List<GridSpace>();
        GridSpace chosenMove = space;
        List<GridSpace> movimientosEnOrden = new List<GridSpace>();
        for (int i = 0; i < maxMovement; i++)
        {
            var directions = new[] { "left", "right", "forward", "back" };

            foreach (var direction in directions)
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
                if (i != maxMovement - 1 || chosenMove.IsEmpty())
                    MovementsToDo.Enqueue(chosenMove);
                possibleMoves.Clear();
            }
        }
        realizandoTurno = true;
    }

    public virtual void BePushed(Vector3Int direction, int pushback, int extraDamage, Ally pushedBy)
    {
        LogicManager.Instance.reactionAbility.pushedBy = pushedBy;
        Debug.Log("Pushed");
        beingPushed = true;
        bool bumped = false;
        bool endOfGrid = false;
        int i = 0, spacesCounter = 0;
        while (!bumped && spacesCounter <= pushback && !endOfGrid)
        {
            Debug.Log("Bumped: " + bumped);
            i++;
            var pushedInto = GridManager.Instance.GetGridSpace(space.gridPosition + direction * i);
            if (!Oil.HasGridSpaceOil(pushedInto)) spacesCounter++;
            if (pushedInto is null)
            {
                endOfGrid = true;
            }
            else if (pushedInto.IsEmpty() || pushedInto.HasTrap())
            {
                MovementsToDo.Enqueue(pushedInto);
                //var IsReactionPosible = false;
                //foreach(var neighbor in pushedInto.neighbors.Values)
                //{
                //    if(neighbor.gridPosition.y == pushedInto.gridPosition.y)
                //    {
                //        IsReactionPosible = true;
                //    }
                //}
                //if(IsReactionPosible)
                //{
                //    bumped = true;

                //}
                if (!pushedInto.IsPassable())
                {
                    Debug.Log("Intentando iniciar caida");
                    bumped = true;
                    CalculateFallFrom(pushedInto, direction);
                }
            }
            else
            {
                Debug.Log("He sufrido " + extraDamage + " y mi vida es " + health);
                DealDamage(extraDamage);
                Debug.Log("Mi vida final es " + health);

                if (pushedInto.GetEntity() is Enemy enemyBumped)
                {
                    Debug.Log(this + " BUMP");
                    DealDamage(extraDamage, null, damageTo: enemyBumped);
                }
                bumped = true;
            }
        }
    }

    public override void Die()
    {
        base.Die();
        if (realizandoTurno) EnemyManager.Instance.enemyTurnEnd();
        realizandoTurno = false;
        IsMoving = false;
        //IsDying = true;
        EnemyManager.Instance.enemyList.Remove(this);
    }

    public override bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        if (destination.GetEntity() is Ally) return false;
        return base.CanMoveThere(start, destination);
    }

    public override void DealDamage(int damage, PJ attacker = null, PJ damageTo = null)
    {
        if (weak)
        {
            damage+= 1;
            weak = false;
        }
        base.DealDamage(damage+comboed, attacker, damageTo);
        //Debug.Log("The actual combo is " + comboed);
        //health -= comboed;
    }
}

