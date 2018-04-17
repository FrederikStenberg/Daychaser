using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour {

    public Animator animator;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxisRaw("Fire") >= 0.1)
            PerformWeaponAttack();
	}

    public void PerformWeaponAttack()
    {
        Sword.PerformAttack(animator);  
    }
}
