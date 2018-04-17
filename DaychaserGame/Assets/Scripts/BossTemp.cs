using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BossTemp : MonoBehaviour, IEnemy {

    public LayerMask aggroLayerMask;
    public float currentHealth;
    public float maxHealth;
    public Animator animator;
    public Vector3 Direction;
    Fireball fireballClass;
    public GameObject fireballPrefab;
    public Transform ProjectileSpawn;
   
    public float range = 40f;
    public float fireRate = 1f;

    Transform spawnProjectile;

    private float fireCountdown = 0f;
    private Transform target;
    private Player player;
    private NavMeshAgent navAgent;
    private Collider[] withinAggroColliders;


	void Start () {

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        //fireball = Resources.Load<Fireball>("Prefabs/Projectiles/Fireball");
        spawnProjectile = transform.Find("ProjectileSpawn").transform;
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
	}

    void UpdateTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        foreach (GameObject player in players)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestPlayer = player;
            }
        }

        if (nearestPlayer != null && shortestDistance <= range)
        {
            target = nearestPlayer.transform;
        } else
        {
            target = null;
        }
    }

    //Fixed to optimize performance. No need to check more often than 50 times a second lol.
	void FixedUpdate () {
        //checks for anything within a sphere around the boss with 20 radius.

        /* withinAggroColliders = Physics.OverlapSphere(transform.position, 20, aggroLayerMask);
         if (withinAggroColliders.Length > 0)
         {
             //If player is within the aggro radius, this does something
             // Takes the first player found in the Array, which is always our player
             ChasePlayer(withinAggroColliders[0].GetComponent<Player>());
         }*/
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            die();

    }


   /* void ChasePlayer(Player player)
    {
        this.player = player;
        animator.SetTrigger("BossWalk");
        navAgent.SetDestination(player.transform.position);
        
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            //Attack boye
            if(!IsInvoking("PerformAttack"))
                InvokeRepeating("PerformAttack", 0.5f, 1.5f);//Boss will attack every 2 seconds when its been in range for 0.5 seconds.
            Debug.Log("Jeg slår på dig");
        }   
        else
        {
            CancelInvoke("PerformAttack");
            Debug.Log("Jeg slår ikke på dig");
        }
        
    }*/

    void die()
    {
        Destroy(gameObject);
        Debug.Log("Congratulations! You've defeated the boss of this level.");
    }

    public void Shoot()
    {
        

        GameObject fireballGO = (GameObject)Instantiate(fireballPrefab, ProjectileSpawn.position, ProjectileSpawn.rotation);
        Fireball fireball = fireballGO.GetComponent<Fireball>();

        fireball.Direction = ProjectileSpawn.forward;
        animator.SetTrigger("ShootFire");
        if (fireball != null)
            fireball.Seek(target);
        //Fireball fireballInstance = (Fireball)Instantiate(fireball, ProjectileSpawn.position, ProjectileSpawn.rotation);

    }

    public void PerformAttack()
    {
        animator.SetTrigger("BossAtk");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<IPlayer>().TakeDamage(1);
        }
    }

    void Update ()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

         if (Input.GetKeyDown(KeyCode.U))
            animator.SetTrigger("ShootFire");

         if (fireCountdown <= 0f)
         {
             Shoot();
             fireCountdown = 1 / fireRate;
         }

         fireCountdown -= Time.deltaTime;
    }
}
