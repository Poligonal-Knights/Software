using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PJ : Entity
{
    public Sprite frontSprite;
    public Sprite backSprite;

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
    public int defense = 0;

    //States
    public bool IsMoving;
    protected bool IsDying;
    public Vector2 orientation;
    SpriteRenderer spriteRenderer;
    //bool inHalf = false;

    public Queue<GridSpace> MovementsToDo = new Queue<GridSpace>();
    GridSpace destination;

    HashSet<Buff> buffs = new HashSet<Buff>();

    public UnityEvent MovementEvent = new UnityEvent();
    bool InvokeMovementEvent = false;

    protected override void Awake()
    {
        base.Awake();
        IsMoving = false;
        IsDying = false;
        CanJump = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        //IsMoving = false;
        //IsDying = false;
        //CanJump = false;
        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Init()
    {
        base.Init();
        destination = space;
        orientation = Vector2.down;
        UpdateOrientation();
        GetComponentInChildren<Animator>().Play("Idle", -1, Random.value );
    }

    protected override void Update()
    {
        if (!IsMoving && MovementsToDo.Any())
        {
            IsMoving = true;
            //var previousDestination;
            destination = MovementsToDo.Dequeue();
            var or = destination.GetWorldPosition() - transform.position;
            if (or.x != 0 || or.z != 0)
            {
                orientation = new Vector2(or.x, or.z);
                InvokeMovementEvent = true;
            }
            else InvokeMovementEvent = true;
            UpdateOrientation();
            //MovementEvent.Invoke();
        }
        if (IsMoving)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination.GetWorldPosition(), step);
            //Cositas de los halfs
            if (destination.neighbors["down"]?.GetEntity() is Half)
            {
                this.transform.Find("Sprite").transform.localPosition = new Vector3(0, -0.5f, 0);
            }
            else
            {
                this.transform.Find("Sprite").transform.localPosition = new Vector3(0, 0, 0);
            }
            //Terminado cositas de los halfs
            if (Vector3.Distance(transform.position, destination.GetWorldPosition()) < 0.001f)
            {
                transform.position = destination.GetWorldPosition();
                if (destination.GetEntity() is null)
                {
                    UpdateGridSpace();
                }
                if (InvokeMovementEvent) MovementEvent.Invoke();
                //transform.position = destination.GetPJPlacement();
                IsMoving = false;
                if (!MovementsToDo.Any())
                {
                    destination = null;
                    LogicManager.Instance.PJFinishedMoving();
                    //UpdateGridSpace();
                    if (space.gridPosition.y == 0) Die();
                }
            }
        }
        /*else if (!MovementsToDo.Any())
        {
            if (health <= 0)
            {
                Debug.Log("tengo movimientos antes de morir: " + MovementsToDo.Any());
                Die();
            }
        }*/
    }

    protected override void UpdateGridSpace()
    {
        base.UpdateGridSpace();
        space.ActivateActivatables(this);
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
        var cont = 0;
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
                cont++;
            }
            ReduceMovement(cont + 1);
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

    public void UpdateOrientation()
    {
        //var or = destination.GetWorldPosition() - transform.position;
        //orientation = new Vector2(or.x, or.z);
        var cam = Camera.main.transform.position - transform.position;
        var camera = new Vector2(cam.x, cam.z);
        var angle = Vector2.SignedAngle(orientation, camera);
        spriteRenderer.sprite = Math.Abs(angle) < 90.0f ? frontSprite : backSprite;
        spriteRenderer.flipX = angle > 0 ? true : false;

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

    public virtual void CalculateFallFrom(GridSpace start, Vector3Int direction)
    {
        //UpdateGridSpace(start);
        var currentPosition = start;
        bool stop = false;
        while (currentPosition.neighbors["down"] != null && !stop)
        {
            var downEntity = currentPosition.neighbors["down"].GetEntity();
            if (downEntity is PJ pj)
            {
                stop = true;
                var newDestination = GridManager.Instance.GetGridSpace(currentPosition.gridPosition + direction);
                MovementsToDo.Enqueue(newDestination);
                CalculateFallFrom(newDestination, direction);
            }
            else if (downEntity is Block)
            {
                this.DealDamage(2);
                stop = true;
            }
            else
            {
                currentPosition = currentPosition.neighbors["down"];
                MovementsToDo.Enqueue(currentPosition);
            }
        }

    }

    public virtual void Die()
    {
        space.SetEntity(null);
        IsDying = true;
        GameManager.Instance.RemovePJ(this);
        Destroy(gameObject, .5f);
    }

    protected override void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
    }

    public virtual void DealDamage(int damage, PJ attacker = null, PJ damageTo = null)
    {
        StartCoroutine(SufferDamage(damage, damageTo));
    }
    IEnumerator SufferDamage(int damage, PJ damageTo)
    {
        if (!MovementsToDo.Any() && !IsMoving)
        {
            Debug.Log("Movimientos finalizados antes del daño");
            if(!damageTo)
            {
                Debug.Log("Debería hacerme daño; " + damage);
                // GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);
                // StartCoroutine(GetColorBack());
                GetComponentInChildren<Animator>().SetTrigger("ReceiveDamage");
                AudioManager.Instance.Play("RecibirDano");
                health -= (damage - defense); 
                if (health <= 0)
                {
                    Debug.Log("tengo movimientos antes de morir: " + MovementsToDo.Any());
                    Die();
                }
            }
            else
            {
                damageTo.DealDamage(damage);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(SufferDamage(damage, damageTo));
            Debug.Log("Se está moviendo = "+ IsMoving + "Tiene donde moverse = "+ MovementsToDo.Any());
        }
        
        
    }

    IEnumerator GetColorBack()
    {
        yield return new WaitForSeconds(.5f);
        GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    public void Heal(int healedHealth)
    {
        //Debug.Log("OMG! I was healed!");
        if (health < maxHealth)
        {
            AudioManager.Instance.Play("Healing");
            GetComponentInChildren<SpriteRenderer>().color = new Color(0, 1, 0);
            StartCoroutine(GetColorBack());
        }
        health = Mathf.Min(health + healedHealth, maxHealth);
    }

    public bool getAttackPerformed()
    {
        return attackPerformed;
    }

    public void setAttackPerformed(bool setter)
    {
        attackPerformed = setter;
        Debug.Log("Ataque realizado: " + attackPerformed);
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

    public void SetMovement(int setTo)
    {
        movement = setTo;
    }

    public void ReduceMovement(int amount)
    {
        SetMovement(movement - amount);
    }

    public void ResetMovement()
    {
        SetMovement(maxMovement);
    }

    protected override void OnChangeTurn()
    {
        base.OnChangeTurn();
        ResetMovement();
        attackPerformed = false;
    }
}
