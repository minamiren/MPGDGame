using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public float speed = 5.0f;
   public Transform cameraTransform;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock rotation on the Rigidbody to prevent any unintended rotation
        rb.freezeRotation = true;
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        // Get the camera's forward and right vectors, with y axis set to 0 to ensure horizontal movement
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

       // forward.y = 0;
       // right.y = 0;

      //  forward.Normalize();
       // right.Normalize();

        // Calculate movement direction
      Vector3 movement = forward * moveValue.y + right * moveValue.x;
        


        // Set the player's velocity directly (instead of using AddForce)
        rb.velocity = movement * speed * Time.fixedDeltaTime + new Vector3(0, rb.velocity.y, 0); // Keep the Y velocity for gravity
    }

}
