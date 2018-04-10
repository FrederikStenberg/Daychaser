using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostSteeringScript : MonoBehaviour
{

    public Transform target;
    public bool navMeshOn;

    NavMeshAgent agent;

    private Vector3 velocity, seekVel;
    public float seekWeight = 1f;

    public float maxSpeed = 5;
    public float maxForce = 2f;

    // Use this for initialization
    void Start()
    {
        if (navMeshOn)
            agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshOn)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            seekVel += Seek() * seekWeight;
            if (seekVel.magnitude >= maxSpeed)
            {
                velocity = Vector3.ClampMagnitude(seekVel, maxSpeed);
            }
            transform.LookAt(target);
            transform.position += velocity * Time.deltaTime;
        }
    }

    private Vector3 Seek()
    {
        Vector3 steer = Vector3.zero;
        Vector3 desired = target.position - transform.position;
        float dist = desired.magnitude;
        desired = desired.normalized;
        desired *= maxSpeed;

        steer = desired - velocity;
        if (steer.magnitude > maxForce)
        {
            steer.Scale(new Vector3(maxForce, maxForce, maxForce));
        }

        Debug.DrawLine(transform.position, transform.position + steer, Color.blue);
        return steer.normalized;
    }
}
