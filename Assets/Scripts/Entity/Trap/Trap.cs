using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esta clase será muy distinta para la beta
public class Trap : Entity
{
    public bool activated;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        activated = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        //if (!activated)
        //{
        //    if(LookIsSomeoneIsSameSpace())
        //    {

        //    }
        //}
    }

    protected virtual bool LookIsSomeoneIsSameSpace()
    {
        //if (Vector3.Distance(transform.position, destination.GetWorldPosition()) < 0.001f)
        //{
        //    IsMoving = false;
        //    if (!MovementsToDo.Any())
        //    {
        //        gameManager.logicManager.PJFinishedMoving();
        //        UpdateGridSpace();
        //    }
        //}
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var collided = other.gameObject.GetComponent<Entity>();
        if(collided is PJ)
        {
            var PJcollided = collided as PJ;
            PJcollided.DealDamage(20);
        }
    }
}
