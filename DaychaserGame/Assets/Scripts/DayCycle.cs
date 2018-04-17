using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour {

    public GameObject boss;
    GameObject[] hearts;
    public int heartAmount;

	// Use this for initialization
	void Start () {
        hearts = GameObject.FindGameObjectsWithTag("Heart");
        Debug.Log(hearts);
        boss.SetActive(true);
        foreach (GameObject heart in hearts) {
            heart.SetActive(true);
        }
	}
}
