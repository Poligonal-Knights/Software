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
    public int maxHealth;
    public int health;
    public int damage;
    public bool CanJump;
    private bool attackPerformed;
    private float speed = 5;
    public float defaultSpeed;

    //States
    public bool IsMoving;
    protected bool IsDying;

    public Queue<GridSpace> MovementsToDo = new Queue<GridSpace>();
    GridSpace destination;

    HashSet<Buff> buffs = new HashSet<Buff>();

    protected override void Start()
    {
        base.Start();
        IsMoving = false;
        IsDying = false;
        maxMovement = 4;
        CanJump = false;
    }

    public override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        if (!IsMoving && MovementsToDo.Any())
        {
            IsMoving = true;
            destination = MovementsToDo.Dequeue();
        }
        if (IsMoving)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination.GetPJPlacement(), step);
            if (Vector3.Distance(transform.position, destination.GetPJPlacement()) < 0.001f)
            {
                if(destination.GetEntity() is null)
                {
                    UpdateGridSpace();
                }
                IsMoving = false;
                transform.position = destination.GetPJPlacement();
                if (!MovementsToDo.Any())
                {
                    LogicManager.Instance.PJFinishedMoving();
                    //UpdateGridSpace();
                    if (space.gridPosition.y == 0) Die();
                }
            }
        }
    }

    public virtual bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        if (CanJump || start.gridPosition.y == destination.gridPosition.y)
        {
            return true;
        }
        else if (start.neighbors["down"].GetEntity() is Half || destination.neighbors["down"].GetEntity() is Half)
        {
            if (Mathf.Abs(start.gridPosition.y - destination.gridPosition.y) < 2)
            {
                return true;
            }
        }
        return false;
    }

    public virtual void MoveTo(GridSpace finalDestination)
    {
        List<GridSpace> movements = new List<GridSpace>();
        var currentNode = finalDestination.node;

        if (!finalDestination.Equals(space))
        {
            while (currentNode.HasParent())
            {
                movements.Add(currentNode.space);
                if (currentNode.space.gridPosition.y != currentNode.parent.space.gridPosition.y)
                {
                    Vector3Int interDestination;
                    if (currentNode.space.gridPosition.y < currentNode.parent.space.gridPosition.y)
                    {
                        interDestination = currentNode.space.gridPosition;
                        interDestination.y = currentNode.parent.space.gridPosition.y;
                    }
                    else
                    {
                        interDestination = currentNode.parent.space.gridPosition;
                        interDestination.y = currentNode.space.gridPosition.y;
                    }
                    movements.Add(GridManager.Instance.GetGridSpace(interDestination));
                }
                currentNode = currentNode.parent;
            }

            movements.Add(currentNode.space);
            //if (currentNode.space.gridPosition.y != currentNode.parent.space.gridPosition.y)
            if (currentNode.space.gridPosition.y != space.gridPosition.y)
            {
                Vector3Int interDestination;
                if (currentNode.space.gridPosition.y < space.gridPosition.y)
                {
                    interDestination = currentNode.space.gridPosition;
                    interDestination.y = space.gridPosition.y;
                }
                else
                {
                    interDestination = space.gridPosition;
                    interDestination.y = currentNode.space.gridPosition.y;
                }
                movements.Add(GridManager.Instance.GetGridSpace(interDestination));
            }
            movements.Reverse();
            MovementsToDo = new Queue<GridSpace>(movements);
        }
    }

    public virtual void CalculateFall()
    {
        //UpdateGridSpace();
        var currentPosition = space;
        while (currentPosition.neighbors["down"] != null && !currentPosition.neighbors["down"].HasBlock())
        {
            currentPosition = currentPosition.neighbors["down"];
            MovementsToDo.Enqueue(currentPosition);
        }
    }

    public virtual void CalculateFallFrom(GridSpace start)
    {
        //UpdateGridSpace(start);
        var currentPosition = start;
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

    public void Heal(int healedHealth)
    {
        Debug.Log("OMG! I was healed!");
        health = Mathf.Max(health + healedHealth, maxHealth);
    }

    public bool getAttackPerformed()
    {
        return attackPerformed;
    }

    public void setAttackPerformed(bool setter)
    {
        attackPerformed = setter;
    }

    protected override void OnChangeTurn()
    {
        base.OnChangeTurn();
    }

    public void AddBuff(Buff addedBuff)
    {
        Debug.Log(this + "Buff received");
        buffs.Add(addedBuff);
    }

    public void RemoveBuff(Buff addedBuff)
    {
        Debug.Log(this + "Buff removed");
        buffs.Add(addedBuff);
    }

    public void SetSpeed(float setTo)
    {
        speed = setTo;
    }

    public float GetSpeed(float setTo)
    {
        return speed;
    }

    public void ResetSpeed()
    {
        SetSpeed(defaultSpeed);
    }
}
