using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : Buff
{
    const int deafaultDuration = 2;
    int defenseIncrement = -1;
    int damageIncrement = -1;

    public Curse(PJ owner, int turnsDuration = deafaultDuration) : base(owner, turnsDuration) { }

    protected override void Init()
    {
        if (Owner is Ally ally)
        {
            ally.defense += defenseIncrement;
            ally.damage += damageIncrement;
        }
    }

    protected override void Finish()
    {
        if (Owner is Ally ally)
        {
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
