using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearTrap : Activatable
{
    public override void Activate()
    {
        base.Activate();
        //Animacion de activar
    }

    public override void Deactivate()
    {
        base.Deactivate();

    }

    protected override void SetActivatableSpaces()
    {
        space.AddActivatable(this);
    }
}
