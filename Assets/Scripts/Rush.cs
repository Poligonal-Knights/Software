using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : Buff
{
    const int deafaultDuration = 1;

    int movementIncrement = 1;
    int pushStrengthIncrement = 1;

    //public Rush(PJ owner) : base(owner, turnsDuration) { }
    public Rush(PJ owner, int turnsDuration = deafaultDuration) : base(owner, turnsDuration) { }

    protected override void Init()
    {
        //base.Init();
        Debug.Log(Owner + " got rushed");

        if (Owner is Ally ally)
        {
            ally.movement += movementIncrement;
            ally.maxMovement += movementIncrement;
            ally.pushStrength += pushStrengthIncrement;
        }
    }

    protected override void Finish()
    {
        if (Owner is Ally ally)
        {
            ally.movement -= movementIncrement;
            ally.maxMovement -= movementIncrement;
            ally.pushStrength -= pushStrengthIncrement;
            base.Finish();
        }
    }

    protected override bool CanOwnerHaveThisBuff()
    {
        return Owner is Ally;
    }
}
