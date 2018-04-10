using UnityEngine;
using System.Collections;


public class ThirdPersonControllerCamera : MonoBehaviour
{
    public Transform target;
    public float lookSmooth = 0.09f;
    public Vector3 offsetFromTarget = new Vector3(0, 6, -8);
    public float xTilt = 10;

    Vector3 destination = Vector3.zero;
    ThirdPersonController charController;
    float rotateVel = 100;

    private string MOUSECAMERAX_AXIS = "Mouse X";
    private string MOUSECAMERAY_AXIS = "Mouse Y";
    private string CONTROLLERCAMERAX_AXIS = "Controller X";
    private string CONTROLLERCAMERAY_AXIS = "Controller Y";
    public float inputDelay = 0.01f;
    Quaternion targetRotation;

    float mouseturnXInput, mouseturnYInput,
    controllerturnXInput, controllerturnYInput;

    private void Start()
    {
        SetCameraTarget(target);
        //Movement
        MoveToTarget();
        //Rotation
        LookAtTarget();
        targetRotation = transform.rotation;
        mouseturnXInput = mouseturnYInput = controllerturnXInput = controllerturnYInput = 0;
    }

    public void SetCameraTarget(Transform t)
    {
        target = t;

        if (target != null)
        {
            if (target.GetComponent<ThirdPersonController>())
            {
                charController = target.GetComponent<ThirdPersonController>(); 
            }
            else
                Debug.LogError("The camera's target needs a character controller.");
        }
        else
            Debug.LogError("Your camera needs a target.");
    }

    private void LateUpdate()
    {
        GetInput();
        LookAtTarget();
    }

    void GetInput()
    {
        mouseturnXInput = Input.GetAxis(MOUSECAMERAX_AXIS);
        mouseturnYInput = Input.GetAxis(MOUSECAMERAY_AXIS);
        controllerturnXInput = Input.GetAxis(CONTROLLERCAMERAX_AXIS);
        controllerturnYInput = Input.GetAxis(CONTROLLERCAMERAY_AXIS);
    }

    void MoveToTarget()
    {
        destination = charController.TargetRotation * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    void LookAtTarget()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, transform.eulerAngles.z);


    }

    void LookAround()
    {
        if (Mathf.Abs(mouseturnXInput) > inputDelay || Mathf.Abs(controllerturnXInput) > inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(rotateVel * (mouseturnXInput + controllerturnXInput) * Time.deltaTime, Vector3.up);
            transform.rotation = targetRotation;
        }
        if (Mathf.Abs(mouseturnYInput) > inputDelay || Mathf.Abs(controllerturnYInput) > inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(rotateVel * (mouseturnYInput + controllerturnYInput) * Time.deltaTime, Vector3.right);
            //if (targetRotation.eulerAngles.x > 50)
            //{
            //    Vector3 tempRot = new Vector3(50, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            //    targetRotation = Quaternion.Euler(tempRot);
            //}

            transform.rotation = targetRotation;
        }
    }
}

