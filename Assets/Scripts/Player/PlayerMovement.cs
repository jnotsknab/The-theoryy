
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public bool sprinting;
    public bool crouching;
    public Vector2 inputLook;
    public Vector2 inputLookRaw;
    public Vector2 inputMove;

    //Physics stuff
    public float groundDrag;
    public float airDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    private bool jumpRequested = false;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Sprint Meter")]
    public float sprintDuration;
    public float currentSprintTime;
    public float regenSpeed;

    [Header("Animations")]
    private Animator armAnimator;
    public Animator sawedOffAnimator;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    public Vector3 moveDirection;
    Rigidbody rb;

    [Header("CamEffects")]
    private PlayerCamEffects playerCamEffects;
    private bool wasInAir = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        currentSprintTime = sprintDuration;
        GetRefs();
    }

    private void Update()
    {
        MyInput();
        HandleAnimations();

    }

    private void FixedUpdate()
    {
        ManageMovement();
        MovePlayer();

        // Handle jump in FixedUpdate for consistent physics.
        if (jumpRequested)
        {
            Jump();
            jumpRequested = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
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
    private void MovePlayer()
    {
        //Calc Move Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {   
            if (IsSprinting() && currentSprintTime > 0f)
            {   

                rb.AddForce(moveDirection * sprintSpeed * 10f, ForceMode.Force);
            }
            else if (IsCrouching())
            {   

                rb.AddForce(moveDirection * crouchSpeed * 10f, ForceMode.Force);
            }
            else
            {   

                rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
            }
            

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

    /// <summary>
    /// Makes the player fall faster.
    /// </summary>
    /// <param name="fallMultiplier"></param>
    private void ApplyFallMultiplier(float fallMultiplier)
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void GetRefs()
    {
        armAnimator = GetComponentInChildren<Animator>();
        playerCamEffects = GetComponentInChildren<PlayerCamEffects>();
    }


    private void HandleAnimations()
    {
        UpdateAnimator(armAnimator);
        UpdateAnimator(sawedOffAnimator);
    }
    private void UpdateAnimator(Animator animator)
    {
        if (moveDirection == Vector3.zero)
        {
            animator.SetFloat("Speed", 0f, 0.05f, Time.deltaTime);
        }
        else if (moveDirection != Vector3.zero && !IsSprinting())
        {
            animator.SetFloat("Speed", 0.5f, 0.05f, Time.deltaTime);
        }
        else if (moveDirection != Vector3.zero && IsSprinting() && currentSprintTime > 0)
        {
            animator.SetFloat("Speed", 1f, 0.05f, Time.deltaTime);
        }
        else if (moveDirection != Vector3.zero && IsSprinting() && currentSprintTime <= 0)
        {
            animator.SetFloat("Speed", 0.5f, 0.05f, Time.deltaTime);
        }
    }

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



}
