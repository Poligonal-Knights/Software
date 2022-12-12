using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spikes : Activatable
{
    public int Damage;

    public override void Activate(PJ Activator)
    {
        
        base.Activate(Activator);
        //Animacion de activar
        Debug.LogWarning("SPIKES ACTIVATED");
        Activator.DealDamage(Damage);
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
