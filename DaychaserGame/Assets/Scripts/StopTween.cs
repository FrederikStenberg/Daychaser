using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTween : MonoBehaviour {

    public GameObject controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "ObjectCollider")
        {
            iTween.Stop(controller);
            Debug.Log("Stop iTween");
        }
    }
}
