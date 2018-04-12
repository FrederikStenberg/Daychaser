using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour
{
    [System.Serializable] //This allows us to see the classes from the inspector
    public class MoveSettings
    {
        public float forwardVel = 6;
        public float rotateVel = 100;
        public float jumpVel = 10;
        public float distToGrounded = 1f;
        public LayerMask ground;
        
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.75f;
    }
    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
        public string MOUSECAMERAX_AXIS = "Mouse X";
        public string MOUSECAMERAY_AXIS = "Mouse Y";
        public string CONTROLLERCAMERAX_AXIS = "Controller X";
        public string CONTROLLERCAMERAY_AXIS = "Controller Y";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    public GameObject camera;
    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rBody;
    Animator anim;
    float forwardInput, turnInput, jumpInput, mouseturnXInput, mouseturnYInput,
        controllerturnXInput, controllerturnYInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }

    void Start()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rBody = GetComponent<Rigidbody>();
        else
            Debug.Log("The character does not have a rigid body attached");
        forwardInput = turnInput = jumpInput = mouseturnXInput = mouseturnYInput =
            controllerturnXInput = controllerturnYInput = 0;
    }

    void GetInput()
    {
        // Controls for both 2D and 3D
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS); //Input.GetAxis gets an interpolated value
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS); //Input.GetAxisRaw gets a non-interpolated value (-1, 0 or 1)
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS);
        mouseturnXInput = Input.GetAxis(inputSetting.MOUSECAMERAX_AXIS);
        mouseturnYInput = Input.GetAxis(inputSetting.MOUSECAMERAY_AXIS);
        controllerturnXInput = Input.GetAxis(inputSetting.CONTROLLERCAMERAX_AXIS);
        controllerturnYInput = Input.GetAxis(inputSetting.CONTROLLERCAMERAY_AXIS);
    }
    
    void Update()
    {
        GetInput();
        Turn();
    }

    void FixedUpdate()
    {
        Run(GetComponent<Animator>());
        Jump();

        rBody.velocity = transform.TransformDirection(velocity);
    }

   public void Run(Animator animator)
    {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            // Move
            animator.SetTrigger("StartWalk");
            if (forwardInput > 0)
                velocity.z = moveSetting.forwardVel * forwardInput;
            else
            {
                velocity.z = moveSetting.forwardVel * forwardInput;
                targetRotation *= Quaternion.Euler(targetRotation.x, targetRotation.y - 180, targetRotation.z);
                Debug.Log("backwards");
            }
        }
        else
        {
            animator.SetTrigger("StopWalk");
            velocity.z = 0;
        }
            // Zero velocity
          


        if (Mathf.Abs(turnInput) > inputSetting.inputDelay)
        {
            animator.SetTrigger("StartWalk");
            //velocity.x = moveSetting.forwardVel * turnInput;
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * (turnInput) * Time.deltaTime, Vector3.up);
            transform.rotation = targetRotation;
        }
        else
        {
            animator.SetTrigger("StopWalk");
            velocity.x = 0;
        }
            // Zero velocity
            
    }

    void Turn()
    {
        if (Mathf.Abs(mouseturnXInput) > inputSetting.inputDelay || Mathf.Abs(controllerturnXInput) > inputSetting.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * (mouseturnXInput + controllerturnXInput) * Time.deltaTime, Vector3.up);
            camera.transform.rotation = targetRotation;
        }
        if (Mathf.Abs(mouseturnYInput) > inputSetting.inputDelay || Mathf.Abs(controllerturnYInput) > inputSetting.inputDelay)
        {
            //targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * (mouseturnYInput + controllerturnYInput) * Time.deltaTime, Vector3.right);
            if (targetRotation.eulerAngles.x > 50)
            {
                //Vector3 tempRot = new Vector3(50, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
                //targetRotation = Quaternion.Euler(tempRot);
            }
            camera.transform.rotation = targetRotation;
        }

    }

    void Jump()
    {
        if (jumpInput > 0 && Grounded())
        {
            // Jump
            velocity.y = moveSetting.jumpVel;
            this.GetComponent<Animator>().Play("Jump", -1, 0.0f);
        }
        else if (jumpInput == 0 && Grounded())
        {
            //Zero out our velocity y when not jumping
            velocity.y = 0;
        }
        else
        {
            // decrease velocity y because we are falling
            velocity.y -= physSetting.downAccel;
        }
    }
}