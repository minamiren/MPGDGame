using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float lookSpeed = 0.1f;  // Mouse look speed
    public float jumpSpeed = 5f; //Jump speed
    public float lastJumpTime = 0f;
    public float jumpCooldown = 0.2f;
    public float groundCheckDistance = 0.1f;
    public bool isGrounded = true;


    public float lastFireTime = 0f;//Attacking
    public float fireCooldown = 0.5f;
    public GameObject stickPrefab;
    public Transform shootingPoint;

    public Transform playerCamera; // Reference to the camera
    public GameObject inventory;

    private Vector2 movementInput; // Store movement input
    private Vector2 lookInput; // Store look input
    private float rotationY = 0f;  // Vertical rotation angle
    private float rotationX = 0f;  // Horizontal rotation angle
    public static bool dialogue = false;

    private Rigidbody rb;

    public Texture2D crosshairTexture; // Crosshair icon texture
    public Vector2 crosshairHotspot = new Vector2(16, 16); // The center of the crosshair texture


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // Prevent unwanted rotation
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);//keep the player's world coordinate, rotateY in 90 degree.
        UnityEngine.Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window

   
    }

    void Update()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
        if (!dialogue)
        {
            MovePlayer();
            LookAround();
        }
        

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
    public void OnJump(InputAction.CallbackContext context)
    {
        // Only allow jump if grounded and cooldown time has passed
        if (context.performed && isGrounded && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse); // Apply jump force
            Debug.Log("Jump pressed");

            lastJumpTime = Time.time; // Update the last jump time
            isGrounded = false; // Set to false as the player is now in the air
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain" || collision.gameObject.tag == "Cave")
        {
            isGrounded = true;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>(); // Get look input
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!dialogue && context.performed && Time.time >= lastFireTime + fireCooldown)
        {
            Debug.Log("Fire action triggered!");
           // Get the mouse position and create a ray from the camera to that point
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray cameraRay = Camera.main.ScreenPointToRay(mousePosition);

            // Calculate the direction from the shooting point to the mouse's world position
            Vector3 targetPoint = cameraRay.GetPoint(1000f); // Get a point far away along the ray
            Vector3 direction = (targetPoint - shootingPoint.position).normalized;
            // Perform a raycast from shootingPoint in the calculated direction
            if (Physics.Raycast(shootingPoint.position, direction, out RaycastHit hit))
            {

                GameObject stickInstance = Instantiate(stickPrefab, shootingPoint.position, shootingPoint.rotation);
                StickThrow stickThrowScript = stickInstance.GetComponent<StickThrow>();
                if(stickThrowScript != null)
                {
                    stickThrowScript.SetTartget(hit.point);
                }

                lastFireTime = Time.time;
            }
            // Instantiate(stickPrefab, shootingPoint.position, Quaternion.identity);
            // lastFireTime = Time.time;

        }
    }

    private void MovePlayer()
    {
        //Debug.Log("Movement Input: " + movementInput);  // Debug the input

        // Create a movement vector based on input
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        move = transform.TransformDirection(move); // Transform to world space

        // Move the player
        // transform.position += move * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);
        //Debug.Log("Move Vector: " + move);
    }

    private void LookAround()
    {
        // Adjust rotation angles based on mouse input
        if(!inventory.activeSelf)
        {
            rotationY -= lookInput.y * lookSpeed; // Invert Y-axis
            rotationY = Mathf.Clamp(rotationY, -20f, 20f); // Limit up and down rotation
            rotationX += lookInput.x * lookSpeed; // Left and right rotation
        }

        // Apply the rotation to the camera
        playerCamera.localEulerAngles = new Vector3(rotationY, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotationX, 0);
    }
}
