using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float zoomSpeed = 2f;
    Vector3 touchStart;
    Vector3 center;
    Vector3 gridSize;
    Vector2 cameraBounds;
    Camera Camera;

    //Rotation
    public float durationOfRotation;
    float speed;
    bool rotating = false;
    Vector3 DefaultCameraPosition;
    Vector3[] defaultPositions = new Vector3[4];
    int defaultPositionsIndex = 0;

    void Start()
    {
        speed = 90.0f / durationOfRotation;
    }

    private void LateUpdate()
    {
        CameraZoomAndPanning();
    }

    public void Init()
    {
        DefaultCameraPosition = Camera.main.transform.position;
        gridSize = GridManager.Instance.GetGridSize();
        CalculateCameraBounds();
        center = GridManager.Instance.GetCenterofGrid();
        Camera = GetComponent<Camera>();
        FixCamera();
    }

    void CameraZoomAndPanning()
    {
        if (rotating) return;
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
            var localDirection = transform.InverseTransformVector(direction);
            var localDefaultCameraPosition = transform.InverseTransformPoint(DefaultCameraPosition);
            var newPan = localDirection - localDefaultCameraPosition;
            newPan = Vector3.Max(newPan, -cameraBounds);
            newPan = Vector3.Min(newPan, cameraBounds);
            newPan.z = 0;
            var newPos = localDefaultCameraPosition + newPan;
            Camera.main.transform.position = transform.TransformPoint(newPos);
        }
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment * zoomSpeed, 1f, gridSize.magnitude * 0.45f);
    }

    void FixCamera()
    {
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
        DefaultCameraPosition = bestPosition;
        transform.position = DefaultCameraPosition;
        transform.LookAt(center);
    }

    void CalculateCameraBounds()
    {
        var x = new Vector2(gridSize.x, gridSize.z).magnitude;
        var y = gridSize.y;
        cameraBounds = new Vector2(x, y) / 2f;
    }

    public void RotateCamera(bool Clockwise)
    {
        if (rotating) return;
        rotating = true;
        StartCoroutine(RotateCam(Clockwise));
    }

    IEnumerator RotateCam(bool dir)
    {
        var centerOfRotation = center;
        var timeSinceStart = 0.0f;
        var clockwise = dir ? -1 : 1;
        while (durationOfRotation > timeSinceStart + Time.deltaTime)
        {
            timeSinceStart += Time.deltaTime;
            transform.RotateAround(centerOfRotation, Vector3.up, clockwise * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.RotateAround(centerOfRotation, Vector3.up, clockwise * speed * (durationOfRotation - timeSinceStart));
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
