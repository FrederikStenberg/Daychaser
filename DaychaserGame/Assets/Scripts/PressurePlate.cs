using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [HideInInspector]
    public bool pressed = false;
    public bool move = false;
    private bool check = false;

    public GameObject MovingPlatForm4;
    public Animator BridgeFallAnimation;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Plate")
        {
            pressed = true;
        }
        if (pressed == true)
        {
            MovingPlatForm4.GetComponent<Elevator>().enabled = true;
        }

    }


    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "InteractableObject")
        {
            if (!check)
            {
                if (Input.GetAxisRaw("Fire1") > 0.1f)
                {
                    check = true;
                }

                if (check)
                {
                    move = true;
                    Debug.Log("True");
                }
            }
            if (Input.GetAxisRaw("Fire1") < 0.1f)
            {
                check = false;
                move = false;
            }
        }
    }


    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Plate")
        {
            pressed = false;
            Debug.Log("OFF");
        }
        if (pressed == false)
        {
            MovingPlatForm4.GetComponent<Elevator>().enabled = false;
        }
    }

}
    /*IEnumerator SpeedAdjust()
    {
        if (move == true)
        {
            BridgeFallAnimation["BridgeFall"].speed = 0.1f;
            yield return new WaitForSeconds(0.25f);
            BridgeFallAnimation["BridgeFall"].speed = 0f;
        }

    }
}*/
