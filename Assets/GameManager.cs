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
        foreach (Entity e in entities) e.Init();
        int vacias = 0;
        int ocupadas = 0;
        foreach (var s in grid.spaces)
        {
            if (s.IsEmpty()) vacias++;
            else ocupadas++;
        }
        Debug.Log("Número de entidades: " + entities.Length);
        Debug.Log("Vacias: " + vacias);
        Debug.Log("Ocupadas: " + ocupadas);
        Debug.Log("Totales: " + (vacias + ocupadas));
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
