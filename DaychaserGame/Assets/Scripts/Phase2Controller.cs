﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2Controller : MonoBehaviour {

    public float speed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0, 1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    Transform cameraT;
    CharacterController controller;

    Animator animator;
    int walkHash = Animator.StringToHash("StartWalk");
    int jumpHash = Animator.StringToHash("Jump");

    void Start () {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
	}

	void Update () {
        /// Input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        Move(inputDir);

        if (Input.GetKeyDown (KeyCode.Space))
        {
            Jump();
            //animator.SetTrigger(jumpHash);
        }

        /// Animations
        float animationSpeedPercent = currentSpeed / speed;
        //animator.SetBool("StartWalk", true);
        animator.SetFloat("StartWalk", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
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
            //animator.SetBool("Jump", true);
        }
        else
        {
            //animator.SetBool("Jump", false);
        }
    }

    float GetModifiedSmoothTime (float smoothTime)
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
}
