using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [HideInInspector]
    public bool pressed = false;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Plate")
        {
            Debug.Log("LUL");
            pressed = true;
            //DO SOMETHING!

        }
        if (pressed == true)
        {

        }
    }
    void OnCollisionExit(Collision col)
    {
        pressed = false;
        Debug.Log("OFF");
    }
}
