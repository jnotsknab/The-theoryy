using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the players animation and item animations before and after interacting with items.
/// </summary>
public class PlayerAnimationHandler : MonoBehaviour
{

    [Header("Animations")]
    public Animator armAnimator;
    public Animator sawedOffAnimator;
    public Animator timeFluxAnimator;
    public Animator inhalerAnimator;

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
    /// Updates item animator's blendtrees for movement, i.e. idle, walk, sprint based on the Item ID of the currently held item.
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
        if (newItemPickupHandler.currentItemIDGlobal == 2)
        {
            UpdateMovementBlendtree(inhalerAnimator);
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