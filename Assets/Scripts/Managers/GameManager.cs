using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    Entity[] entities;

    private void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        GridManager.Instance.Init();
        TurnManager.Instance.Init();

        //Cosas de debug, se eliminaran mas adelante
        //foreach (var p in FindObjectsOfType<PJ>())
        //{
        //    p.FindPath(Vector3Int.zero);
        //}
        //var aux = FindObjectOfType<Goal>().GetGridSpace();
        //if (aux.IsVisited())
        //{
        //    var actualNode = aux.node;
        //    while (actualNode.HasParent())
        //    {
        //        actualNode = actualNode.parent;
        //    };
        //}
        //else Debug.Log("Meta no encontrada");
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
