using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager grid;

    // Start is called before the first frame update
    void Start()
    {
        grid.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
