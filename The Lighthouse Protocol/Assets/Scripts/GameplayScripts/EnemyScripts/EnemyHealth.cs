using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    
    public float Maxhealth = 50f;
    public float currentHealth;
    public Slider HealthSlider;
    public DomeControl domeDamage;
    public int moneyGained = 50;
    void Start()
    {
        currentHealth = Maxhealth;
        HealthSlider.maxValue = Maxhealth;
        HealthSlider.value = Maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        HealthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            EnemyDead();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dome"))
        {
            TakeDamage(domeDamage.DomeDamge);
            Debug.Log("Enemy is taking damage");
        }
        
    }
    public void EnemyDead()
    {
        GameManager.Instance.GainMoney(moneyGained);
        Destroy(gameObject);
    }
}
