using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Player : MonoBehaviour {

    public float currentHealth;
    public float maxHealth;
	void Start () {
        this.currentHealth = this.maxHealth;
	}

    public void TakeDamage(int amount)
    {
        Debug.Log("Player takes " + amount + " damage!");   
        currentHealth -= amount;
        if (currentHealth <= 0)
            die();

    }

    void die()
    {
        Debug.Log("You dead son");
        this.currentHealth = this.maxHealth;

    }
	// Update is called once per frame
	void Update () {
		
	}
}
