using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Serialization;

public class PlayerStates : MonoBehaviour
{
    public int maxHealth;
    //public int damage;

    public int maxFullBelly;
    public float HealthIncreseRate;
    public float HungerIncreseRate;
    public float currentHealth;
    public float currentHunger;

    public Slider HealthSlider;
    public Slider HungerySlider;

    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI HungerText;
    public TextMeshProUGUI gameState;

    public GameObject startPanel;

    // Start is called before the first frame update
    void Start()
    {
        //HealthIncreseRate = 5; //Increase Rate
        HungerIncreseRate = -0.5f; //decrese Rate

        maxHealth = 100;
        maxFullBelly = 100;

        HealthSlider.value = maxHealth;
        HungerySlider.value = maxFullBelly;

        currentHunger = maxFullBelly;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //HealthText.text = "HEALTH: " + (int)currentHealth;
        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    public void UpdateHealthUI()
    {
        HealthText.text = "HEALTH: " + (int)currentHealth;
        HealthSlider.value = currentHealth;


    }


//============Hunger with Food==============
    public void FillBelly(int fill)
    {
        currentHunger += fill;
       // currentHunger = Mathf.Clamp(currentHunger, 0, maxFullBelly);
       // HungerText.text = "HUNGER: " + (int)currentHunger;
    }

    public void HungerByTime()
    {
        currentHunger += HungerIncreseRate * Time.deltaTime;
        //currentHunger = Mathf.Clamp(currentHunger, 0, maxFullBelly);

    }

    public void UpdateHunegrUI()
    {
        HungerText.text = "HUNGER: " + (int)currentHunger;
        HungerySlider.value = currentHunger;
        if (currentHunger <= 0f)
        {
            PlayerDie();
        }

    }

    public void PlayerDie()
    {
        gameState.text = "Game Over!";
    }
    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentHunger = Mathf.Clamp(currentHunger, 0, maxFullBelly);

        UpdateHealthUI();
        if(!startPanel.activeSelf && !PlayerMovement.dialogue)
        {
            HungerByTime();
            UpdateHunegrUI();
        }

    }
}
