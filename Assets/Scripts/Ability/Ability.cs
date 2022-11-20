using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using static UnityEngine.UI.GridLayoutGroup;

public class Ability
{
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
    public virtual void Confirm() { }
    public virtual void Cancel()
    {
        Debug.Log("Cancelling Hability");
        LogicManager.Instance.currentHability = null;
        LogicManager.Instance.PJFinishedMoving();
    }

    public virtual void AddAffectedSpace(GridSpace spaceToAdd)
    {
        spaceToAdd.SetAffected(true);
        AffectedSpaces.Add(spaceToAdd);
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

    public static Ability GetHability(PJ pj, int i)
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
        if (pj is Wizard)
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


        Debug.Log("ERROR AL OBTENER HABILIDAD");
        return null;
    }

    public static Ability GetMovementHability(PJ pj)
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
