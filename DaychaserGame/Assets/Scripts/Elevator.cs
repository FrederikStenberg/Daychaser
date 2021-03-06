﻿using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour {

    public Transform movingPlatform;
    public Transform position1;
    public Transform position2;
    public Vector3 newPosition;
    public string currentState;
    public float smooth;
    public float resetTime;

	// Use this for initialization
	void Start () {
        ChangeTarget();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        movingPlatform.position = Vector3.Lerp(movingPlatform.position, newPosition, smooth * Time.deltaTime);
	}
    void ChangeTarget()
    {
        if (currentState == "To1" || currentState == "")
        {
            currentState = "To2";
            newPosition = position2.position;
        }
        else if (currentState == "To2")
        {
            currentState = "To1";
            newPosition = position1.position;
        }
      
        Invoke("ChangeTarget", resetTime);
    }

}
