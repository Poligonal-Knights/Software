using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ally : PJ
{
    //Stats
    public int energy;
    public int maxEnergy;
    //public int trapBonusDamage;
    public int pushStrength;

    //States
    bool invencibility;
    bool reactionAvailable;
    public bool deniedReaction = false; //Ha sido denegada la reacción actual en concreto.

    protected int AbilitySelected;
    protected GridSpace spaceSelected;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (reactionAvailable)
        {
            foreach (var neighbor in space.neighbors.Values)
            {
                if (neighbor.gridPosition.y == space.gridPosition.y)
                {
                    if (neighbor.GetEntity() is Enemy enemy && enemy.beingPushed && !LogicManager.Instance.reactionAbility.getAllies().Contains(this))
                    {
                        Debug.Log("Reacción iniciada por " + this);
                        LogicManager.Instance.reactionAbility.Engage(this, enemy);
                    }
                }
            }
        }
    }

    public override bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        if (destination.GetEntity() is Enemy) return false;
        return base.CanMoveThere(start, destination);
    }

    public override void DealDamage(int damage, PJ attacker = null, PJ damageTo = null)
    {
        AudioManager.Instance.Play("AtaqueEnemigo");
        if (!invencibility)
        {
            base.DealDamage(damage, attacker);
            AudioManager.Instance.Play("RecibirDano");
        }
        else
        {
            
            AudioManager.Instance.Play("Invencibilidad");
        }
    }

    public void SetInvencibility(bool setTo)
    {
        invencibility = setTo;
        if(setTo) 
            AudioManager.Instance.Play("Buff");
    }

    public bool IsInvencible()
    {
        return invencibility;
    }

    public bool CanReact()
    {
        return reactionAvailable;
    }

    public void SetReactionAvailable(bool setTo)
    {
        reactionAvailable = setTo;
    }

    protected override void OnChangeTurn()
    {
        if (TurnManager.Instance.IsPlayerTurn()) //Cambio de ronda
        {
            //foreach buff in buffs
            //buff.cont--;

            //Debug.Log(this +" : Invencibilidad " + invencibility);
            SetInvencibility(false);
            SetReactionAvailable(true);
        }
        base.OnChangeTurn();
    }
    public void ReduceEnergy(int reduceAmount)
    {
        Debug.Log("Energy reduced by: " + reduceAmount);
        energy -= reduceAmount;
    }
}
