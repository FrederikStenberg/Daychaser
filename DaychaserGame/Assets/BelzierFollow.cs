using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelzierFollow : MonoBehaviour {

    public BezierPoint[] points;
    public Transform character;   
    public float speed;

    Rigidbody rbody;
    Vector3[] pointsVector;
    public float percentage;

    // Use this for initialization
    void Start () {
        rbody = character.gameObject.GetComponentInChildren<Rigidbody>();
        pointsVector = new Vector3[points.Length];

        for(int i = 0; i < points.Length; i++)
        {
            pointsVector[i] = points[i].transform.position;
        }
        character.position = BezierCurve.GetPoint(percentage, pointsVector);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Debug.Log(rbody.velocity);
        character.position = BezierCurve.GetPoint(percentage, pointsVector);   

        if (Input.GetKey(KeyCode.D))
        {
            if(percentage < 1)
            {
                if(rbody.velocity.x != 0 && rbody.velocity.z != 0)
                {
                    percentage += Time.deltaTime * speed;
                }             
            }           
        }      
        
        if(Input.GetKey(KeyCode.A))
        {
            if(percentage > 0)
            {
                if (rbody.velocity.x != 0 && rbody.velocity.z != 0)
                {
                    percentage -= Time.deltaTime * speed;
                }
            }
        }
	}
}
