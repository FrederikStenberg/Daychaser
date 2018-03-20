using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelzierScript : MonoBehaviour {


    public List<Vector3> pointsForPlayer = new List<Vector3>();

    public List<GameObject> waypoints = new List<GameObject>();
    public Color color = Color.red;
    public float width = 0.2f;
    public int numberOfPoints = 20;
    LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

        if (null == lineRenderer || waypoints == null || waypoints.Count < 0)
        {
            return; //Not enough waypoints
        }

        //update line renderer
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        if (numberOfPoints < 2)
        {
            numberOfPoints = 2;
        }
        lineRenderer.positionCount = numberOfPoints * (waypoints.Count - 2);

        Vector3 p0, p1, p2;
        for (int j = 0; j < waypoints.Count - 2; j++)
        {
            //check waypoints
            if (waypoints[j] == null || waypoints[j + 1] == null || waypoints[j + 2] == null)
            {
                return;
            }

            //determine waypoints of segment
            p0 = 0.5f * (waypoints[j].transform.position + waypoints[j + 1].transform.position);
            p1 = waypoints[j + 1].transform.position;
            p2 = 0.5f * (waypoints[j + 1].transform.position + waypoints[j + 2].transform.position);

            //set points of quadrati curve
            Vector3 position;
            float t;
            float pointStep = 1.0f / numberOfPoints;
            if (j == waypoints.Count - 3)
            {
                pointStep = 1.0f / (numberOfPoints - 1.0f);
                //last point of last segment should reach p2
            }
            for (int i = 0; i < numberOfPoints; i++)
            {
                t = i * pointStep;
                position = (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
                lineRenderer.SetPosition(i + j * numberOfPoints, position);
                pointsForPlayer.Add(position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
