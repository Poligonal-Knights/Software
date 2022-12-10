using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer
{
    LineRenderer lr;

    static public void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject("AttackLine");
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = Resources.Load("MaterialBase") as Material;
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.startWidth = 0.1f;
        lr.numCapVertices = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        float centery = Vector3.Distance(start, end) * 0.3f;
        Vector3 center = new Vector3((start.x + end.x) / 2, start.y + centery, (start.z + end.z) / 2);

        List<Vector3> pointList = new List<Vector3>();

        for (float ratio = 0.0f; ratio <= 1.0f; ratio += 1f / pointList.Capacity)
        {
            Vector3 tangent1 = Vector3.Lerp(start, center, ratio);
            Vector3 tangent2 = Vector3.Lerp(center, end, ratio);
            Vector3 curve = Vector3.Lerp(tangent1, tangent2, ratio);

            pointList.Add(curve);
        }
        lr.positionCount = pointList.Count;
        lr.SetPositions(pointList.ToArray());
        Object.Destroy(myLine, 0.5f);
    }
}
