using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public Vector3 Direction { get; set; }
    public float Range { get; set; }
    public int Damage { get; set; }

    Vector3 spawnPosition;

    private Transform target;

    void Start()
    {
        Damage = 1;
        Range = 20f;
        spawnPosition = transform.position;
        GetComponent<Rigidbody>().AddForce(Direction * 35f);
    }

    void Update()
    {
        if (Vector3.Distance(spawnPosition, transform.position) >= Range)
        {
            KillBall();
        }

        //Vector3 dir = target.position - transform.position;
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            Debug.Log("I hit" + col.transform.tag);
            col.transform.GetComponent<IPlayer>().TakeDamage(Damage);
            KillBall();
        }
        if (col.transform.tag == "Enemy")
        {
            KillBall();
        }
        if (col.transform.tag == "InteractableObject")
        {
            Debug.Log("I hit" + col.transform.tag);
        }
        else
        KillBall();
    }

    void KillBall()
    {
        Destroy(gameObject);
    }
}