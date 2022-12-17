
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

//TrapRope
public class Rogue_Ability_1 : Ability
{
    public Rogue_Ability_1(Rogue owner, int energyRequired = 2) : base(owner, energyRequired) { }

    GridSpace affSpace = null;

    public override void Preview()
    {
        base.Preview();
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
        if (affSpace != null)
        {
            affSpace.SetAffected(false);
            affSpace.SetSelectable(true);
        }
        affSpace = selected;
        affSpace.SetAffected(true);
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        var rogue = Owner as Rogue;
        var c = Object.Instantiate(rogue.ropeTrap, affSpace.GetWorldPosition(), Quaternion.identity);
        c.GetComponent<RopeTrap>().Init();
        LogicManager.Instance.PJFinishedMoving();
        base.Confirm();
    }

    public override void Cancel()
    {
        base.Cancel();
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
