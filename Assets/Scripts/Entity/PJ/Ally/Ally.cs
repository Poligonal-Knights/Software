using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : PJ
{
    //Stats
    public int energy;
    public int trapBonusDamage;
    public int pushStrength;

    //States
    bool invencibility;

    protected int HabilitySelected;
    protected GridSpace spaceSelected;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        if (destination.GetEntity() is Enemy) return false;
        return base.CanMoveThere(start, destination);
    }

    public void SetInvencibility(bool setTo)
    {
        Debug.Log("Soy to guay");
        invencibility = setTo;
    }

    public bool IsInvencible()
    {
        return invencibility;
    }
}
