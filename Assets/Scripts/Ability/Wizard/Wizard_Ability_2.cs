using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Prisa
public class Wizard_Ability_2 : Ability
{
    public Wizard_Ability_2(PJ owner) : base(owner) { EnergyConsumed = 2; }

    public override void Preview()
    {
        readyToConfirm = true;
        Debug.Log("RushHab");
        foreach (var ally in GameManager.Instance.allies)
        {
            AddHealedSpace(ally.GetGridSpace());
        }
    }

    public override void Confirm()
    {
        base.Confirm();

        foreach (var ally in GameManager.Instance.allies)
        {
            new Rush(ally);
        }
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }

    public override void Cancel()
    {
        base.Cancel();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }
}
