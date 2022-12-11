using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTrap : Activatable
{
    public int Damage;

    public override void Activate(PJ Activator)
    {
        if (Activator is Ally) return;
        base.Activate(Activator);
        //Animacion de activar
        Debug.LogWarning("ROPETRAP ACTIVATED");
        Activator.DealDamage(Damage);
        Activator.SetMovement(0);
        Activator.MovementsToDo.Clear();
        Destroy(gameObject, 0.5f);
    }

    public override void Deactivate()
    {
        base.Deactivate();

    }

    protected override void SetActivatableSpaces()
    {
        AddActivatableSpace(space);
    }
}
