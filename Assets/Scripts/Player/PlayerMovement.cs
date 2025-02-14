using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
<<<<<<< Updated upstream

=======
    public float sprintSpeed;
    public float crouchSpeed;
    public bool sprinting;
    public bool crouching;
    public Vector2 inputLook;
    public Vector2 inputLookRaw;
    public Vector2 inputMove;
    public Vector3 moveDirection;


    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Physics Stuff")]
    Rigidbody rb;
>>>>>>> Stashed changes
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

<<<<<<< Updated upstream
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

=======
>>>>>>> Stashed changes
    [Header("Ground Check")]
    public float playerHeight;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask whatIsGround;
<<<<<<< Updated upstream
    bool grounded;

=======
    public bool grounded;

    [Header("Sprint Meter")]
    public float sprintDuration;
    public float currentSprintTime;
    public float regenSpeed;

    [Header("Input Stuff")]
>>>>>>> Stashed changes
    public Transform orientation;

    float horizontalInput;
    float verticalInput;
<<<<<<< Updated upstream

    Vector3 moveDirection;

    Rigidbody rb;
=======
    
    [Header("CamEffects")]
    private PlayerCamEffects playerCamEffects;
>>>>>>> Stashed changes


    private void Awake()
    {
        GetRefs();
    }
    private void Start()
    {
        
        rb.freezeRotation = true;
        readyToJump = true;
<<<<<<< Updated upstream
=======
        currentSprintTime = sprintDuration;
        
>>>>>>> Stashed changes
    }

    private void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
        MyInput();
<<<<<<< Updated upstream
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;

        }
        else
        {
            rb.drag = 0;
            ApplyFallMultiplier(1.75f);
            
        }

=======
>>>>>>> Stashed changes
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

<<<<<<< Updated upstream
=======
    private void GetRefs()
    {
        rb = GetComponent<Rigidbody>();
        playerCamEffects = GetComponentInChildren<PlayerCamEffects>();
    }

    private void MyInput()
    {   
        horizontalInput = inputMove.x;
        verticalInput = inputMove.y;

        if (Keyboard.current.spaceKey.isPressed && readyToJump && grounded)
        {
            readyToJump = false;
            jumpRequested = true;
        }
    }
    
    /// <summary>
    /// Handles the actual movement of the player by adding force to the player RigidBody. (called in FixedUpdate)
    /// </summary>
>>>>>>> Stashed changes
    private void MovePlayer()
    {
        //Calc Move Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        }
        else if (!grounded)
        {
               rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void ApplyFallMultiplier(float fallMultiplier)
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

<<<<<<< Updated upstream

=======
    /// <summary>
    /// Manages the playermovement and physics based operations. (called in FixedUpdate)
    /// </summary>
    private void ManageMovement()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
            ApplyFallMultiplier(1.75f);
        }

        UpdateSprintUI();
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        inputMove = value.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        inputLook = value.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        crouching = context.ReadValue<float>() > 0;
    }

    public bool IsCrouching()
    {
        return crouching;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        sprinting = context.ReadValue<float>() > 0;
    }

    public bool IsSprinting()
    {   
        return sprinting;
    }

    //Move this to its own player UI class soon
    public void UpdateSprintUI()
    {
        if (IsSprinting() && currentSprintTime > 0f)
        {
            currentSprintTime -= Time.deltaTime; // Deplete sprint meter when sprinting
        }
        else if (!IsSprinting() && currentSprintTime < sprintDuration)
        {
            currentSprintTime += Time.deltaTime * regenSpeed; // Regenerate sprint meter when not sprinting
        }
    }
>>>>>>> Stashed changes

}
