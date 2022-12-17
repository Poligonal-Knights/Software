using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

//Cerbatana
public class Rogue_Ability_2 : Ability
{
    public Rogue_Ability_2(Rogue owner, int energyRequired = 2) : base(owner, energyRequired) { }

    int attackRange = 4;

    public override void Preview()
    {
        base.Preview();
        SelectedSpace = null;
        AffectedSpaces = GridManager.SpacesAtManhattanRange(Owner.GetGridSpace(), attackRange);
        foreach (var s in AffectedSpaces)
        {
            if (s.GetEntity() is Enemy enemy)
            {
                AddSelectableSpace(s);
            }
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        RefreshSelectableSpaces();
        if (SelectedSpace != null)
        {
            SelectedSpace.SetSelectable(true);
        }
        SelectedSpace = selected;
        SelectedSpace.SetAffected(true);
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        var enemyAffected = SelectedSpace.GetEntity() as Enemy;
        new Poison(enemyAffected);
        AudioManager.Instance.PlayAttackSound();
        LogicManager.Instance.PJFinishedMoving();
        base.Confirm();
    }

    public override void Cancel()
    {
        base.Cancel();
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
