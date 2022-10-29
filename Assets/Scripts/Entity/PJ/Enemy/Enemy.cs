using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : PJ
{
    public bool realizandoTurno=false;
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
        if (realizandoTurno && !MovementsToDo.Any())
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
            if (chosenMove.neighbors["right"].IsPassable())
            {
                possibleMoves.Add(chosenMove.neighbors["right"]);
            }

            if (chosenMove.neighbors["left"].IsPassable())
            {
                possibleMoves.Add(chosenMove.neighbors["left"]);
            }

            if (chosenMove.neighbors["forward"].IsPassable())
            {
                possibleMoves.Add(chosenMove.neighbors["forward"]);
            }
            if (chosenMove.neighbors["back"].IsPassable())
            {
                possibleMoves.Add(chosenMove.neighbors["back"]);
            }

            chosenMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
            movimientosEnOrden.Add(chosenMove);
            possibleMoves.Clear();
        }

        for (int i = movimientosEnOrden.Count - 1; i >= 0; i--)
        {
            MovementsToDo.Push(movimientosEnOrden[i]);
        }

        realizandoTurno = true;
    }

    protected virtual void attackAI()
    {
        
    }
}
