using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GUI UIManager;
    public GameManager GameManager;

    bool playerTurn;

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
        UIManager.ChangeTurn(playerTurn);
        if (!playerTurn)
        {
          GameManager.enemyManager.enemyTurn();
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
