using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager grid;
    Entity[] entities;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        grid.Init();
        foreach(var p in FindObjectsOfType<PJ>())
        {
            p.FindPath(Vector3Int.zero);
        }
        var aux = FindObjectOfType<Goal>().GetGridSpace();
        if (aux.IsVisited())
        {
            var actualNode = aux.node;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            while(actualNode.HasParent())
            {
                Instantiate(sphere, actualNode.space.GetWorldPosition(), Quaternion.identity);
                actualNode = actualNode.parent;
            };
            Instantiate(sphere, actualNode.space.GetWorldPosition(), Quaternion.identity);
            sphere.SetActive(false);
        }
        else Debug.Log("Meta no encontrada");
    }

    void Init()
    {
        entities = FindObjectsOfType<Entity>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
