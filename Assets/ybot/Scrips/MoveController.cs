using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MoveController : MonoBehaviour
{
    public bool active = true;
    public float velocity = 7f;
    public float verticalVel = 3.5f;
    public Transform groundCheck;
    public LayerMask layer;
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
    private bool isJumpping;
    private bool isFalling;
    private Vector3 desiretMoveDirection;
    private Vector3 moveVector;
    private float playerYPosSave;
    private float verticalSpeed = 0;

    private CharacterController controller;
    private Animator anim;

    private bool isDancing = false;
    private bool isWaving = false;

    private static MoveController _instance;
    public static MoveController instance { get => _instance; }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    public void Start()
    {
        playerYPosSave = transform.position.y;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    public void Update()
    {
        JumpCheck();
        InputMagnitude();
        //***************************************************************
        if (isDancing || isWaving)
        {
            if (Input.anyKeyDown)
            {
                anim.SetTrigger("End Trig");
                isDancing = false;
                isWaving = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                anim.SetTrigger("Dancing Trig");
                isDancing = true;
            } else if(Input.GetKeyDown(KeyCode.V))
            {
                anim.SetTrigger("Waving Trig");
                isWaving = true;
            }
        }
        //***************************************************************
    }
    private void JumpCheck()
    {
        if (Physics.OverlapSphere(groundCheck.position, 0.1f, layer).Length != 0)
            isGround = true;
        else
            isGround = false;
        anim.SetBool("IsGround", isGround);
        moveVector = new Vector3(0, verticalSpeed * Time.deltaTime, 0);
        controller.Move(moveVector);
        if (isGround)
        {
            verticalSpeed -= 0;
            isJumpping = false;
            isFalling = false;
            if (Input.GetKeyDown(KeyCode.Space))
                verticalSpeed = active ? verticalVel : 0;
        }
        else
        {
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
            if (transform.position.y > playerYPosSave && !isJumpping)
            {
                isJumpping = true;
                anim.SetTrigger("Jump Up");
            }
            else if (transform.position.y < playerYPosSave && !isFalling)
            {
                isFalling = true;
                anim.SetTrigger("Jump Down");
            }
        }
        playerYPosSave = transform.position.y;
    }
    private void InputMagnitude()
    {
        inputX = active ? Input.GetAxis("Horizontal") : 0;
        inputZ = active ? Input.GetAxis("Vertical") : 0;
        speed = new Vector2(inputX, inputZ).sqrMagnitude;

        if(speed > allowPlayerMove)
        {
            anim.SetFloat("Blend", speed, StartAnimTime, Time.deltaTime);
            PlayerMove(inputX, inputZ);
            if(CameraController.instance.cameraMode == CameraMode.ThirdPersonCam)
                PlayerRotation();
        } else if(speed < allowPlayerMove)
        {
            anim.SetFloat("Blend", speed, StopAnimTime, Time.deltaTime);
        }
        if(CameraController.instance.cameraMode == CameraMode.FirstPersonCam)
            PlayerRotation();
    }
    private void PlayerRotation()
    {
        if (blockPlayerRotation)
            return;
        
        if(CameraController.instance.cameraMode == CameraMode.FirstPersonCam)
        {
            float rotationX = 0f;
            rotationX += Camera.main.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 5f;
            transform.localEulerAngles = new Vector3(0, rotationX, 0);
        }
        else if (CameraController.instance.cameraMode == CameraMode.ThirdPersonCam)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiretMoveDirection), desiretRotationSpeed);
    }
    private void PlayerMove(float inputX, float inputZ)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        desiretMoveDirection = forward * inputZ + right * inputX;

        controller.Move(desiretMoveDirection.normalized * velocity * Time.deltaTime);
    }
}
