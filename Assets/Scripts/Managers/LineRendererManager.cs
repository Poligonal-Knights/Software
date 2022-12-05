using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererManager : MonoBehaviour
{
    //List<LineRenderer> lines = new List<LineRenderer>();
    // Start is called before the first frame update
    public static LineRendererManager Instance;

	private void Awake()
	{
        Instance = this;
	}

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddLine(Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetColors(Color.red, Color.red);
        Debug.Log("Color hecho");
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        var center = Vector3.Lerp(start, end, 0.5f);
        center.y += Vector3.Distance(start, end)*0.3f;
        List<Vector3> pointList = new List<Vector3>();
        Debug.Log("Bucle incoming");
        for (float ratio = 0; ratio <= 1; ratio += 1 / 12)
        {
            var tangent1 = Vector3.Lerp(start, center, ratio);
            var tangent2 = Vector3.Lerp(center, end, ratio);
            var curve = Vector3.Lerp(tangent1, tangent2, ratio);

            pointList.Add(curve);
        }
        lr.positionCount = pointList.Count;
        lr.SetPositions(pointList.ToArray());
        Debug.Log("LineaCreada");
        GameObject.Destroy(myLine, 5f);
    }
}
