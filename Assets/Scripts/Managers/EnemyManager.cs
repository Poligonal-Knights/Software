using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    //Lista de enemigos
    public List<Enemy> enemyList = new List<Enemy>();
    public GameManager gameManager;

    private int actualEnemyTurn =  0;
    // Start is called before the first frame update

    private void Awake() => Instance = this;

    void Start()
    {
        foreach(var enemy in GameManager.Instance.enemies)
        {
            enemyList.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!enemyList.Any())
        {
            //GameManager.Instance.LoadNextScene();
            //Debug.Log("Esto se esta activando");
            //SceneManager.LoadScene("Victory");
        }
    }


    public void enemyTurn()
    {
        enemyList[actualEnemyTurn].EnemyAI();
        UIManager.Instance.ShowEmptyCanvas();
    }

    public void enemyTurnEnd()
    {
        GridManager.Instance.visitedSpaces.Clear();
        actualEnemyTurn++;
        if (actualEnemyTurn >= enemyList.Count)
        {
            actualEnemyTurn = 0;
            TurnManager.Instance.ChangeTurn();
        }
        else
        {
            //enemyTurn();
            StartCoroutine(NextEnemy());
        }
    }

    IEnumerator NextEnemy()
    {
        var currentEnemy = enemyList[actualEnemyTurn-1];
        while (currentEnemy.MovementsToDo.Any() && currentEnemy.IsMoving)
        {
            yield return new WaitForEndOfFrame();
        }
        enemyTurn();
    }
}
