using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Ability_4 : Ability
{
    public Wizard_Ability_4(PJ owner) : base(owner) { EnergyConsumed = 2; }
    
    
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


