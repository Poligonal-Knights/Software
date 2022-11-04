using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

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
