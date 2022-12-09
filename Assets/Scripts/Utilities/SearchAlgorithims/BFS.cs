using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BFS
{
    public static List<GridSpace> GetSpacesInRange(GridSpace start, int range, Func<GridSpace, GridSpace, bool> CanMoveThere)
    {
        List<GridSpace> visitedSpaces = new List<GridSpace>();
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        visitedSpaces.Add(start);
        foreach (GridSpace move in start.moves)
        {
            if (range > 0 && !visitedSpaces.Contains(move) && CanMoveThere(start, move))
            {
                visitedSpaces.Add(move);
                nodes.Enqueue(new BFS_Node(move, null, 1));
            }
        }
        while (nodes.Any())
        {
            BFS_Node currentNode = nodes.Dequeue();
            if (currentNode.distance < range)
            {
                foreach (GridSpace move in currentNode.space.moves)
                {
                    if (!visitedSpaces.Contains(move) && CanMoveThere(currentNode.space, move))
                    {
                        if (!(currentNode.distance + 1 == range && move.GetEntity() is PJ))
                        {
                            visitedSpaces.Add(move);
                            nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                        }
                    }
                }
            }
        }
        return visitedSpaces;
    }

    public static GridSpace GetGoalGridSpace(GridSpace start, int range, Func<GridSpace, GridSpace, bool> CanMoveThere, Predicate<GridSpace> goalCheck)
    {
        List<GridSpace> visitedSpaces = new List<GridSpace>();
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        visitedSpaces.Add(start);
        foreach (GridSpace move in start.moves)
        {
            if (range > 0 && !visitedSpaces.Contains(move) && CanMoveThere(start, move))
            {
                if (goalCheck(move))
                {
                    return move;
                }
                visitedSpaces.Add(move);
                nodes.Enqueue(new BFS_Node(move, null, 1));
            }
        }
        while (nodes.Any())
        {
            BFS_Node currentNode = nodes.Dequeue();
            if (goalCheck(currentNode.space))
            {
                return currentNode.space;
            }
            if (currentNode.distance < range)
            {
                foreach (GridSpace move in currentNode.space.moves)
                {
                    if (!visitedSpaces.Contains(move) && CanMoveThere(currentNode.space, move))
                    {
                        if (!(currentNode.distance + 1 == range && move.GetEntity() is PJ))
                        {
                            visitedSpaces.Add(move);
                            nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                        }
                    }
                }
            }
        }
        return null;
    }
}
