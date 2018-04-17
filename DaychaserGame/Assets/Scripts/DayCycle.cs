using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour {

    GameObject ghost;
    GameObject boss;
    GameObject[] hearts;
    public int heartAmount;

	// Use this for initialization
	void Start () {
        hearts = GameObject.FindGameObjectsWithTag("Heart");
        Debug.Log(hearts);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
