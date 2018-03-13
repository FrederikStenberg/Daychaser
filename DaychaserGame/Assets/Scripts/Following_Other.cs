using UnityEngine;

[ExecuteInEditMode]
public class Following_Other : MonoBehaviour {
    public GameObject followed;

    private void Update()
    {
        GetComponent<Rigidbody>().velocity = 0.95f * GetComponent<Rigidbody>().velocity + 0.05f * (followed.transform.position - transform.position);
    }

}
