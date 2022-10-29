using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PJ : Entity
{
    //Stats
    protected int maxMovement;
    protected int movement;
    protected int health;

    //States
    protected bool IsMoving;

    protected Stack<GridSpace> MovementsToDo = new Stack<GridSpace>();
    GridSpace destination;

    protected override void Start()
    {
        base.Start();
        IsMoving = false;
    }

    public override void Init()
    {
        base.Init();
        maxMovement = 4;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!IsMoving && MovementsToDo.Any())
        {
            IsMoving = true;
            destination = MovementsToDo.Pop();
        }
        if (IsMoving)
        {
            var step = 3 * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, destination.GetWorldPosition(), step);
            if (Vector3.Distance(transform.position, destination.GetWorldPosition()) < 0.001f)
            {
                IsMoving = false;
                if (!MovementsToDo.Any())
                {
                    gameManager.logicManager.PJFinishedMoving();
                    UpdateGridSpace();
                }
            }
        }
    }

    public void FindPath(Vector3Int goal)
    {
        BFS();
    }

    public void BFS()
    {
        space.SetVisited(true);
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        foreach (var move in space.moves.Values)
        {
            if (!move.visited && CanMoveThere(space, move))
            {
                move.SetVisited(true);
                nodes.Enqueue(new BFS_Node(move, null, 1));

                //Animation
                if (!(move.GetEntity() is PJ))
                {
                    Block b = move.neighbors["down"].GetEntity() as Block;
                    b.SetInPreviewMode();
                }
            }
        }
        while (nodes.Any())
        {
            var currentNode = nodes.Dequeue();
            if (currentNode.distance < maxMovement)
            {
                foreach (var move in currentNode.space.moves.Values)
                {
                    if (!move.visited && CanMoveThere(currentNode.space, move))
                    {
                        if (!(currentNode.distance + 1 == maxMovement && move.GetEntity() is PJ))
                        {
                            move.SetVisited(true);
                            nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                            //Animation
                            if(!(move.GetEntity() is PJ))
                            {
                                Block b = move.neighbors["down"].GetEntity() as Block;
                                b.SetInPreviewMode();
                            }
                        }
                    }
                }
            }
        }
    }

    protected virtual bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        if (start.gridPosition.y == destination.gridPosition.y) return true;
        return false;
    }


    protected virtual void Movement(GridSpace start, GridSpace destination)
    {
        start.GetWorldPosition();
    }

    public virtual void MoveTo(GridSpace finalDestination)
    {
        var actualNode = finalDestination.node;
        while (actualNode.HasParent())
        {
            MovementsToDo.Push(actualNode.space);
            actualNode = actualNode.parent;
        };
        MovementsToDo.Push(actualNode.space);
        space.SetEntity(null);
    }

    protected override void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
    }

    // public void setHealth(int newHealth)
    // {
    //     health = newHealth;
    // }
    //
    // public int getHealth()
    // {
    //     return health;
    // }
    //
    // public void setMovement(int newMovement)
    // {
    //     speed=newMovement;
    // }
    //
    // public int getMovement()
    // {
    //     return speed;
    // }
}
