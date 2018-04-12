using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelzierFollow : MonoBehaviour {

    public BezierPoint[] points;
    public Transform character;   
    public float speed;

    Vector3[] pointsVector;
    float percentage;

    // Use this for initialization
    void Start () {

        pointsVector = new Vector3[points.Length];

        for(int i = 0; i < points.Length; i++)
        {
            pointsVector[i] = points[i].transform.position;
        }
        character.position = BezierCurve.GetPoint(percentage, pointsVector);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        character.position = BezierCurve.GetPoint(percentage, pointsVector);

        if (Input.GetKey(KeyCode.D))
        {
            if(percentage < 1)
            {
                percentage += Time.deltaTime * speed;
            }           
        }      
        
        if(Input.GetKey(KeyCode.A))
        {
            if(percentage > 0)
            {
                percentage -= Time.deltaTime * speed;
            }
        }
	}
}
