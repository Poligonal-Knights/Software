using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public bool SpecialAbility = false;
    public int EnergyRequired;

    public static HashSet<GridSpace> SelectableSpaces = new HashSet<GridSpace>();
    public static HashSet<GridSpace> AffectedSpaces = new HashSet<GridSpace>();
    public static GridSpace SelectedSpace;

    protected bool readyToConfirm;
    protected bool EnoughEnergy = false;
    protected bool AbilityAvailable = false;

    public Ally Owner;

    public Ability()
    {
        readyToConfirm = false;
    }

    public Ability(Ally owner, int energyRequired = 0, bool specialAbility = true)
    {
        Owner = owner;
        readyToConfirm = false;
        EnergyRequired = energyRequired;
        EnoughEnergy = energyRequired <= owner.energy;
        SpecialAbility = specialAbility;
        AbilityAvailable = SpecialAbility ? !Owner.getAttackPerformed() : true;
    }

    public virtual void Preview() { }
    public virtual void SelectTarget(GridSpace selected) { }
    public virtual void Confirm()
    {
        Debug.Log("Habilidad Confirmada");
        if(SpecialAbility) Owner.setAttackPerformed(true);
        Owner.ReduceEnergy(EnergyRequired);
    }

    public bool EnougEnergy()
    {
        return EnoughEnergy;
    }

    public bool IsAbilityAvailable()
    {
        return AbilityAvailable;
    }

    public virtual void Cancel()
    {
        Debug.Log("Cancelling Hability");
        LogicManager.Instance.currentAbility = null;
        LogicManager.Instance.PJFinishedMoving();
    }

    public virtual void AddAffectedSpace(GridSpace spaceToAdd)
    {
        spaceToAdd.SetAffected(true);
        AffectedSpaces.Add(spaceToAdd);
    }

    public virtual void AddHealedSpace(GridSpace spacetoAdd)
    {
        spacetoAdd.SetHealed(true);
        AffectedSpaces.Add(spacetoAdd);
    }

    public virtual void AddSelectableSpace(GridSpace spaceToAdd)
    {
        spaceToAdd.SetSelectable(true);
        SelectableSpaces.Add(spaceToAdd);
    }

    public void ClearAffectedSpaces()
    {
        foreach (var space in AffectedSpaces)
        {
            space.SetAffected(false);
        }
        AffectedSpaces.Clear();
    }

    public void ClearSelectableSpaces()
    {
        foreach (var space in SelectableSpaces)
        {
            var d = space.neighbors["down"];
            //if (!d.IsEmpty()) d.GetEntity().OnClick.RemoveAllListeners();
            space.SetSelectable(false);
        }
        SelectableSpaces.Clear();
    }

    public static Ability GetAbility(Ally ally, int i)
    {
        if (ally is Knight knight)
        {
            switch (i)
            {
                case 0:
                    return new Knight_Ability_0(knight);
                case 1:
                    return new Knight_Ability_1(knight);
                case 2:
                    return new Knight_Ability_3(knight);
                case 3:
                    return new Knight_Ability_4(knight);
                    //case 4:
                    //    return new Knight_Ability_4(pj);
            }
        }
        else if (ally is Wizard wizard)
        {
            switch (i)
            {
                case 0:
                    return new Wizard_Ability_0(wizard);
                case 1:
                    return new Wizard_Ability_1(wizard);
                case 2:
                    return new Wizard_Ability_2(wizard);
                case 3:
                    return new Wizard_Ability_3(wizard);
                    //case 4:
                    //    return new Wizard_Ability_4(pj);
            }
        }
        else if (ally is Priest priest)
        {
            switch (i)
            {
                case 0:
                    return new Priest_Ability_0(priest);
                case 1:
                    return new Priest_Ability_1(priest);
                case 2:
                    return new Priest_Ability_2(priest);
                case 3:
                    return new Priest_Ability_4(priest);
                    //case 4:
                    //    return new Priest_Ability_4(pj);
            }
        }
        else if (ally is Rogue rogue)
        {
            switch (i)
            {
                case 0:
                    return new Rogue_Ability_0(rogue);
                case 1:
                    return new Rogue_Ability_1(rogue);
                case 2:
                    return new Rogue_Ability_2(rogue);
                case 3:
                    return new Rogue_Ability_3(rogue);
                    //case 4:
                    //    return new Rogue_Ability_4(pj);
            }
        }

        Debug.LogWarning("ERROR AL OBTENER HABILIDAD");
        return null;
    }

    public static Ability GetMovementAbility(Ally ally)
    {
        return new Movement_Ability(ally);
    }

    protected virtual bool CanOwnerDoSpecialAbility()
    {
        return !Owner.getAttackPerformed();
    }
    protected virtual bool IsSpecialAbility()
    {
        return true;
    }


    public bool IsReadyToConfirm()
    {
        return readyToConfirm;
    }

    public virtual void ClickedEntity(Entity clickedEntity)
    {

    }
}
