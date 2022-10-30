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
    protected bool IsDying;

    protected Queue<GridSpace> MovementsToDo = new Queue<GridSpace>();
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
            destination = MovementsToDo.Dequeue();
        }
        if (IsMoving)
        {
            var step = 20 * Time.deltaTime; // calculate distance to move
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

        if (space.gridPosition.y == 0) Destroy(gameObject);
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
        List<GridSpace> movements = new List<GridSpace>();
        var actualNode = finalDestination.node;
        while (actualNode.HasParent())
        {
            movements.Add(actualNode.space);
            actualNode = actualNode.parent;
        };
        movements.Add(actualNode.space);
        movements.Reverse();
        MovementsToDo = new Queue<GridSpace>(movements);
        space.SetEntity(null);
    }

    public virtual void CalculateFall()
    {
        UpdateGridSpace();
        var actualPosition = space;
        while (actualPosition.neighbors["down"] !=null && !actualPosition.neighbors["down"].HasBlock())
        {
            actualPosition = actualPosition.neighbors["down"];
            MovementsToDo.Enqueue(actualPosition);
        }
    }

    public virtual void CalculateFallFrom(GridSpace start)
    {
        UpdateGridSpace(start);
        var actualPosition = space;
        while (actualPosition.neighbors["down"] != null && !actualPosition.neighbors["down"].HasBlock())
        {
            actualPosition = actualPosition.neighbors["down"];
            MovementsToDo.Enqueue(actualPosition);
        }
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
