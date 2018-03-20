using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public LightPickup lPickUp;
    //public BelzierScript bScript;

    //Movement variables
    public float speed = 0;
    public float maxSpeed = 10;
    public float accel = 10;
    public float decel = 10;

    //Other variables
    public GameObject accuratePathPref;
    List<Vector3> phaseOneCurvePositions;
    List<GameObject> phaseOneCurveObjects = new List<GameObject>();
    Vector3 currentPointTarget;
    int currentPointTargetNumber;

    // Use this for initialization
    void Start () {
        //phaseOneCurvePositions = bScript.pointsForPlayer;
        for(int i = 0; i < phaseOneCurvePositions.Count; i++)
        {
            phaseOneCurveObjects.Add(Instantiate(accuratePathPref, phaseOneCurvePositions[i], Quaternion.identity));
        }

        currentPointTarget = phaseOneCurveObjects[0].transform.position;
        //currentPointTargetNumber = 0;
    }

    // Update is called once per frame
    void Update () {

		if(lPickUp.currentPhase == "night")
        {
            nightMovement();
        } else
        {
            dayMovement();
        }
	}

    void nightMovement()
    {
        if(Input.GetKey(KeyCode.A) && speed > -maxSpeed)
        {
            speed = speed - accel * Time.deltaTime;
            characterDirection = Direction.Reverse;

            //if (phaseOneCurveObjects[currentPointTargetNumber - 1] != null)
            //{
            //    for(int i = 0; i < phaseOneCurveObjects.Count; i++)
            //    {
            //        RaycastHit hit;
            //        Ray ray = new Ray(phaseOneCurveObjects[i].transform.position, Vector3.up);
            //        if(Physics.Raycast(ray, out hit))
            //        {
            //            if(hit.collider.tag == "Player")
            //            {
            //                if(currentPointTargetNumber != i)
            //                {
            //                    currentPointTargetNumber = currentPointTargetNumber - 1;
            //                }                          
            //            }
            //        }
            //    }
            //    transform.Rotate(new Vector3(transform.rotation.x, phaseOneCurveObjects[currentPointTargetNumber - 1].transform.position.y, transform.rotation.z), Space.World);
            //}
        }  else if (Input.GetKey(KeyCode.D) && speed < maxSpeed)
        {
            speed = speed + accel * Time.deltaTime;
            characterDirection = Direction.Forward;

            //if(phaseOneCurveObjects[currentPointTargetNumber + 1] != null)
            //{
            //    for (int i = 0; i < phaseOneCurveObjects.Count; i++)
            //    {
            //        RaycastHit hit;
            //        Ray ray = new Ray(phaseOneCurveObjects[i].transform.position, Vector3.up);
            //        if (Physics.Raycast(ray, out hit))
            //        {
            //            if (hit.collider.tag == "Player")
            //            {
            //                if (currentPointTargetNumber != i)
            //                {
            //                    currentPointTargetNumber = currentPointTargetNumber + 1;
            //                }
            //            }
            //        }
            //    }
            //    transform.Rotate(new Vector3(transform.rotation.x, phaseOneCurveObjects[currentPointTargetNumber + 1].transform.position.y, transform.rotation.z), Space.World);
            //}           
        } else
        {
            if(speed > decel * Time.deltaTime)
            {
                speed = speed - decel * Time.deltaTime;
            } else if (speed < -decel * Time.deltaTime)
            {
                speed = speed + decel * Time.deltaTime;
            } else
            {
                speed = 0;
            }
        }

        //if (transform.position.x == currentPointTarget.x)
        //{
        //    currentPointTarget = phaseOneCurveObjects[currentPointTargetNumber + 1].transform.position;
        //    currentPointTargetNumber += 1;
        //}

        //Debug.Log(currentPointTargetNumber);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void dayMovement()
    {

    }

    void OnDrawGizmos()
    {
        iTween.DrawPath(controlPath, Color.blue);
    }

    public Transform[] controlPath;
    private float pathPosition = 0;
    private float lookAheadAmount = .01f;
    public enum Direction { Forward, Reverse };
    private Direction characterDirection;
    void followNightPath()
    {
        float pathPercent = pathPosition % 1;
        Vector3 coordinateOnPath = iTween.PointOnPath(controlPath, pathPercent);
        Vector3 lookTarget;

        //calculate look data if we aren't going to be looking beyond the extents of the path:
        if (pathPercent - lookAheadAmount >= 0 && pathPercent + lookAheadAmount <= 1)
        {

            //leading or trailing point so we can have something to look at:
            if (characterDirection == Direction.Forward)
            {
                lookTarget = iTween.PointOnPath(controlPath, pathPercent + lookAheadAmount);
            }
            else
            {
                lookTarget = iTween.PointOnPath(controlPath, pathPercent - lookAheadAmount);
            }

            //look:
            transform.LookAt(lookTarget);

            //nullify all rotations but y since we just want to look where we are going:
            float yRot = transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, yRot, 0);
        }

        //if (Physics.Raycast(coordinateOnPath, -Vector3.up, out hit, rayLength))
        //{
        //    Debug.DrawRay(coordinateOnPath, -Vector3.up * hit.distance);
        //    floorPosition = hit.point;
        //}
    }
}
