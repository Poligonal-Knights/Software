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
