using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour {

    public GameObject boss;
    public GameObject hearts;

	// Use this for initialization
	void Start () {
        Debug.Log(hearts);
        boss.SetActive(true);
        hearts.SetActive(true);
	}
}
