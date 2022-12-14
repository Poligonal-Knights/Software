using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Ability_4 : Ability
{
    public Wizard_Ability_4(Wizard owner, int energyRequired = 2) : base(owner, energyRequired) { }

    public override void Preview()
    {
        base.Preview();
        UIManager.Instance.ShowAbilityNonDefined();
    }
    
    public override void Cancel()
    {
        base.Cancel();
        UIManager.Instance.HideAbilityNonDefined();
    }
}


