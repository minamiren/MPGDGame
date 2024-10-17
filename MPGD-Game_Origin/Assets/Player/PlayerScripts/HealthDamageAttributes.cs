using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDamageAttributes : MonoBehaviour
{


    public int health;
    public int damage;
   
    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<HealthDamageAttributes>();
        if(atm != null)
        {
            atm.TakeDamage(damage);

        }
    }
}
