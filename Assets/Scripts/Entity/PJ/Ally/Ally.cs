using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : PJ
{
    //Stats
    int energy;
    int trapBonusDamage;
    int pushStrength;

    //States
    public bool IsDoingHability;
    public bool IsInitiatingHability;
    public bool IsSelectingDirection;
    public bool IsConfirming;
    public bool IsDoingHabilityAnimation;

    protected int HabilitySelected;
    protected GridSpace spaceSelected;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        IsDoingHability = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (IsDoingHability)
        {
            switch (HabilitySelected)
            {
                case 0:
                    Hability0();
                    break;
                case 1:
                    Hability1();
                    break;
                case 2:
                    Hability2();
                    break;
                case 3:
                    Hability3();
                    break;
                case 4:
                    Hability4();
                    break;
            }
        }
        
    }

    public virtual void DoHability(int HabilityToDo)
    {
        HabilitySelected = HabilityToDo;
        IsDoingHability = true;
        IsInitiatingHability = true;
    }

    public virtual void StopDoingHability()
    {
        IsDoingHability = false;
    }

    public void SetHabilitySpaceSelected(GridSpace selected)
    {
        if (selected != null)
        {
            IsInitiatingHability = true;
        }
        spaceSelected = selected;
        IsSelectingDirection = true;
    }

    public void ConfirmHability()
    {
        IsConfirming = true;
    }

    protected virtual void Hability0() { Debug.Log(this + "performed Hab. 0"); }
    protected virtual void Hability1() { Debug.Log(this + "performed Hab. 1"); }
    protected virtual void Hability2() { Debug.Log(this + "performed Hab. 2"); }
    protected virtual void Hability3() { Debug.Log(this + "performed Hab. 3"); }
    protected virtual void Hability4() { Debug.Log(this + "performed Hab. 4"); }
}
