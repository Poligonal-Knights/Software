using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless : Buff
{
    const int deafaultDuration = 2;

    int movementIncrement = 1;
    int pushStrengthIncrement = 1;
    int healthIncrement = 5;

    public Bless(PJ owner, int turnsDuration = deafaultDuration) : base(owner, turnsDuration) { }

    protected override void Init()
    {
        Debug.Log(Owner + " Blessed");

        if (Owner is Ally ally)
        {
            ally.movement += movementIncrement;
            ally.maxMovement += movementIncrement;
            ally.pushStrength += pushStrengthIncrement;
            ally.maxHealth += healthIncrement;
            ally.Heal(healthIncrement);
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
            base.Finish();
        }
    }

    protected override bool CanOwnerHaveThisBuff()
    {
        return Owner is Ally;
    }
}
