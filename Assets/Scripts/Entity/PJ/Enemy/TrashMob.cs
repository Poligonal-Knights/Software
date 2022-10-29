using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrashMob : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 3;
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
    /*
     Metodo IA()
    {
        moverse random
        atacar si es posible, random
    }
     */
}
