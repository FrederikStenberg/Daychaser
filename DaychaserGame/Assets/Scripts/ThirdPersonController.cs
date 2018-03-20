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
        public string MOUSECAMERA_AXIS = "Mouse X";
        public string CONTROLLERCAMERA_AXIS = "Mouse Y";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();



    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rBody;
    Animator anim;
    float forwardInput, turnInput, jumpInput, mouseturnInput, controllerturnInput;

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
        forwardInput = turnInput = jumpInput = mouseturnInput = controllerturnInput = 0;
    }

    void GetInput()
    {
        // Controls for both 2D and 3D
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS); //Input.GetAxis gets an interpolated value
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS); //Input.GetAxisRaw gets a non-interpolated value (-1, 0 or 1)
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS);
        mouseturnInput = Input.GetAxis(inputSetting.MOUSECAMERA_AXIS);
        controllerturnInput = Input.GetAxis(inputSetting.CONTROLLERCAMERA_AXIS);
    }
    
    void Update()
    {
        GetInput();
        Turn();
    }

    void FixedUpdate()
    {
        Run();
        Jump();

        rBody.velocity = transform.TransformDirection(velocity);
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            // Move
            velocity.z = moveSetting.forwardVel * forwardInput;
        }
        else
            // Zero velocity
            velocity.z = 0;


        if (Mathf.Abs(turnInput) > inputSetting.inputDelay)
        {
            velocity.x = moveSetting.forwardVel * turnInput;
        }
        else
            // Zero velocity
            velocity.x = 0;
    }

    void Turn()
    {
        
        if (Mathf.Abs(mouseturnInput) > inputSetting.inputDelay || Mathf.Abs(controllerturnInput) > inputSetting.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * (mouseturnInput + controllerturnInput) * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
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