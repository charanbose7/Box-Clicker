using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeShooter : MonoBehaviour
{
    private Camera playerCamera;

    private void Awake()
    {
        // Find the camera attached to the player
        playerCamera = GetComponentInChildren<Camera>();

        if (playerCamera == null)
        {
            Debug.LogError("Player camera not found.");
        }
    }

    void Update()
    {
        // Check for player input to shoot
        if (Input.GetMouseButtonDown(0))
        {
            ShootCube();
        }
    }

    void ShootCube()
    {
        // Create a ray from the center of the camera
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // Perform a raycast to check for hits
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the hit object has the "Cube" tag
            if (hit.collider.CompareTag("Cube"))
            {
                // Destroy the cube
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
