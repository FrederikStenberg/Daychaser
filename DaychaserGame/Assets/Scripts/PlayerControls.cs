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
            if (phaseOneCurveObjects[currentPointTargetNumber - 1] != null)
            {
                for(int i = 0; i < phaseOneCurveObjects.Count; i++)
                {
                    RaycastHit hit;
                    Ray ray = new Ray(phaseOneCurveObjects[i].transform.position, Vector3.up);
                    if(Physics.Raycast(ray, out hit))
                    {
                        if(hit.collider.tag == "Player")
                        {
                            if(currentPointTargetNumber != i)
                            {
                                currentPointTargetNumber = currentPointTargetNumber - 1;
                            }                          
                        }
                    }
                }
                transform.Rotate(new Vector3(transform.rotation.x, phaseOneCurveObjects[currentPointTargetNumber - 1].transform.position.y, transform.rotation.z), Space.World);
            }
        }  else if (Input.GetKey(KeyCode.D) && speed < maxSpeed)
        {
            speed = speed + accel * Time.deltaTime;
            if(phaseOneCurveObjects[currentPointTargetNumber + 1] != null)
            {
                for (int i = 0; i < phaseOneCurveObjects.Count; i++)
                {
                    RaycastHit hit;
                    Ray ray = new Ray(phaseOneCurveObjects[i].transform.position, Vector3.up);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Player")
                        {
                            if (currentPointTargetNumber != i)
                            {
                                currentPointTargetNumber = currentPointTargetNumber + 1;
                            }
                        }
                    }
                }
                transform.Rotate(new Vector3(transform.rotation.x, phaseOneCurveObjects[currentPointTargetNumber + 1].transform.position.y, transform.rotation.z), Space.World);
            }           
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

        if (transform.position.x == currentPointTarget.x)
        {
            currentPointTarget = phaseOneCurveObjects[currentPointTargetNumber + 1].transform.position;
            currentPointTargetNumber += 1;
        }

        Debug.Log(currentPointTargetNumber);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void dayMovement()
    {

    }
}
