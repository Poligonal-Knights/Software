using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public int EnergyConsumed;

    public static HashSet<GridSpace> SelectableSpaces = new HashSet<GridSpace>();
    public static HashSet<GridSpace> AffectedSpaces = new HashSet<GridSpace>();
    public static GridSpace SelectedSpace;

    protected bool readyToConfirm;

    public PJ Owner;

    public Ability()
    {
        readyToConfirm = false;
    }

    public Ability(PJ owner)
    {
        Owner = owner;
        readyToConfirm = false;
    }

    public virtual void Preview() { }
    public virtual void SelectTarget(GridSpace selected) { }
    public virtual void Confirm()
    {
        Debug.Log("Habilidad Confirmada");
        (Owner as Ally).ReduceEnergy(EnergyConsumed);
    }

    public virtual int GetEnergyConsumed() { return EnergyConsumed; }
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

    public static Ability GetAbility(PJ pj, int i)
    {
        if (pj is Knight)
        {
            switch (i)
            {
                case 0:
                    return new Knight_Ability_0(pj);
                case 1:
                    return new Knight_Ability_1(pj);
                case 2:
                    return new Knight_Ability_2(pj);
                case 3:
                    return new Knight_Ability_3(pj);
                case 4:
                    return new Knight_Ability_4(pj);
            }
        }
        else if (pj is Wizard)
        {
            switch (i)
            {
                case 0:
                    return new Wizard_Ability_0(pj);
                case 1:
                    return new Wizard_Ability_1(pj);
                case 2:
                    return new Wizard_Ability_2(pj);
                case 3:
                    return new Wizard_Ability_3(pj);
                case 4:
                    return new Wizard_Ability_4(pj);
            }
        }
        else if (pj is Priest)
        {
            switch (i)
            {
                case 0:
                    return new Priest_Ability_0(pj);
                case 1:
                    return new Priest_Ability_1(pj);
                case 2:
                    return new Priest_Ability_2(pj);
                case 3:
                    return new Priest_Ability_3(pj);
                case 4:
                    return new Priest_Ability_4(pj);
            }
        }


        Debug.Log("ERROR AL OBTENER HABILIDAD");
        return null;
    }

    public static Ability GetMovementAbility(PJ pj)
    {
        return new Movement_Ability(pj);
    }

    public bool IsReadyToConfirm()
    {
        return readyToConfirm;
    }

    public virtual void ClickedEntity(Entity clickedEntity)
    {

    }
}
