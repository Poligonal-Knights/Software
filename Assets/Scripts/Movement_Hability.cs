using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement_Hability : Hability
{
    HashSet<GridSpace> visitedSpaces = new HashSet<GridSpace>();

    public override void Preview()
    {
        pj = LogicManager.Instance.GetSelectedPJ();
        BFS();
    }

    public override void SelectTarget(GridSpace selected)
    {
        SelectedSpace = selected;
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        pj.MoveTo(SelectedSpace);
        ClearVisitedSpaces();
    }

    public override void Cancel()
    {
        base.Cancel();
        ClearVisitedSpaces();
        LogicManager.Instance.PJFinishedMoving();
    }

    public override void ClickedEntity(Entity entityClicked)
    {
        if(entityClicked is Block)
        {
            var selectedSpace = entityClicked.GetGridSpace().neighbors["up"];
            if(selectedSpace.IsVisited() && selectedSpace.IsEmpty())
            {
                SelectTarget(selectedSpace);
            }
        }
    }

    public void BFS()
    {
        var PJSpace = pj.GetGridSpace();
        AddVisitedSpace(PJSpace);
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        foreach (var move in PJSpace.moves.Values)
        {
            if (!move.visited && pj.CanMoveThere(PJSpace, move))
            {
                AddVisitedSpace(move);
                nodes.Enqueue(new BFS_Node(move, null, 1));
            }
        }
        while (nodes.Any())
        {
            var currentNode = nodes.Dequeue();
            if (currentNode.distance < pj.maxMovement)
            {
                foreach (var move in currentNode.space.moves.Values)
                {
                    if (!move.visited && pj.CanMoveThere(currentNode.space, move))
                    {
                        if (!(currentNode.distance + 1 == pj.maxMovement && move.GetEntity() is PJ))
                        {
                            AddVisitedSpace(move);
                            nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                        }
                    }
                }
            }
        }
    }

    void AddVisitedSpace(GridSpace spaceToAdd)
    {
        spaceToAdd.SetVisited(true);
        visitedSpaces.Add(spaceToAdd);
    }

    void ClearVisitedSpaces()
    {
        foreach(var space in visitedSpaces)
        {
            space.SetVisited(false);
        }
        visitedSpaces.Clear();
    }
}
