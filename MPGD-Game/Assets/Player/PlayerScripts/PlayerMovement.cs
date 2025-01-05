using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float sprintSpeed = 10f; //Sprint speed
    private bool isSprinting = false; //Sprint state


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

    //=====Crosshair=======
    public Texture2D crosshairTexture; // Crosshair icon texture
    public Vector2 crosshairHotspot = new Vector2(16, 16); // The center of the crosshair texture
    public Image crosshairImage; // Reference to the UI Image for crosshair

    //=====Animation======
    public Animator animator;
    private bool hasGun = false;
    public Animator gunAnimator;

    //====Sound=====
    private float footstepTimer = 0f;
    private float currentFootstepInterval;
    private float walkFootstepInterval = 0.5f;  // Time between steps while walking
    private float runFootstepInterval = 0.3f;   // Time between steps while running
    public AudioSource footstepsSound;

    public AudioClip terrainFootstepClip;  // Footstep sound for terrain
    public AudioClip caveFootstepClip;     // Footstep sound for cave
    private AudioClip currentFootstepClip; // Currently playing footstep sound

    //====Cursor Lock & Unlock status=======
    public enum GameState
    {
        StartScene,
        PlayScene,
        DialogueScene
    };
    public GameState currentState = GameState.StartScene;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // Prevent unwanted rotation
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);//keep the player's world coordinate, rotateY in 90 degree.
        //UnityEngine.Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window

        animator = GetComponentInChildren<Animator>(); //Set reference for animator
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the child object!");
        }

        // Hide crosshair by default
        if (crosshairImage != null)
        {
            crosshairImage.enabled = false;
        }

        updateCursorState();
    }

    public void updateCursorState()
    {
        switch (currentState)
        {
            case GameState.StartScene:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true; // Show cursor in StartScene
                break;

            case GameState.PlayScene:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; // Hide cursor during gameplay
                break;

            case GameState.DialogueScene:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;
        }

    }
    
    //Set the StartButton to connect here, which change the GameState after clicking:
    public void GameStart()
    {
        SoundManager.PlaySound(SoundType.GAMESTART);
       
        currentState = GameState.PlayScene;
       
        
        updateCursorState();
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            updateCursorState();  // Reapply the cursor state when the game regains focus
        }
    }


    void Update()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
        if (!dialogue)
        {
            MovePlayer();
            LookAround();
            updateAnimation();
            UpdateFootstepSound();
        }

    }


    private void TriggerJump()
    {
        animator.SetTrigger("Jump");
        Debug.Log("Jump active!");
    }

    public void SetGunPossession(bool status)
    {
        hasGun = status;
        // Show or hide the crosshair based on whether the player has the gun
        if (crosshairImage != null)
        {
            crosshairImage.enabled = hasGun;
        }
    }

    public void ResetPlayer()
    {
        SetGunPossession(false);
    }

    // This method will be called to set the gunAnimator after pickup
    /*
    public void SetGunAnimator(Animator gunAnimator)
    {
        this.gunAnimator = gunAnimator;
    }
    */

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>(); // Get movement input
            footstepsSound.enabled = true;
        }
        else if (context.canceled)
        {
            movementInput = Vector2.zero; // Reset movement input
            footstepsSound.enabled = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
            Debug.Log("Sprint started!");
        }
        else if (context.canceled)
        {
            isSprinting = false;
            Debug.Log("Sprint stopped!");
        }
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        // Only allow jump if grounded and cooldown time has passed
        if (context.performed && isGrounded && Time.time >= lastJumpTime + jumpCooldown)
        {
            TriggerJump();

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

        // Check what type of surface the player is on
        if (collision.gameObject.CompareTag("Terrain"))
        {
            currentFootstepClip = terrainFootstepClip;  // Set terrain footstep sound
        }
        else if (collision.gameObject.CompareTag("Cave"))
        {
            currentFootstepClip = caveFootstepClip;     // Set cave footstep sound
        }

        // Set footstep sound clip to the audio source
        footstepsSound.clip = currentFootstepClip;
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>(); // Get look input
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (PauseMenu.isPaused)
            return;
        if (!dialogue && context.performed && Time.time >= lastFireTime + fireCooldown && hasGun)
        {
            Debug.Log("Fire action triggered!");

            animator.SetTrigger("GunShoot");  // Trigger gun shoot animation
            SoundManager.PlaySound(SoundType.GUNSHOOT);
            /*
             if (gunAnimator != null)
             {
                 gunAnimator.SetTrigger("Shoot");
             }
            */

            /*
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
            */

            Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(cameraRay, out RaycastHit hit, 100f))
            {
                Debug.Log(hit.transform.name);


                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(); // Damage only this enemy
                }
                // Destroy(gameObject); // Destroy the projectile after hitting an enemy

                lastFireTime = Time.time;
            }
        }
    }

    private void updateAnimation()
    {
        if (movementInput == Vector2.zero)
        {
            animator.SetFloat("Velocity", 0f, 0.2f, Time.deltaTime);
        }
        else if (movementInput != Vector2.zero && !isSprinting)
        {
            animator.SetFloat("Velocity", 0.5f, 0.2f, Time.deltaTime);
        }
        else if (movementInput != Vector2.zero && isSprinting)
        {
            animator.SetFloat("Velocity", 1f, 0.2f, Time.deltaTime);
        }

    }
    private void UpdateFootstepSound()
    {
        if (movementInput != Vector2.zero) // Player is moving
        {
            if (!footstepsSound.isPlaying)
            {
                footstepsSound.loop = true;  // Enable looping
                footstepsSound.Play();       // Start playing sound continuously
            }
            // Accumulate time for footstep sound
            footstepTimer += Time.deltaTime;

            // Set footstep interval based on whether sprinting or walking
            currentFootstepInterval = isSprinting ? runFootstepInterval : walkFootstepInterval;

            // Play footstep sound when it's time based on interval
            if (footstepTimer >= currentFootstepInterval)
            {
                footstepsSound.Play();  // Play footstep sound
                footstepTimer = 0f;     // Reset the timer
            }
        }
        else // Player is not moving
        {
            if (footstepsSound.isPlaying)
            {
                footstepsSound.loop = false; // Stop looping when not moving
                footstepsSound.Stop();       // Stop the sound
            }
            footstepTimer = 0f;  // Reset the footstep timer when not moving
        }
    }

    private void MovePlayer()
    {
        //Debug.Log("Movement Input: " + movementInput);  // Debug the input

        //===Check if sprint or walk======
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;
        //Debug.Log($"Current Speed: {currentSpeed}");

        // Create a movement vector based on input
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        move = transform.TransformDirection(move); // Transform to world space

        // Move the player
        // transform.position += move * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move * currentSpeed * Time.deltaTime);
        //Debug.Log("Move Vector: " + move);
    }

    private void LookAround()
    {
        if (PauseMenu.isPaused) 
            return;
        // Adjust rotation angles based on mouse input
        if (!inventory.activeSelf)
        {
            rotationY -= lookInput.y * lookSpeed; // Invert Y-axis
            rotationY = Mathf.Clamp(rotationY, -20f, 20f); // Limit up and down rotation
            rotationX += lookInput.x * lookSpeed; // Left and right rotation
        }

        // Apply the rotation to the camera
        playerCamera.localEulerAngles = new Vector3(rotationY, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotationX, 0);
    }

    public void SetSensitivity(float sensitivity)
    {
        lookSpeed = sensitivity;
    }
}
