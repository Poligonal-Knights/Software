using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basico
public class Priest_Ability_0 : Ability
{
    public Priest_Ability_0(PJ owner) : base(owner) { }

    int radius = 5;
    int range = 5;

    public override void Preview()
    {
        var ownerSpacePosition = Owner.GetGridSpace().gridPosition;
        var limits = GridManager.Instance.GetGridSize();

        for (int x = ownerSpacePosition.x - radius; x <= ownerSpacePosition.x + radius; x++)
        {
            for (int z = ownerSpacePosition.z - radius + Mathf.Abs(x - ownerSpacePosition.x); z <= ownerSpacePosition.z + radius - Mathf.Abs(x - ownerSpacePosition.x); z++)
            {
                for (int y = 0; y < limits.y; y++)
                {
                    var candidateSpace = GridManager.Instance.GetGridSpace(x, y, z);
                    if (candidateSpace != null && candidateSpace.IsPassable() && candidateSpace.GetEntity() is Enemy)
                    {
                        AddSelectableSpace(candidateSpace);
                    }
                }
            }
        }
    }

}
