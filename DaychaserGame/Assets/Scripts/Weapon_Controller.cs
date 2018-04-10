using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour {

    public Animator animator;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.X))
            PerformWeaponAttack();
	}

    public void PerformWeaponAttack()
    {
        Sword.PerformAttack(animator);  
    }
}
