using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [HideInInspector]
    public bool pressed = false;
    public bool move = false;
    private bool check = false;

    public GameObject MovingPlatForm4;
    public GameObject Bridge;
    public Animator anim;

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

        if (col.tag == "Player")
        {
            anim.SetTrigger("CubeTrigger");
        }

    }

   

    //void OnTriggerStay(Collider col)
    //{
    //    if (col.gameObject.tag == "InteractableObject")
    //    {
    //        if (!check)
    //        {
    //            if (Input.GetAxisRaw("Fire1") > 0.1f)
    //            {
    //                check = true;
    //            }

    //            if (check)
    //            {
    //                move = true;
    //                Debug.Log("True");
    //            }
    //        }
    //        if (Input.GetAxisRaw("Fire1") < 0.1f)
    //        {
    //            check = false;
    //            move = false;
    //        }
    //    }
    //}

    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "InteractableObject")
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Bridge.GetComponent<Animator>().enabled = true;
                StartCoroutine(SpeedAdjust());
            }
        }
    }

    IEnumerator SpeedAdjust()
    {
        Bridge.GetComponent<Animator>().speed = 10f;
        yield return new WaitForSeconds(0.05f);
        Bridge.GetComponent<Animator>().speed = 0f;
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
            //MovingPlatForm4.GetComponent<Elevator>().enabled = false;
        }
    }
}
