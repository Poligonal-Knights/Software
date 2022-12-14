using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Curacion
public class Priest_Ability_1 : Ability
{
    public Priest_Ability_1(Priest owner, int energyRequired = 2) : base(owner, energyRequired) { }

    int selectableRadius = 5;
    int areaRange = 5;

    int healing = 5;

    public override void Preview()
    {
        base.Preview();
        Debug.Log("Priest H1 preview");
        var ownerSpacePosition = Owner.GetGridSpace().gridPosition;
        var limits = GridManager.Instance.GetGridSize();

        for (int x = ownerSpacePosition.x - selectableRadius; x <= ownerSpacePosition.x + selectableRadius; x++)
        {
            for (int z = ownerSpacePosition.z - selectableRadius + Mathf.Abs(x - ownerSpacePosition.x); z <= ownerSpacePosition.z + selectableRadius - Mathf.Abs(x - ownerSpacePosition.x); z++)
            {
                for (int y = 0; y < limits.y; y++)
                {
                    var candidateSpace = GridManager.Instance.GetGridSpace(x, y, z);
                    if (candidateSpace != null && candidateSpace.IsPassable())
                    {
                        AddSelectableSpace(candidateSpace);
                    }
                }
            }
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        ClearAffectedSpaces();
        foreach (var s in SelectableSpaces)
            s.SetSelectable(true);

        readyToConfirm = true;
        SelectedSpace = selected;
        var selectedSpacePosition = SelectedSpace.gridPosition;
        var limits = GridManager.Instance.GetGridSize();

        for (int x = selectedSpacePosition.x - areaRange; x <= selectedSpacePosition.x + areaRange; x++)
        {
            for (int z = selectedSpacePosition.z - areaRange + Mathf.Abs(x - selectedSpacePosition.x); z <= selectedSpacePosition.z + areaRange - Mathf.Abs(x - selectedSpacePosition.x); z++)
            {
                for (int y = 0; y < limits.y; y++)
                {
                    var candidateSpace = GridManager.Instance.GetGridSpace(x, y, z);
                    if (candidateSpace != null && candidateSpace.IsPassable())
                    {
                        AddHealedSpace(candidateSpace);
                    }
                }
            }
        }
    }
    public override void Confirm()
    {
        base.Confirm();

        foreach (var affectedSpace in AffectedSpaces)
        {
            if (affectedSpace.GetEntity() is Ally ally)
            {
                ally.Heal(healing);
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
