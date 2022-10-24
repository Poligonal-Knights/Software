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
        speed = 6;
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
        space.visited = true;
        Queue<BFS_Node> nodes = new Queue<BFS_Node>();
        foreach (var move in space.moves.Values)
        {
            if (!move.visited && CanMoveThere(space, move))
            {
                print(move.visited);
                move.SetVisited(true);
                print(move.visited);
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Instantiate(sphere, move.getWorldPosition(), Quaternion.identity);
                sphere.SetActive(false);
                nodes.Enqueue(new BFS_Node(move, null, 1));
            }
        }
        while (nodes.Any())
        {
            var actualNode = nodes.Dequeue();
            if (actualNode.distance < speed)
            {
                foreach (var move in actualNode.space.moves.Values)
                {
                    if (!move.visited && CanMoveThere(actualNode.space, move))
                    {
                        move.SetVisited(true);
                        //Aqui posiblemente iniciar animacion, mirar espacio de abajo de move, el bloque de ese espacio inicia animacion
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        Instantiate(sphere, move.getWorldPosition(), Quaternion.identity);
                        sphere.SetActive(false);
                        nodes.Enqueue(new BFS_Node(move, actualNode, actualNode.distance + 1));
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
