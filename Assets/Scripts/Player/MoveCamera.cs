using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps the playercamera aligned with the player.
/// </summary>
public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
