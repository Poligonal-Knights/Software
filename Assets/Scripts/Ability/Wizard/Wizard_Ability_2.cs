using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Prisa
public class Wizard_Ability_2 : Ability
{
    public Wizard_Ability_2(Wizard owner, int energyRequired = 2) : base(owner, energyRequired) { }

    public override void Preview()
    {
        base.Preview();
        readyToConfirm = true;
        foreach (var ally in GameManager.Instance.allies)
        {
            AddHealedSpace(ally.GetGridSpace());
        }
    }

    public override void Confirm()
    {
        foreach (var ally in GameManager.Instance.allies)
        {
            new Rush(ally);
        }
        base.Confirm();
    }

    public override void Cancel()
    {
        base.Cancel();
    }
}
