using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Buff
{
    const int deafaultDuration = 2;

    int damagePerMovement = 1;

    public Poison(PJ owner, int turnsDuration = deafaultDuration) : base(owner, turnsDuration) { }

    protected override void Init()
    {
        if (Owner is Enemy enemy)
        {
            enemy.MovementEvent.AddListener(DamagePerMovement);
        }
    }

    protected override void Finish()
    {
        if (Owner is Enemy enemy)
        {
            enemy.MovementEvent.RemoveListener(DamagePerMovement);
            enemy.MovementEvent.RemoveListener(DamagePerMovement);
            base.Finish();
        }
    }

    void DamagePerMovement()
    {
        Owner.DealDamage(damagePerMovement);
    }

    protected override bool CanOwnerHaveThisBuff()
    {
        return Owner is Enemy;
    }
}
