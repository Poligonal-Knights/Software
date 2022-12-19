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
        //ChangeTurn(); Ahora se llama en NuevoDialogo
    }

    public void Init()
    {

    }

    public void ChangeTurn()
    {
        playerTurn = !playerTurn;
        Debug.Log("Cambio de turno. Turno del jugador: " + playerTurn);
        UIManager.Instance.ChangeTurn(playerTurn);
        if (!playerTurn)
        {
            EnemyManager.Instance.enemyTurn();
        }
        GridManager.Instance.clearNodes();
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
