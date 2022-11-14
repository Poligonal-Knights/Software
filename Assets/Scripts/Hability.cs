using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Hability
{
    public static HashSet<GridSpace> SelectableSpaces = new HashSet<GridSpace>();
    public static HashSet<GridSpace> AffectedSpaces = new HashSet<GridSpace>();
    public static GridSpace SelectedSpace;

    public virtual void Preview() { }
    public virtual void SelectTarget(GridSpace selected) { }
    public virtual void Confirm() { }

    public virtual void AddAffectedSpace(GridSpace spaceToAdd)
    {
        spaceToAdd.SetAffected(true);
        AffectedSpaces.Add(spaceToAdd);
    }

    public virtual void AddSelectableSpace(GridSpace spaceToAdd)
    {
        spaceToAdd.SetSelectable(true);
        SelectableSpaces.Add(spaceToAdd);
    }

    public void ClearAffectedSpaces()
    {
        foreach (var space in AffectedSpaces)
        {
            space.SetAffected(false);
        }
        AffectedSpaces.Clear();
    }

    public void ClearSelectableSpaces()
    {
        foreach (var space in SelectableSpaces)
        {
            var d = space.neighbors["down"];
            if (!d.IsEmpty()) d.GetEntity().OnClick.RemoveAllListeners();
            space.SetSelectable(false);
        }
        SelectableSpaces.Clear();
    }

    public static Hability GetHability(PJ pj, int i)
    {
        if (pj is Knight)
        {
            switch (i)
            {
                case 0:
                    return new Knight_Hability_0(); break;
                case 1:
                    return new Knight_Hability_1(); break;
            }
        }
        Debug.Log("ERROR AL OBTENER HABILIDAD");
        return null;
    }
}
