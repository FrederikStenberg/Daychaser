using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTemp : MonoBehaviour, IEnemy {

    public float currentHealth;
    public float maxHealth;


	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            die();

    }

    void die()
    {
        Destroy(gameObject);
    }

    public void PerformAttack()
    {
       
    }
}
