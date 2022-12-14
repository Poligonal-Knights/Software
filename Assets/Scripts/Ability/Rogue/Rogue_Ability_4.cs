using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

//Muñeco explosivo, no tiene futuro
public class Rogue_Ability_4 : Ability
{
    public Rogue_Ability_4(Rogue owner, int energyRequired = 2) : base(owner, energyRequired) { }

    GameObject dummy;
    public override void Preview()
    {
        base.Preview();
        Debug.Log("Hability Preview");
        var PJSpace = Owner.GetGridSpace();
        foreach (var move in PJSpace.moves)
        {
            if (move.gridPosition.y == PJSpace.gridPosition.y && move.GetEntity() is null)
            {
                AddSelectableSpace(move);
            }
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        Debug.Log("Selecting Target");
        ClearAffectedSpaces();
        RefreshSelectableSpaces();

        SelectedSpace = selected;
        AddHealedSpace(SelectedSpace);

        //Spawnear dummy
        var rogue = Owner as Rogue;
        //GameObject dummy = Object.Instantiate(rogue.dummy);
        dummy.transform.position = SelectedSpace.GetWorldPosition();
        
    }

    public override void Confirm()
    {
        base.Confirm();
        Debug.Log("Confirming Hability");
        //Cambiar opacidad del sprite del dummy
        ClearAffectedSpaces();
        ClearSelectableSpaces();
        LogicManager.Instance.PJFinishedMoving();
    }

    public override void Cancel()
    {
        base.Cancel();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }

    void RefreshSelectableSpaces()
    {
        foreach (var s in SelectableSpaces)
        {
            s.SetSelectable(true);
        }
    }

    public override void ClickedEntity(Entity entityClicked)
    {
        GridSpace spaceToBeSelected = null;
        if (entityClicked is PJ)
        {
            spaceToBeSelected = entityClicked.GetGridSpace();
        }
        else if (entityClicked is Block)
        {
            spaceToBeSelected = entityClicked.GetGridSpace().neighbors["up"];
        }
        if (spaceToBeSelected is not null && spaceToBeSelected.IsSelectable())
            SelectTarget(spaceToBeSelected);
    }
}
