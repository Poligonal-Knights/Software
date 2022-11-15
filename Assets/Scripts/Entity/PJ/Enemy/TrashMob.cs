using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Panda;

public class TrashMob : Enemy
{
    PJ focusedEnemy;
    List<PJ> enemiesInRangeList = new List<PJ>();
    public int myDamage ;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 5;
        damage = myDamage;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();
    }

    
    [Task]
    void IsEnemyFocused() 
    {
        if (focusedEnemy!=null) ThisTask.Succeed();
        else ThisTask.Fail();
    }
    [Task]
    void IsEnemyAlive()
    {
        if (focusedEnemy.health > 0) ThisTask.Succeed();
        else ThisTask.Fail();
    }
    [Task]
    void EnemiesInRange()
    {
        //Implementar comprobación de enemigos a rango

    }
    [Task]
    void ChooseCloserEnemy()
    {
        //Implementar lógica de elección de enemigo
        ThisTask.Succeed();
    }
    [Task]
    void ChooseInjured()
    {
        var healthMostInjured = Mathf.Infinity; 
        foreach (PJ enemy in enemiesInRangeList)
        {
            if (enemy.health < healthMostInjured)
            {
                focusedEnemy = enemy;
                healthMostInjured = enemy.health;
            }
        }
    }
    [Task]
    void BattleCryActive()
    {
        bool worked = false;
        foreach (PJ enemy in enemiesInRangeList)
        {
            if (enemy is Knight)
            {
                if ((enemy as Knight).UsingGritoDeBatalla())
                {
                    focusedEnemy = enemy;
                    ThisTask.Succeed();
                    worked = true;
                }
            }
        }
        if (worked == false) ThisTask.Fail();
    }

    [Task]
    void CanIAttack()
    {
        if (focusedEnemy && !getAttackPerformed())
        {
            ThisTask.Succeed();
        }
        else ThisTask.Fail();
    }

    [Task]
    void InAttackRange()
    {
        //Si la distancia entre focusedEnemy y Yo es =<1 -> ThisTask.Succeed()
        if (true)
        {
            ThisTask.Succeed();
        }
        else ThisTask.Fail();
    }
    [Task]
    void Attack()
    {
        //Correr animación de ataque mirando al enemigo
        focusedEnemy.health -= damage;
        ThisTask.Succeed();
    }

    [Task]
    void CanIMove()
    {
        if (movement > 0) ThisTask.Succeed();
        else ThisTask.Fail();
    }
    [Task]
    void GetCloser()
    {
        //Implementar camino con el máximo movimiento posible hasta el enemigo
        ThisTask.Succeed();
    }

    [Task]
    void EndTurn()
    {
        EnemyManager.Instance.enemyTurnEnd();
        ThisTask.Succeed();
    }

    [Task]
    void IsMyTurn()
    {
        if (realizandoTurno)
            ThisTask.Succeed();
    }
}
