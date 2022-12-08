using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bless
public class Priest_Ability_2 : Ability
{
    public Priest_Ability_2(PJ owner) : base(owner) { EnergyConsumed = 2; }

    public override void Preview()
    {
        foreach(var ally in GameManager.Instance.allies)
        {
            AddSelectableSpace(ally.GetGridSpace());
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        ClearAffectedSpaces();
        foreach (var s in SelectableSpaces)
        {
            s.SetSelectable(true);
        }
        SelectedSpace = selected;
        AddHealedSpace(selected);
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        base.Confirm();

        foreach (var s in AffectedSpaces)
        {
            if(s.GetEntity() is Ally ally)
            {
                new Bless(ally);
            }
        }
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }

    public override void Cancel()
    {
        base.Cancel();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
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
