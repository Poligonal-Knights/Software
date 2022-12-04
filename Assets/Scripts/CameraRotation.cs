using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float duration;
    float speed;
    Vector3 center;
    bool rotating = false;

    void Start()
    {
        center = GridManager.Instance.GetCenterofGrid();
        speed = 90.0f / duration;
    }

    public void RotateCamera(bool Clockwise)
    {
        if (!rotating)
        {
            //startTime = Time.time;
            rotating = true;
            StartCoroutine(RotateCam(Clockwise));
        }
    }

    IEnumerator RotateCam(bool dir)
    {
        var timeSinceStart = 0.0f;
        var clockwise = dir ? -1 : 1;
        while (duration > timeSinceStart + Time.deltaTime)
        {
            timeSinceStart += Time.deltaTime;
            transform.RotateAround(center, Vector3.up, clockwise * speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        transform.RotateAround(center, Vector3.up, clockwise * speed * (duration - timeSinceStart));
        foreach (var pjs in FindObjectsOfType<PJ>())
        {
            pjs.UpdateOrientation();
        }
        rotating = false;
    }
}

