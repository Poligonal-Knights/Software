using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GridManager gridManager;
    public int speed;
    
    // Start is called before the first frame update
    Vector3 center;
    bool rotateL = false;
    bool rotateR = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (rotateL)
        {
            transform.RotateAround(center, new Vector3(0, 1, 0), -speed * Time.deltaTime);
            
            //no funciona buscar como parar la rotacion
            if (transform.rotation.y == 45 || transform.rotation.y == 135 || transform.rotation.y == -45 || transform.rotation.y == -135) rotateL = false;
        }
        if (rotateR)
        {
            transform.RotateAround(center, new Vector3(0, 1, 0), speed * Time.deltaTime);
            //no funciona buscar como parar la rotacion
            if (transform.rotation.y == 45 || transform.rotation.y == 135 || transform.rotation.y == -45 || transform.rotation.y == -135) rotateR = false;
        }

    }

    public void RotRight() {
        center = gridManager.GetCenterofGrid();
        //Debug.LogWarning(center);

        rotateR = true;

        
            
        
    }

    public void RotLeft()
    {
        center = gridManager.GetCenterofGrid();
        //Debug.LogWarning(center);

        rotateL = true;


    }
}

