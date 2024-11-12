using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Health.IHealthListener
{
    public float walkingSpeed = 7f;
    public float mouseSens = 1f;

    public float gravity = 10f;
    public float terminalSpeed = 50f; // MaxFallingSpeed

    public float jumpSpeed = 10f;

    public Transform cameraTransform;

    public List<Weapon> weapons;
    int currentWeaponIndex;

    float horizontalAngle;
    float verticalAngle;
    float verticalSpeed;
    float groundedTimer;
    bool isGrounded;

    InputAction moveAction;
    InputAction lookAction;
    InputAction fireAction;
    InputAction reloadAction;
    InputAction changeWeaponAction;

    CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputActionAsset inputActions = GetComponent<PlayerInput>().actions;

        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        fireAction = inputActions.FindAction("Fire");
        reloadAction = inputActions.FindAction("Reload");
        changeWeaponAction = inputActions.FindAction("ChangeWeapon");

        characterController = GetComponent<CharacterController>();

        horizontalAngle = transform.localEulerAngles.y;
        verticalSpeed = 0f;

        groundedTimer = 0f;
        isGrounded = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            return;
        }
        MoveControl();
        TurnControl();
        GravityControl();
        ActionControl();
    }

    void MoveControl()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();

        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);

        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        move = move * walkingSpeed * Time.deltaTime;
        move = transform.TransformDirection(move);
        characterController.Move(move);
    }

    void TurnControl()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();

        float turnPlayer = look.x * mouseSens;
        horizontalAngle += turnPlayer;
        if (horizontalAngle >= 360) horizontalAngle -= 360;
        if (horizontalAngle < 0) horizontalAngle += 360;
        Vector3 currentAngle = transform.localEulerAngles;
        currentAngle.y = horizontalAngle;
        transform.localEulerAngles = currentAngle;

        float turnCam = look.y * mouseSens;
        verticalAngle -= turnCam;
        verticalAngle = Mathf.Clamp(verticalAngle, -89f, 89f);
        currentAngle = cameraTransform.localEulerAngles;
        currentAngle.x = verticalAngle;
        cameraTransform.localEulerAngles = currentAngle;
    }

    void GravityControl()
    {
        verticalSpeed -= gravity * Time.deltaTime;

        if (verticalSpeed < -terminalSpeed)
        {
            verticalSpeed = -terminalSpeed;
        }

        Vector3 verticalMove = new Vector3(0, verticalSpeed, 0);
        verticalMove *= Time.deltaTime;

        CollisionFlags flag = characterController.Move(verticalMove);
        if ((flag & (CollisionFlags.Below | CollisionFlags.Above)) != 0)
        {
            verticalSpeed = 0;
        }

        if (!characterController.isGrounded)
        {
            if (isGrounded)
            {
                groundedTimer += Time.deltaTime;
                if (groundedTimer > 0.5f)
                {
                    isGrounded = false;
                }
            }
        }
        else
        {
            isGrounded = true;
            groundedTimer = 0f;
        }
    }

    void ActionControl()
    {
        if (fireAction.WasPressedThisFrame())
        {
            weapons[currentWeaponIndex].FireWeapon();
        }
        if (reloadAction.WasPressedThisFrame())
        {
            weapons[currentWeaponIndex].ReloadWeapon();
        }
    }

    public void OnChangeWeapon()
    {
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        currentWeaponIndex++;
        if (currentWeaponIndex > weapons.Count - 1)
        {
            currentWeaponIndex = 0;
        }

        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    void OnJump()
    {
        if (isGrounded)
        {
            verticalSpeed = jumpSpeed;
            isGrounded = false;
        }
    }
    public void OnDie()
    {
        GetComponent<Animator>().SetTrigger("Die");
        GameManager.Instance.PlayerDie();

    }
}
