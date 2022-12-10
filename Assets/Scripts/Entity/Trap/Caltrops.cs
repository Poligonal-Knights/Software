using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caltrops : Activatable
{
    public int Damage;

    public override void Activate(PJ Activator)
    {
        if (Activator is Ally) return;
        base.Activate(Activator);
        //Animacion de activar
        Debug.LogWarning("CALTROPs ACTIVATED");
        Activator.DealDamage(Damage);
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
