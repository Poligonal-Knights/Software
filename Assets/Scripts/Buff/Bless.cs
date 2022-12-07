using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless : Buff
{
    const int deafaultDuration = 2;

    int movementIncrement = 1;
    int pushStrengthIncrement = 1;
    int healthIncrement = 5;
    int defenseIncrement = 1;
    int damageIncrement = 1;

    public Bless(PJ owner, int turnsDuration = deafaultDuration) : base(owner, turnsDuration) { }

    protected override void Init()
    {
        Debug.Log(Owner + " Blessed");
        //Animacion de inicio

        if (Owner is Ally ally)
        {
            ally.movement += movementIncrement;
            ally.maxMovement += movementIncrement;
            ally.pushStrength += pushStrengthIncrement;
            ally.maxHealth += healthIncrement;
            ally.Heal(healthIncrement);
            ally.defense += defenseIncrement;
            ally.damage += damageIncrement;
        }
    }

    protected override void Finish()
    {
        if (Owner is Ally ally)
        {
            ally.movement -= movementIncrement;
            ally.maxMovement -= movementIncrement;
            ally.pushStrength -= pushStrengthIncrement;
            ally.maxHealth -= healthIncrement;
            ally.health = Mathf.Min(ally.health, ally.maxHealth);
            ally.defense -= defenseIncrement;
            ally.damage -= damageIncrement;
            base.Finish();
        }
        //Animacion de finish
    }

    protected override bool CanOwnerHaveThisBuff()
    {
        return Owner is Ally;
    }

    protected override void OnChangeTurn()
    {
        base.OnChangeTurn();
        //Animacion de cada turno
    }
}
