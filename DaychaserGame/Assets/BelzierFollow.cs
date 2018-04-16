using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelzierFollow : MonoBehaviour {

    public BezierPoint[] points;
    public Transform character;   
    public float speed;
    public float percentage;

    Rigidbody rbody;
    Vector3[] pointsVector;
    RaycastHit[] hit = new RaycastHit[9];
    float raycastOffset;
    float colliderBottom;
    float rayLength = 100;
    Vector3 colliderSize;
    Vector3 colliderPosition;
    Vector3 colliderCenter;
    bool percentageCanChange = true;

    // Use this for initialization
    void Start () {
        rbody = character.gameObject.GetComponentInChildren<Rigidbody>();
        pointsVector = new Vector3[points.Length];

        for(int i = 0; i < points.Length; i++)
        {
            pointsVector[i] = points[i].transform.position;
        }
        character.position = BezierCurve.GetPoint(percentage, pointsVector);
        character.LookAt(BezierCurve.GetPoint(percentage, pointsVector).normalized);
        colliderSize = Vector3.Scale(character.localScale, character.GetComponentInChildren<BoxCollider>().size);
        colliderCenter = Vector3.Scale(character.localScale, character.GetComponentInChildren<BoxCollider>().center);
        colliderPosition = character.position + colliderCenter;
        colliderBottom = colliderPosition.y - (colliderSize.y / 2);
    }
	
    void Update()
    {
        Debug.DrawRay(character.position, Vector3.forward * rayLength, Color.yellow);
        for(int i = 0; i < hit.Length; i++)
        {
            if (Physics.Raycast(new Vector3(character.position.x, colliderBottom, character.position.z), Vector3.forward, out hit[0], rayLength))
            {
                percentageCanChange = true;
            } else
            {
                percentageCanChange = true;
            }
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        character.position = BezierCurve.GetPoint(percentage, pointsVector);
        if (Input.GetKey(KeyCode.D))
        {
            if(percentage < 1 && percentageCanChange == true)
            {
                 percentage += Time.deltaTime * speed;           
            }           
        }      
        
        if(Input.GetKey(KeyCode.A))
        {
            if(percentage > 0 && percentageCanChange == true)
            {
                percentage -= Time.deltaTime * speed;
            }
        }
	}
}
