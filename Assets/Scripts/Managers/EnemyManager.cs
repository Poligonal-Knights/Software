using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    //Lista de enemigos
    public List<Enemy> enemyList = new List<Enemy>();
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
        if(!enemyList.Any())
        {
            SceneManager.LoadScene("Victory");
        }
    }


    public void enemyTurn()
    {
        Debug.Log("Probando");
        enemyList[actualEnemyTurn].EnemyAI();
        Debug.Log("Probando terminado");
        gameManager.UIManager.ShowEmptyCanvas();
    }

    public void enemyTurnEnd()
    {
        gameManager.gridManager.visitedSpaces.Clear();
        actualEnemyTurn++;
        if (actualEnemyTurn >= enemyList.Count)
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
