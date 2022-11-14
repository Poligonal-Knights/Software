using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public UnityEvent ChangeTurnEvent = new UnityEvent();

    bool playerTurn;

    private void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        playerTurn = false;
        ChangeTurn();
    }

    public void Init()
    {

    }

    public void ChangeTurn()
    {
        Debug.Log("Pasa turno");
        playerTurn = !playerTurn;
        UIManager.Instance.ChangeTurn(playerTurn);
        if (!playerTurn)
        {
            EnemyManager.Instance.enemyTurn();
        }
        ChangeTurnEvent.Invoke();
    }

    public bool IsPlayerTurn()
    {
        return playerTurn;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
