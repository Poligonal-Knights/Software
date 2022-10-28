using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PJ : Entity
{
    int speed;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    public override void Init()
    {
        UpdateGridSpace();
        speed = 25;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FindPath(Vector3Int goal)
    {
        BFS();
    }

    void BFS()
    {
        space.SetVisited(true);
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        foreach (var move in space.moves.Values)
        {
            if (!move.visited && CanMoveThere(space, move))
            {
                move.SetVisited(true);
                nodes.Enqueue(new BFS_Node(move, null, 1));
                //Mostrar que el siguiente espacio también se puede mover
                move.neighbors["down"].GetEntity().GetComponent<Block>().getCurrentAnimator().SetInteger("estadoAnimacion", 1);
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
                        //Aqui posiblemente iniciar animacion, mirar espacio de abajo de move, el bloque de ese espacio inicia animacion
                        move.neighbors["down"].GetEntity().GetComponent<Block>().getCurrentAnimator().SetInteger("estadoAnimacion", 1);
                        nodes.Enqueue(new BFS_Node(move, currentNode, currentNode.distance + 1));
                    }
                }
            }
        }
    }

    protected virtual bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        return true;
    }
}
