using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BossTemp : MonoBehaviour, IEnemy {

    public LayerMask aggroLayerMask;
    public float currentHealth;
    public float maxHealth;

    private Player player;
    private NavMeshAgent navAgent;
    private Collider[] withinAggroColliders;


	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
	}

    //Fixed to optimize performance. No need to check more often than 50 times a second lol.
	void FixedUpdate () {
        //checks for anything within a sphere around the boss with 10 radius.
        withinAggroColliders = Physics.OverlapSphere(transform.position, 10, aggroLayerMask);
        if (withinAggroColliders.Length > 0)
        {
            //If player is within the aggro radius, this does something
            // Takes the first player found in the Array, which is always our player
            ChasePlayer(withinAggroColliders[0].GetComponent<Player>());
        }
	}

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            die();

    }


    void ChasePlayer(Player player)
    {
        navAgent.SetDestination(player.transform.position);
        this.player = player;
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            //Attack boye
            if(!IsInvoking("PerformAttack"))
                InvokeRepeating("PerformAttack", 1f, 2); //Boss will attack every 2 seconds when its been in range for 0.5 seconds.
        }
        else
        {
            CancelInvoke("PerformAttack");
        }
        
    }

    void die()
    {
        Destroy(gameObject);
    }

    public void PerformAttack()
    {
        player.TakeDamage(1);
    }
}
