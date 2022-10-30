using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Ally
{   
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health=10;
        pushStrength = 4;
        trapBonusDamage = 4;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        return base.CanMoveThere(start, destination);
    }

    protected override void Hability0()
    {
        if(IsInitiatingHability)
        {
            IsInitiatingHability = false;
            foreach (var move in space.moves.Values)
            {
                if (move.gridPosition.y == space.gridPosition.y)
                {
                    move.SetSelectable(true);
                    var b = move.neighbors["down"].GetEntity() as Block;
                    b.SetInSelectedMode();
                }
            }

            gridManager.ClearAffectedSpaces();
        }
        if(IsSelectingDirection)
        {
            Debug.Log("PJ_SelectingDirection");
            IsSelectingDirection = false;
            var auxVector = spaceSelected.gridPosition - space.gridPosition;
            var aux = Vector3.Cross(auxVector, Vector3.up);
            var spaceAffected1 = gridManager.GetGridSpace(Vector3Int.RoundToInt(spaceSelected.gridPosition + aux));
            var spaceAffected2 = gridManager.GetGridSpace(Vector3Int.RoundToInt(spaceSelected.gridPosition - aux));
            spaceSelected.SetAffected(true);
            spaceAffected1.SetAffected(true);
            spaceAffected2.SetAffected(true);
            (spaceSelected.neighbors["down"].GetEntity() as Block).SetInAreaAttackMode();
            (spaceAffected1.neighbors["down"].GetEntity() as Block).SetInAreaAttackMode();
            (spaceAffected2.neighbors["down"].GetEntity() as Block).SetInAreaAttackMode();
        }
        if (IsConfirming)
        {
            IsConfirming = false;
            var pushDirection = spaceSelected.gridPosition - space.gridPosition;
            foreach(var affectedSpace in gridManager.affectedSpaces)
            {
                // Debug.Log(affectedSpace.gridPosition);
                var entity = affectedSpace.GetEntity();
                if (entity is Enemy)
                {
                    var enemy = entity as Enemy;
                    enemy.BePushed(pushDirection, pushStrength, trapBonusDamage);
                }
            }
        }
    }
}
