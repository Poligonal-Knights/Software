using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public TurnManager turnManager;
    public LogicManager logicManager;
    public InputHandler inputHandler;
    public GUI UIManager;
    public EnemyManager enemyManager;

    Entity[] entities;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        gridManager = FindObjectOfType<GridManager>();
        turnManager = FindObjectOfType<TurnManager>();
        logicManager = FindObjectOfType<LogicManager>();
        inputHandler = FindObjectOfType<InputHandler>();
        enemyManager = FindObjectOfType<EnemyManager>();
        UIManager = FindObjectOfType<GUI>();
        gridManager.Init();
        turnManager.Init();

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
