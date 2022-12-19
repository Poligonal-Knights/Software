using System;
using UnityEngine;

//Shield throw
public class Knight_Ability_4 : Ability
{
    public Knight_Ability_4(Knight owner, int energyRequired = 2) : base(owner, energyRequired) { }

    public GameObject shield;

    int attackRange = 4;
    GridSpace affSpace = null;

    public override void Preview()
    {
        base.Preview();
        var PJSpace = Owner.GetGridSpace();
        var spacesInRange = GridManager.SpacesAtManhattanRange(PJSpace, attackRange);
        foreach(var s in spacesInRange) AddSelectableSpace(s);
    }

    public override void SelectTarget(GridSpace selected)
    {
        if (selected.GetEntity() is not Enemy) return;
        if(affSpace != null)
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
        if(affSpace.GetEntity() is Enemy enemy)
        {
            LineDrawer.DrawLine(Owner.GetGridSpace().GetWorldPosition(), affSpace.GetWorldPosition());
            var direction = enemy.GetGridSpace().gridPosition - Owner.GetGridSpace().gridPosition;
            direction.Set(Math.Sign(direction.x), 0, Math.Sign(direction.z));
            enemy.BePushed(direction, Owner.pushStrength, Owner.damage, Owner);
        }
        AudioManager.Instance.PlayAttackSound();
        LogicManager.Instance.PJFinishedMoving();
        affSpace?.SetAffected(false);
        affSpace?.SetSelectable(false);
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
