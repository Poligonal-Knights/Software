using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Lista de enemigos
    List<Enemy> enemyList = new List<Enemy>();

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
}
