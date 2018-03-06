using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollowPlayer : MonoBehaviour {

    public GameObject player;
    float moveSpeed = 4;
    float MaxDist = 10;
    float minDist = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(player.transform.position);

        if(Vector3.Distance(transform.position, player.transform.position) >= minDist)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
	}
}
