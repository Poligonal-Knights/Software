using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PJ : Entity
{
    //Stats
    public int maxMovement;
    public int movement;
    public int health;
    public int health;
    private bool attackPerformed;

    //States
    protected bool IsMoving;
    protected bool IsDying;

    protected Queue<GridSpace> MovementsToDo = new Queue<GridSpace>();
    GridSpace destination;

    protected override void Start()
    {
        base.Start();
        IsMoving = false;
        IsDying = false;
        maxMovement = 4;
    }

    public override void Init()
    {
        base.Init();
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
            var step = 5 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination.GetWorldPosition(), step);
            if (Vector3.Distance(transform.position, destination.GetWorldPosition()) < 0.001f)
            {
                IsMoving = false;
                transform.position = destination.GetWorldPosition();
                if (!MovementsToDo.Any())
                {
                    LogicManager.Instance.PJFinishedMoving();
                    UpdateGridSpace();
                    if (space.gridPosition.y == 0) Die();
                }
            }
        }
    }

    public virtual bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        if (start.gridPosition.y == destination.gridPosition.y) return true;
        return false;
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
        var currentPosition = space;
        while (currentPosition.neighbors["down"] != null && !currentPosition.neighbors["down"].HasBlock())
        {
            currentPosition = currentPosition.neighbors["down"];
            MovementsToDo.Enqueue(currentPosition);
        }
    }

    public virtual void CalculateFallFrom(GridSpace start)
    {
        UpdateGridSpace(start);
        var currentPosition = space;
        while (currentPosition.neighbors["down"] != null && !currentPosition.neighbors["down"].HasBlock())
        {
            currentPosition = currentPosition.neighbors["down"];
            MovementsToDo.Enqueue(currentPosition);
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
        IsDying = true;
    }

    protected override void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
    }

    public virtual void DealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    public bool getAttackPerformed()
    {
        return attackPerformed;
    }

    public void setAttackPerformed(bool setter)
    {
        attackPerformed = setter;
    }
}
