using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Hability
{
    public static HashSet<GridSpace> SelectableSpaces = new HashSet<GridSpace>();
    public static HashSet<GridSpace> AffectedSpaces = new HashSet<GridSpace>();

    public virtual void Preview() { }
    public virtual void SelectTarget(GridSpace selected) { }
    public virtual void Confirm() { }

    public static Hability GetHability(PJ pj, int i)
    {
        if(pj is Knight)
        {
            switch (i)
            {
                case 0:
                    return new Knight_Hability_1();
            }
        }
        Debug.Log("ERROR AL OBTENER HABILIDAD");
        return null;
    }
}
