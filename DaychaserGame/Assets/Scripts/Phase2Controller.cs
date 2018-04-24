using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Phase2Controller : MonoBehaviour {

    public float speed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0, 1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    bool _getPushedOnce = false;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;
    Vector3 bounce = Vector3.zero;

    Transform cameraT;
    CharacterController controller;

    Animator animator;
    int walkHash = Animator.StringToHash("StartWalk");
    int jumpHash = Animator.StringToHash("Jump");

    void Start() {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        Debug.Log(GetComponent<Player>().currentHealth);
        /// Input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        Move(inputDir);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1) || _getPushedOnce)
        {
            if (_getPushedOnce)
            {
                jumpHeight *= 5;
                FindObjectOfType<AudioManager>().Play("boing");
            }
            Jump();
            animator.Play(jumpHash);
            if (_getPushedOnce)
            {
                jumpHeight /= 5;
                _getPushedOnce = false;
            }
        }

        /// Animations
        float animationSpeedPercent = currentSpeed / speed;
        animator.SetFloat("Walk", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    void Move(Vector2 inputDir)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = speed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (hit.gameObject.tag == "PushObject")
        {
            if ((body == null || body.isKinematic) && hit.controller.velocity.y < -1f)
            {
                _getPushedOnce = true;
            }
        }

        if (hit.gameObject.tag == "Heart")
        {
            if (GetComponent<Player>().currentHealth < 3)
            {
                hit.gameObject.SetActive(false);
                GetComponent<Player>().currentHealth += 1;
            }
        }
    }
}
