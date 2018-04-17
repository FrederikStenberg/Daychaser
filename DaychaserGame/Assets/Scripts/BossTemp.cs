using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BossTemp : MonoBehaviour, IEnemy {

    public LayerMask aggroLayerMask;
    public float currentHealth;
    public float maxHealth = 10;
    public Animator animator;
    public Vector3 Direction;
    Fireball fireballClass;
    public GameObject fireballPrefab;
    public Transform ProjectileSpawn;
   
    public float range = 40f;
    public float fireRate = 1f;

    Transform spawnProjectile;

    private float fireCountdown = 3f;
    private Transform target;
    public float dstToTarget = 10;
    private Player player;
    private NavMeshAgent navAgent;
    private Collider[] withinAggroColliders;

    Vector3 playerPos;
    private bool _attackCD = false;

    void Start () {

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        //fireball = Resources.Load<Fireball>("Prefabs/Projectiles/Fireball");
        spawnProjectile = transform.Find("ProjectileSpawn").transform;
        //navAgent = GetComponent<NavMeshAgent>();
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
        ProjectileSpawn.LookAt(playerPos);
        fireball.Direction = ProjectileSpawn.forward;
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

        playerPos = GameObject.Find("Target look").transform.position;
        Vector3 dir = playerPos - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 1f).eulerAngles;
        //Vector3 rotation = lookRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);


         if (fireCountdown <= 0f)
         {

             animator.SetTrigger("ShootFire");
             fireCountdown = fireRate;

            Debug.Log("The distance between Boss and target is: " + Vector3.Distance(playerPos, transform.position));
            if (_attackCD)
            {
                Debug.Log("Boss tried to Melee the target!");
                PerformAttack();
            }
            else
            {
                Shoot();
                Debug.Log("Boss tried to Shoot the target!");
            }

            if (Vector3.Distance(playerPos, transform.position) <= dstToTarget)
            {
                Debug.Log("Boss set next attack to be Melee!");
                _attackCD = true;
            }
            else
            {
                Debug.Log("Boss set next attack to be Shoot!");
            }
            fireCountdown = fireRate;

         }
        //Debug.Log(fireCountdown);
        fireCountdown -= Time.deltaTime;
    }
}
