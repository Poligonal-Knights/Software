using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PJ : Entity
{
    int speed;
    protected override void Start()
    {
        base.Start();
    }

    public override void Init()
    {
        base.Init();
        speed = 5;
    }

    // Update is called once per frame
    protected override void Update()
    {

    }

    public void FindPath(Vector3Int goal)
    {
        BFS();
    }

    public void BFS()
    {
        space.SetVisited(true);
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        foreach (var move in space.moves.Values)
        {
            if (!move.visited && CanMoveThere(space, move))
            {
                move.SetVisited(true);
                nodes.Enqueue(new BFS_Node(move, null, 1));

                //Animation
                Block b = move.neighbors["down"].GetEntity() as Block;
                b.SetInPreviewMode();
            }
        }
        while (nodes.Any())
        {
            var currentNode = nodes.Dequeue();
            if (currentNode.distance < speed)
            {
                foreach (var move in currentNode.space.moves.Values)
                {
                    if (!move.visited && CanMoveThere(currentNode.space, move))
                    {
                        move.SetVisited(true);
                        nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));

                        //Animation
                        Block b = move.neighbors["down"].GetEntity() as Block;
                        b.SetInPreviewMode();
                    }
                }
            }
        }
    }

    protected virtual bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        if (start.gridPosition.y == destination.gridPosition.y) return true;
        return false;
    }

    protected override void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
    }

    protected virtual void Movement(GridSpace start, GridSpace destination)
    {
        start.GetWorldPosition();
    }
    
}
