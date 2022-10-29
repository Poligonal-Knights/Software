using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Lista de enemigos
    List<Enemy> enemyList = new List<Enemy>();
    public GameManager gameManager;

    private int actualEnemyTurn =  0;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            enemyList.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void enemyTurn()
    {
        enemyList[actualEnemyTurn].EnemyAI();
        gameManager.UIManager.ShowEmptyCanvas();
    }

    public void enemyTurnEnd()
    {
        actualEnemyTurn++;
        if (actualEnemyTurn < enemyList.Count)
        {
            actualEnemyTurn = 0;
            gameManager.turnManager.ChangeTurn();
        }
        else
        {
            enemyTurn();
        }
    }
}
