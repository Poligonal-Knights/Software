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
        GameManager.Instance.StartCoroutine(PaintRoute());
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
        GridSpace spaceToBeSelected = null;
        if (entityClicked is Block)
        {
            spaceToBeSelected = entityClicked.GetGridSpace().neighbors["up"];
        }
        else if (entityClicked is PJ or Trap)
        {
            spaceToBeSelected = entityClicked.GetGridSpace();
        }
        if (spaceToBeSelected != null && spaceToBeSelected.IsVisited() && (spaceToBeSelected.IsEmpty() || spaceToBeSelected.HasTrap()))
        {
            SelectTarget(spaceToBeSelected);
        }
    }

    public void BFS()
    {
        var PJSpace = pj.GetGridSpace();
        AddVisitedSpace(PJSpace);
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        foreach (var move in PJSpace.moves)
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
                foreach (var move in currentNode.space.moves)
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

    void PaintRouteC()
    {
        //RefreshVisitedSpaces();
        //var node = SelectedSpace.node;
        //PaintNode(node);
        //while (node.HasParent())
        //{
        //    node = node.parent;
        //    PaintNode(node);
        //}
    }

    IEnumerator PaintRoute()
    {
        var speed = 0.03f;
        RefreshVisitedSpaces();
        Stack<BFS_Node> nodes = new Stack<BFS_Node>();
        var node = SelectedSpace.node;
        do
        {
            nodes.Push(node);
            node = node.parent;
        } while (node != null);

        while (nodes.Any())
        {
            node = nodes.Pop();
            yield return new WaitForSeconds(node.distance * speed);
            node.space.SetSelectable(true);
        }
    }


    void AddVisitedSpace(GridSpace spaceToAdd)
    {
        spaceToAdd.SetVisited(true);
        visitedSpaces.Add(spaceToAdd);
    }

    void ClearVisitedSpaces()
    {
        foreach (var space in visitedSpaces)
        {
            space.SetVisited(false);
        }
        visitedSpaces.Clear();
    }

    void RefreshVisitedSpaces()
    {
        foreach (var space in visitedSpaces)
        {
            space.SetVisited(true);
        }
    }
}
