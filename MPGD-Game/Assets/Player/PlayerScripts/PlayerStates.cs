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




    public Image HealthBar;
    public Image HungerBar;
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        HealthText.text = "HEALTH: " + (int)currentHealth;
        if (currentHealth < 0)
        {
            PlayerDie();
        }
    }
    public void PlayerDie()
    {
        gameState.text = "Game Over!";
    }

    void HealthBarUIUPdate()
    {
        HealthText.text = "HEALTH: " + (int)currentHealth;
        HealthBar.fillAmount = (float)currentHealth / maxHealth;


    }


//============Hunger with Food==============
    public void FillBelly(int fill)
    {
        currentHunger += fill;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxFullBelly);
        HungerText.text = "HUNGER: " + (int)currentHunger;
    }

    public void hungerByTime()
    {
        currentHunger += HungerIncreseRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxFullBelly);

    }

    public void hunegrBarUpdate()
    {
        HungerBar.fillAmount = currentHunger / maxFullBelly;
        HungerText.text = "HUNGER: " + (int)currentHunger;
    }
    // Update is called once per frame
    void Update()
    {
        HealthBarUIUPdate();
        hungerByTime();
        hunegrBarUpdate();

        if (currentHunger <= 0f)
        {
            currentHunger = 0f;
            gameState.text = "Game Over!";
        }

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            gameState.text = "Game Over!";
        }
    }
}
