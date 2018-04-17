using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostSteeringScript : MonoBehaviour
{
    public Transform target;
    public Transform goBack;
    public float speed = 2;

    bool spawnCooldownCheck = false;
    bool isGoingBack = false;
    bool endOnce = true;

    private void Start()
    {
        StartCoroutine(spawnCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCooldownCheck == true)
        {
            GetComponent<Animator>().enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.LookAt(target);
        }  

        if(isGoingBack == true && Vector3.Distance(transform.position, goBack.position) > 0.2f) {
            speed = 8;
            transform.position = Vector3.MoveTowards(transform.position, goBack.transform.position, speed * Time.deltaTime);
            transform.LookAt(goBack);
            Debug.Log(Vector3.Distance(transform.position, goBack.position));
        }

        if (isGoingBack == true && Vector3.Distance(transform.position, goBack.position) < 1)
        {
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetTrigger("trigger");
            if(endOnce == true)
            {
                StartCoroutine(waitToDie());
                endOnce = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        } else
        {
            isGoingBack = true;
            spawnCooldownCheck = false;
        }
    }

    IEnumerator spawnCooldown()
    {
        yield return new WaitForSeconds(3);
        spawnCooldownCheck = true;
    }

    IEnumerator waitToDie()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
