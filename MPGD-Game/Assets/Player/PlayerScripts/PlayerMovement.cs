using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float lookSpeed = 2f;  // Mouse look speed
    public Transform playerCamera; // Reference to the camera

    private Vector2 movementInput; // Store movement input
    private Vector2 lookInput; // Store look input
    private float rotationY = 0f;  // Vertical rotation angle
    private float rotationX = 0f;  // Horizontal rotation angle

    void Update()
    {
        MovePlayer();
        LookAround();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>(); // Get movement input
        }
        else if (context.canceled)
        {
            movementInput = Vector2.zero; // Reset movement input
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>(); // Get look input
    }

    private void MovePlayer()
    {
        // Create a movement vector based on input
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        move = transform.TransformDirection(move); // Transform to world space

        // Move the player
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    private void LookAround()
    {
        // Adjust rotation angles based on mouse input
        rotationY -= lookInput.y * lookSpeed; // Invert Y-axis
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Limit up and down rotation
        rotationX += lookInput.x * lookSpeed; // Left and right rotation

        // Apply the rotation to the camera
        playerCamera.localEulerAngles = new Vector3(rotationY, rotationX, 0);
    }
}
