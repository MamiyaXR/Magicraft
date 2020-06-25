using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour
{
    public bool active;
    public float velocity;
    [Space]
    public float allowPlayerMove = 0.1f;
    public bool blockPlayerRotation = false;
    public float desiretRotationSpeed = 0.1f;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    private float inputX;
    private float inputZ;
    private float speed;
    private bool isGround;
    private float verticalSpeed;
    private Vector3 desiretMoveDirection;
    private Vector3 moveVector;

    private CharacterController controller;
    private Animator anim;
    private Camera cam;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }

    public void Update()
    {
        InputMagnitude();

        isGround = controller.isGrounded;
        if (isGround)
            verticalSpeed -= 0;
        else
            verticalSpeed -= 1;
        moveVector = new Vector3(0, verticalSpeed * .2f * Time.deltaTime, 0);
        controller.Move(moveVector);
    }

    private void InputMagnitude()
    {
        inputX = active ? Input.GetAxis("Horizontal") : 0;
        inputZ = active ? Input.GetAxis("Vertical") : 0;
        speed = new Vector2(inputX, inputZ).sqrMagnitude;

        if(speed > allowPlayerMove)
        {
            anim.SetFloat("Blend", speed, StartAnimTime, Time.deltaTime);
            PlayerMoveAndRotation(inputX, inputZ);
        } else if(speed < allowPlayerMove)
        {
            anim.SetFloat("Blend", speed, StopAnimTime, Time.deltaTime);
        }
    }

    private void PlayerMoveAndRotation(float inputX, float inputZ)
    {
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        desiretMoveDirection = forward * inputZ + right * inputX;

        if(blockPlayerRotation == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiretMoveDirection), desiretRotationSpeed);
            controller.Move(desiretMoveDirection * velocity * Time.deltaTime);
        }
    }
}
