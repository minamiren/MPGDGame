using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAttributes : MonoBehaviour
{
    public int maxHealth;
    //public int damage;

    public int maxFullBelly;
    public float HealthIncreseRate;
    public float HungerIncreseRate;
    public float currentHealth;
    public float currentHunger;




    public Image Health;
    public Image Hunger;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI HungerText;
    public TextMeshProUGUI gameState;

    // Start is called before the first frame update
    void Start()
    {
        HealthIncreseRate = 5f; //Increase Rate
        HungerIncreseRate = -1f; //decrese Rate

        maxHealth = 100;
        maxFullBelly = 100;
        currentHunger = maxFullBelly;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHunger += HungerIncreseRate * Time.deltaTime;
        Hunger.fillAmount = currentHunger / maxFullBelly;
        HungerText.text = "HUNGER: " + (int)currentHunger;

        currentHealth += currentHunger * 0.1f;
        Health.fillAmount = currentHealth / maxHealth;
        HealthText.text = "HEALTH: " + (int)currentHealth;

        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(currentHealth <= 0f)
        {
            currentHealth = 0f;
            gameState.text = "Game Over!";
        }

        if(currentHunger >= maxFullBelly)
        {
            currentHunger = maxFullBelly;
        }

        if (currentHunger <= 0f)
        {
            currentHunger = 0f;
            gameState.text = "Game Over!";
        }
    }
}
