using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff// : MonoBehaviour
{
    const int deafaultDuration = 1;

    protected PJ Owner;
    protected int Duration;

    public Buff(PJ owner, int turnsDuration = deafaultDuration)
    {
        Owner = owner;
        Duration = turnsDuration;
        if (CanOwnerHaveThisBuff())
        {
            TurnManager.Instance.ChangeTurnEvent.AddListener(OnChangeTurn);
            Owner.AddBuff(this);
            Init();
        }
    }

    protected virtual void OnChangeTurn()
    {
        Duration--;
        if (Duration <= 0)
        {
            Finish();
        }
    }

    protected virtual void Init()
    {

    }

    protected virtual void Finish()
    {
        Debug.Log("Removing buff");
        Owner.RemoveBuff(this);
        TurnManager.Instance.ChangeTurnEvent.RemoveListener(OnChangeTurn);
    }

    protected virtual bool CanOwnerHaveThisBuff()
    {
        return false;
    }
}
