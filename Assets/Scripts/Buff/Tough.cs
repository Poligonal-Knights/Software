using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tough : Buff
{
    const int deafaultDuration = 2;

    int defenseIncrement = 1;

    public Tough(PJ owner, int turnsDuration = deafaultDuration) : base(owner, turnsDuration) { }

    protected override void Init()
    {
        if (Owner is Enemy enemy)
        {
            AudioManager.Instance.Play("Buff");
            enemy.defense += defenseIncrement;
        }
    }

    protected override void Finish()
    {
        if (Owner is Enemy enemy)
        {
            enemy.defense -= defenseIncrement;
            base.Finish();
        }
    }

    protected override bool CanOwnerHaveThisBuff()
    {
        return Owner is Enemy;
    }
}
