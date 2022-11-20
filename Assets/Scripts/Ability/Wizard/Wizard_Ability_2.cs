using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Prisa
public class Wizard_Ability_2 : Ability
{
    public Wizard_Ability_2(PJ owner) : base(owner) { }

    public override void Preview()
    {
        readyToConfirm = true;
        Debug.Log("RushHab");
    }

    public override void Confirm()
    {
        foreach (var ally in Object.FindObjectsOfType<Ally>())
        {
            new Rush(ally);
        }
    }

    public override void Cancel()
    {
        base.Cancel();
    }
}
