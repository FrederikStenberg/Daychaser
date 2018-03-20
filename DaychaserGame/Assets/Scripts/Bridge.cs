using System.Collections;
using UnityEngine;

public class Bridge : MonoBehaviour {

    public GameObject bridge;
    public Animation anim;
    bool move = false;


	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxisRaw("Fire1") > 0.1f)
        {
            move = true;
        }
        else if (Input.GetAxisRaw("Fire1") < 0.1f)
        {
            move = false;
        }
	}

  
    IEnumerator SpeedAdjust()
    {
        if (move == true)
        {
            anim["BridgeFall"].speed = 0.1f;
            yield return new WaitForSeconds(0.25f);
            anim["BridgeFall"].speed = 0f;
        }
        
    }
}
