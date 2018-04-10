using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour {

    public GameObject target;
    public float damping;
    public float cameraDefaultHeight;

	// Update is called once per frame
	void Update () {
        //Rotation
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

        //Position
        transform.position = new Vector3(transform.position.x, target.transform.position.y + cameraDefaultHeight, transform.position.x);
	}
}
