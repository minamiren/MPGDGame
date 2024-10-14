using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 moveValue;
    public float speed;

    private Vector3 oldPosition;
    private Vector3 velocity;
    private float scalarSpeed;

    public Inventory inventory;

    void Start()
    {
        oldPosition = transform.position;

    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            inventory.AddItem(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}