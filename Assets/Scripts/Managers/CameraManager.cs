using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GridManager gridManager;
    public int speed;
    
    // Start is called before the first frame update
    Vector3 center;
    bool rotating = false;
    
    void Start()
    {
        
    }

    // Las rotaciones no son 100% precisas cuantoa mas speed mas error
    void Update()
    {
        
        

    }

    private void FixedUpdate()
    {
        
    }

    public void RotRight() {
        center = gridManager.GetCenterofGrid();
        if (!rotating) {
            rotating = true;
            StartCoroutine(RotateCam(true));
        }
         
    }

    public void RotLeft()
    {
        center = gridManager.GetCenterofGrid();
        if (!rotating)
        {
            rotating = true;
            StartCoroutine(RotateCam(false));
        }


    }

    public IEnumerator RotateCam(bool dir) {

        float rotation = 0;
        if (dir == true)
        {
            
            while (rotation <= 90f)
            {
                float rotPerFrame = speed * Time.deltaTime;
                transform.RotateAround(center, new Vector3(0, 1, 0), -rotPerFrame);
                rotation += rotPerFrame;
                yield return new WaitForEndOfFrame();
            }
        }
        else {
            while (rotation <= 90f)
            {
                float rotPerFrame = speed * Time.deltaTime;
                transform.RotateAround(center, new Vector3(0, 1, 0), rotPerFrame);
                rotation += rotPerFrame;
                yield return new WaitForEndOfFrame();
            }
        }
        rotating = false;
        

    }
}

