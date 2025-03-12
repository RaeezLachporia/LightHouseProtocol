using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHouseScript : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float domeRadius = 5f;
    public float maxDomeRadius = 10f;
    public GameObject Dome;

    private SphereCollider domeCollider;
    void Start()
    {
        currentEnergy = maxEnergy;
        domeCollider = GetComponent<SphereCollider>();

        if (domeCollider == null)
        {
            domeCollider = gameObject.AddComponent<SphereCollider>();
            domeCollider.isTrigger = true;
        }
        UpdateDomeSize();
    }

    
    void Update()
    {
        UpdateDomeSize();
    }

    private void UpdateDomeSize() //Handles updating the dome size for the light house
    {
        float normalizedEnergy = currentEnergy / maxEnergy;
        float newRaidus = Mathf.Lerp(domeRadius, maxDomeRadius, normalizedEnergy);

        Debug.Log("Udpating the dome size: " + newRaidus); 
        domeCollider.radius = newRaidus;

        if (Dome != null)
        {
            Dome.transform.localScale = Vector3.one * newRaidus * 2;
            Debug.Log("Dome size updated ");
        }

        
    }

    
}
