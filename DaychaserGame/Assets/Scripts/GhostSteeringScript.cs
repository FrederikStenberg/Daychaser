using UnityEngine;

public class GhostSteeringScript : MonoBehaviour {
    public GameObject target;
    private Vector3 velocity, seekVel, avoidVel;

    public float maxSpeed = 5;
    public float maxForce = 2f;
    public float seekWeight = 1f;

    public float avoidWeight = 0.5f;


    private void Update()
    {
        seekVel += Seek() * seekWeight;
        if (seekVel.magnitude >= maxSpeed)
        {
            velocity = Vector3.ClampMagnitude(seekVel, maxSpeed);
        }

        //// Calculating avoidance and adding it to the velocity along with the seek velocity.
        //avoidVel += CollisionAvoidance() * avoidWeight;

        //velocity = seekVel + avoidVel;
        //velocity /= (seekWeight + avoidWeight);

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        transform.LookAt(target.transform);
        transform.position += velocity * Time.deltaTime;
    }

    private Vector3 Seek()
    {
        Vector3 steer = Vector3.zero;
        Vector3 desired = target.transform.position - transform.position;
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

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            // Insert damage code here
        }
        else
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), col.collider);
        }
    }

    private Vector3 CollisionAvoidance()
    {
        Vector3 desired = Vector3.zero;
        Vector3 steer = Vector3.zero;
        var dir = (target.transform.position - transform.position).normalized;
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(transform.position, transform.forward, out hit, 20))
        {
            if (hit.transform != transform)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                dir += hit.normal * 50;
            }
        }

        var leftR = transform.position;
        var rightR = transform.position;

        leftR.x -= 2;
        rightR.x += 2;

        if (Physics.Raycast(leftR, transform.forward, out hit, 20))
        {
            if (hit.transform != transform)
            {
                Debug.DrawLine(leftR, hit.point, Color.red);
                dir += hit.normal * 50;
            }
        }
        

        if (Physics.Raycast(rightR, transform.forward, out hit, 20))
        {
            if (hit.transform != transform)
            {
                Debug.DrawLine(rightR, hit.point, Color.red);
                dir += hit.normal * 50;
            }
        }

        var rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
        steer += transform.forward * 5 * Time.deltaTime;

        return steer.normalized;
    }
}
