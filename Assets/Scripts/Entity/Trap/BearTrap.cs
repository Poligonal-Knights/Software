using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearTrap : Activatable
{
    public int Damage;

    public override void Activate(PJ Activator)
    {
        base.Activate(Activator);
        //Animacion de activar
        Debug.LogWarning("BEARTRAP ACTIVATED");
        Activator.DealDamage(Damage);
        Activator.SetMovement(0);
        Activator.MovementsToDo.Clear();
        Destroy(gameObject, 0.4f);
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
