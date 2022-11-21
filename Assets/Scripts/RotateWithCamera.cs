using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        transform.rotation = Camera.main.transform.rotation;
        
        //Introducir limites con los sprites y crearlos en conjuntos de 4 que siempre sigan el mismo orden
    }
}
