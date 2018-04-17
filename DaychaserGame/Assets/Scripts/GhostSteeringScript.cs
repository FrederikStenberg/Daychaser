using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostSteeringScript : MonoBehaviour
{

    public Transform target;
    private CharacterController controller;
    private Collider collider;

    private Vector3 velocity, seekVel;
    private Vector3 defaultPos;

    public float speed = 2;

    /*public*/ float seekWeight = 1f;
    /*public*/ float maxForce = 2f;

    // Use this for initialization
    void Start()
    {
        collider = gameObject.GetComponent<Collider>();
        velocity = gameObject.GetComponent<Rigidbody>().velocity;
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /// AI Steering script using Seek (bugs out after a bit of time)s
        //seekVel += Seek() * seekWeight;
        //if (seekVel.magnitude >= maxSpeed)
        //{
        //    velocity = Vector3.ClampMagnitude(seekVel, maxSpeed);
        //}
        //transform.position += velocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        transform.LookAt(target);
    }

    /// Unused function
    private Vector3 Seek()
    {
        Vector3 steer = Vector3.zero;
        Vector3 desired = target.position - transform.position;
        float dist = desired.magnitude;
        desired = desired.normalized;
        desired *= speed;

        steer = desired - velocity;
        if (steer.magnitude > speed)
        {
            steer.Scale(new Vector3(maxForce, maxForce, maxForce));
        }

        Debug.DrawLine(transform.position, transform.position + steer, Color.blue);
        return steer.normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.position = defaultPos;
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }
        else
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), collider);
        }
    }
}
