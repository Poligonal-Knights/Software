using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Oil : Entity
{
    static HashSet<GridSpace> spacesWithOil = new HashSet<GridSpace>();
    int duration = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        UpdateGridSpace();
        spacesWithOil.Add(space);
        duration = 1;
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    protected override void UpdateGridSpace()
    {
        Vector3Int pos = Vector3Int.RoundToInt(transform.position);
        space = GridManager.Instance.GetGridSpaceWorldCoords(pos);
    }

    public static bool HasGridSpaceOil(GridSpace spaceTocheck)
    {
        return spacesWithOil.Contains(spaceTocheck);
    }

    protected override void OnChangeTurn()
    {
        duration--;
        if(duration <= 0)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnMouseUpAsButton()
    {

    }

    protected override void OnDisable()
    {
        spacesWithOil.Remove(space);
        base.OnDisable();
    }
}
