using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [HideInInspector]
    public bool pressed = false;

    public GameObject MovingPlatForm4;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Plate")
        {
            Debug.Log("LUL");
            pressed = true;
            //DO SOMETHING!

        }
        if (pressed == true)
        {
            MovingPlatForm4.GetComponent<Elevator>().enabled = true;
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
