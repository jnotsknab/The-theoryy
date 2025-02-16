using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimationHandler : MonoBehaviour
{

    [Header("Animations")]
    public Animator armAnimator;
    public Animator sawedOffAnimator;
    public Animator timeFluxAnimator;

    [Header("Movement Reference")]
    public PlayerMovement playerMovement;
    private NewItemPickupHandler newItemPickupHandler;

    private int speedHash;

    private void Awake()
    {
        speedHash = Animator.StringToHash("Speed");
        newItemPickupHandler = this.GetComponent<NewItemPickupHandler>();
    }

    void Update()
    {
        HandleArmMovementAnim();
        HandleItemMovementAnim();
    }


    /// <summary>
    /// Updates item animator's blendtrees for movement, i.e. idle, walk, sprint.
    /// </summary>
    private void HandleItemMovementAnim()
    {
        if (newItemPickupHandler.currentItemIDGlobal == 0)
        {
            UpdateMovementBlendtree(sawedOffAnimator);
        }
        if (newItemPickupHandler.currentItemIDGlobal == 1)
        {
            UpdateMovementBlendtree(timeFluxAnimator);
        }

    }

    /// <summary>
    /// Updates the arm animators blendtree for movement, i.e. idle, walk, sprint.
    /// </summary>
    private void HandleArmMovementAnim()
    {
        UpdateMovementBlendtree(armAnimator);
    }


    private void UpdateMovementBlendtree(Animator animator)
    {
        if (playerMovement.moveDirection == Vector3.zero)
        {
            animator.SetFloat(speedHash, 0f, 0.05f, Time.deltaTime);
        }
        else if (playerMovement.moveDirection != Vector3.zero && !playerMovement.IsSprinting())
        {
            animator.SetFloat(speedHash, 0.5f, 0.05f, Time.deltaTime);
        }
        else if (playerMovement.moveDirection != Vector3.zero && playerMovement.IsSprinting() && playerMovement.currentSprintTime > 0)
        {
            animator.SetFloat(speedHash, 1f, 0.05f, Time.deltaTime);
        }
        else if (playerMovement.moveDirection != Vector3.zero && playerMovement.IsSprinting() && playerMovement.currentSprintTime <= 0)
        {
            animator.SetFloat(speedHash, 0.5f, 0.05f, Time.deltaTime);
        }
    }
}
