using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PickupWeapon : MonoBehaviour
{
    private GameObject currentWeaponObject;
    public GameObject GunWeapon;
    public Transform WeaponHolderR;
    public Transform GunPosition;
    private bool gunPickedUp = false;
    public TextMeshProUGUI pickupText;

    private Vector3 initialPosition;
    private Transform initialParent;
    private bool initialIsKinematic;

    private void Start()
    {
        initialPosition = transform.position;
        initialParent = transform.parent;
        initialIsKinematic = GetComponent<Rigidbody>().isKinematic;

        if (pickupText != null)
        {
            pickupText.enabled = true;  // Initially visual the pickup text
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (!gunPickedUp && other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            Animator playerAnimation = other.GetComponentInChildren<Animator>();

            if (playerMovement != null && playerAnimation != null)
            {
                TriggerGunAttackStatus(playerAnimation, other.transform);
                playerMovement.SetGunPossession(true);  // Notify player has the gun
                gunPickedUp = true;
                //gameObject.GetComponent<Collider>().enabled = false;
                pickupText.enabled = false;//hide the pickup text, as its pickedup.
            }
        }
    }

    /*private void TriggerGunAttackStatus(Animator animation)
    {
        animation.SetBool("IsGun", true);
        Debug.Log("Gun Attack Status active!");

        // Instantiate only if gun is not already equipped
        if (currentWeaponObject == null)
        {
            currentWeaponObject = Instantiate(GunWeapon, WeaponHolderR);
            //currentWeaponObject.transform.localPosition = Vector3.zero;
            //currentWeaponObject.transform.localRotation = Quaternion.identity;
        }
    }
    */
    private void TriggerGunAttackStatus(Animator animation, Transform player)
    {
        animation.SetBool("IsGun", true);
        Debug.Log("Gun Attack Status active!");

        // Parent the existing gun object to WeaponHolderR
        transform.SetParent(WeaponHolderR);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //transform.localPosition = WeaponHolderR.localPosition;
       // transform.localRotation = WeaponHolderR.localRotation;

        GetComponent<Collider>().enabled = false;  // Disable collider to avoid retriggering
        GetComponent<Rigidbody>().isKinematic = true;  // Disable physics to prevent falling

        /*
        Animator gunAnimator = GetComponentInChildren<Animator>();
        if(gunAnimator == null)
        {
            Debug.LogError("GUnAnimator not found on the gun!");
        }
        else
        {
            player.GetComponent<PlayerMovement>().SetGunAnimator(gunAnimator);
        }
        */
    }
    public void ResetWeapon()
    {
        // Restore the weapon to its initial parent and position
        transform.SetParent(initialParent);
        transform.position = initialPosition;

        // Reset the Rigidbody and Collider
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = initialIsKinematic;
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        gunPickedUp = false;

        // Re-enable the pickup text UI if available
        if (pickupText != null)
        {
            pickupText.enabled = true;
        }
        // Find the Player object and reset animation states
        PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            Animator playerAnimator = player.GetComponentInChildren<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("IsGun", false);
            }
        }
    }
}
