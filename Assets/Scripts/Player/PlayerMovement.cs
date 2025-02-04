using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public bool sprinting;
    public bool crouching;


    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

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

        HandleMovement();
        HandleAnimations();

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

    private void MovePlayer()
    {
        //Calc Move Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {   
            if (Input.GetKey(sprintKey) && currentSprintTime > 0f)
            {   
                crouching = false;
                sprinting = true;
                rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force);
            }
            else if (Input.GetKey(crouchKey))
            {   
                sprinting = false;
                crouching = true;
                rb.AddForce(moveDirection.normalized * crouchSpeed * 10f, ForceMode.Force);
            }
            else
            {   
                sprinting = false;
                crouching = false;
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
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
        //sawedOffAnimator = transform.Find("SawedOff").GetComponent<Animator>();
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
        else if (moveDirection != Vector3.zero && !Input.GetKey(sprintKey))
        {
            animator.SetFloat("Speed", 0.5f, 0.05f, Time.deltaTime);
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(sprintKey) && currentSprintTime > 0)
        {
            animator.SetFloat("Speed", 1f, 0.05f, Time.deltaTime);
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(sprintKey) && currentSprintTime <= 0)
        {
            animator.SetFloat("Speed", 0.5f, 0.05f, Time.deltaTime);
        }
    }

    private void HandleMovement()
    {
        bool wasGrounded = grounded;
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        // Check if the player was in the air and has now landed
        if (wasInAir && grounded)
        {
            //playerCamEffects.TriggerLandingEffect();
            //wasInAir = false;
        }
        if (!grounded && !wasInAir)
        {
            wasInAir = true;  // Player has left the ground
        }

        

        MyInput();
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

        if (Input.GetKey(sprintKey) && currentSprintTime > 0f)
        {
            currentSprintTime -= Time.deltaTime; // Deplete sprint meter when sprinting
        }
        else if (!Input.GetKey(sprintKey) && currentSprintTime < sprintDuration)
        {
            currentSprintTime += Time.deltaTime * regenSpeed; // Regenerate sprint meter when not sprinting
        }
    }


}
