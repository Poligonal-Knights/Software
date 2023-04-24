using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float zoomSpeed = 2f;
    Vector3 touchStart;
    Vector3 center;
    Vector3 gridSize;
    Vector2 pan = Vector2.zero;
    Vector2 cameraBounds;
    Camera Camera;
    Vector3 DefaultCameraPosition;

    //Rotation
    public float duration;
    float speed;
    bool rotating = false;
    Vector3[] defaultPositions = new Vector3[4];
    int defaultPositionsIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        speed = 90.0f / duration;
        Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        CameraPanning();
        CameraZoom();
    }

    void CameraPanning()
    {

    }

    void CameraZoom()
    {
        //Camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        //Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, 1f, gridSize.magnitude * 0.45f);

        if (Input.GetMouseButtonDown(1))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Actualizar pan
            pan += (Vector2)direction;
            //Limitar el pan a los bounds
            pan = Vector2.Max(pan, -cameraBounds);
            pan = Vector2.Min(pan, cameraBounds);
            //Modificar la posicion default de la camara sumandole el pan
            Camera.main.transform.localPosition = DefaultCameraPosition + new Vector3(pan.x, pan.y, 0);

        }
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment * zoomSpeed, 1f, gridSize.magnitude * 0.45f);
    }

    public void Init()
    {
        FixCamera();
        DefaultCameraPosition = Camera.main.transform.position;
        gridSize = GridManager.Instance.GetGridSize();
        cameraBounds = (Vector2)gridSize / 2;
        cameraBounds *= 10;
        center = GridManager.Instance.GetCenterofGrid();
    }

    void FixCamera()
    {
        center = GridManager.Instance.GetCenterofGrid();
        gridSize = GridManager.Instance.GetGridSize();
        Camera.orthographicSize = gridSize.magnitude * 0.28f;
        var originalPosition = transform.position;
        transform.position = center + new Vector3(1f, 0.82f, 1f) * gridSize.magnitude * 0.45f;
        var bestPosition = transform.position;
        float bestDistance = float.MaxValue;
        for (int i = 0; i < defaultPositions.Length; i++)
        {
            defaultPositions[i] = transform.position;
            var distance = Vector3.Distance(defaultPositions[i], originalPosition);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestPosition = defaultPositions[i];
                defaultPositionsIndex = i;
            }
            transform.RotateAround(center, Vector3.up, 90);
        }
        transform.position = bestPosition;
        //DefaultCameraPosition = bestPosition;
        transform.LookAt(center);

    }

    void CalculateCameraBounds()
    {

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
        var centerOfRotation = center + (Vector3)pan;
        var timeSinceStart = 0.0f;
        var clockwise = dir ? -1 : 1;
        while (duration > timeSinceStart + Time.deltaTime)
        {
            timeSinceStart += Time.deltaTime;
            transform.RotateAround(centerOfRotation, Vector3.up, clockwise * speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        transform.RotateAround(centerOfRotation, Vector3.up, clockwise * speed * (duration - timeSinceStart));
        foreach (var pjs in GameManager.Instance.PJs)
        {
            pjs.UpdateOrientation();
        }
        defaultPositionsIndex += clockwise;
        if (defaultPositionsIndex < 0) defaultPositionsIndex = defaultPositions.Length - 1;
        else if (defaultPositionsIndex >= defaultPositions.Length) defaultPositionsIndex = 0;
        DefaultCameraPosition = defaultPositions[defaultPositionsIndex];
        rotating = false;
    }
}
