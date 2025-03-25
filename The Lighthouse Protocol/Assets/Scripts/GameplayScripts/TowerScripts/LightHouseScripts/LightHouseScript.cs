using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHouseScript : MonoBehaviour
{
   
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public float minDomeRadius = 2f;
    public float maxDomeRadius = 10f;
    public GameObject Dome;

    public float damagePerSecond = 10f;    // Damage dealt to enemies per second
    public float energyDrainPerSecond = 5f; // Energy lost per second when an enemy touches the dome

    private SphereCollider domeCollider;

    void Start()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        domeCollider = GetComponent<SphereCollider>();

        if (domeCollider == null)
        {
            domeCollider = gameObject.AddComponent<SphereCollider>();
        }

        domeCollider.isTrigger = true;

        // Set initial dome size
        UpdateDomeSize(true);
    }

    void Update()
    {
        UpdateDomeSize();
    }

    private void UpdateDomeSize(bool forceFullSize = false)
    {
        float normalizedEnergy = Mathf.Clamp(currentEnergy / maxEnergy, 0f, 1f);
        float newRadius = forceFullSize ? maxDomeRadius : Mathf.Lerp(minDomeRadius, maxDomeRadius, normalizedEnergy);

        // Update collider and dome size
        domeCollider.radius = newRadius;
        if (Dome != null)
        {
            float scaleFactor = newRadius * 2f;
            Dome.transform.localScale =new Vector3(scaleFactor,scaleFactor,scaleFactor);
            Dome.transform.position = transform.position;
        }

        // If energy is 0, disable the dome
        if (currentEnergy <= 0)
        {
            domeCollider.enabled = false;
            if (Dome != null) Dome.SetActive(false);
        }
        else
        {
            domeCollider.enabled = true;
            if (Dome != null) Dome.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       /* if (other.CompareTag("Enemy") && currentEnergy > 0)
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerSecond);
            }

            // Drain energy
            currentEnergy -= energyDrainPerSecond;
            currentEnergy = Mathf.Max(0, currentEnergy);
        }*/
    }

    private void OnTriggerStay(Collider other)
    {
       /* if (other.CompareTag("Enemy") && currentEnergy > 0)
        {
            // Prevent the enemy from moving forward by pushing it back
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                enemyRb.AddForce(pushDirection * 10f, ForceMode.Force);
            }
        }*/
    }
}



