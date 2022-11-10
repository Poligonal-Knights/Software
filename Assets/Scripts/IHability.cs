using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHability
{
    public void Preview();
    public void SelectTarget(GridSpace grid);
    public void Confirm();

}
