using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeControl : MonoBehaviour
{
    [Header("Dome Variables")]
    public Transform domePos;
    public float Energy = 100f;
    public float MaxEnergy = 100f;
    public float MinRadius = 2f;
    public float MaxRadius = 10f;
    public float DomeDamge = 100f;
    public float Shrink = 50f;
    [Header("Object Reference")]
    public GameObject DomePrefab;
    private GameObject CurrentDome;

    private void Start()
    {
        domePos = Instantiate(DomePrefab, transform.position, Quaternion.identity).transform;
        domePos.localScale = Vector3.one * MaxRadius;
    }

    private void Update()
    {
        if (Energy>0)
        {
            float targetDomeSize = Mathf.Lerp(MinRadius, MaxRadius, Energy / 100f);
            domePos.localScale = Vector3.one * targetDomeSize;
            Debug.Log("Dome size: " + domePos.localScale);
        }
        else
        {
            domePos.localScale = Vector3.one * MinRadius;
        }
    }
   /* public void CreatedDome()
    {
        CurrentDome = Instantiate(DomePrefab, transform.position, Quaternion.identity, transform);
        CurrentDome.name = "Dome";
    }
    public void UpdateDomeSize()
    {
        float normalizedEnergy = Mathf.Clamp01(Energy / MaxEnergy);
        float domeScale = Mathf.Lerp(MinRadius, MaxRadius, normalizedEnergy);
        CurrentDome.transform.localScale = Vector3.one * domeScale;
    }*/

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHP = other.GetComponent<EnemyHealth>();
            if (enemyHP != null)
            {
                Debug.Log("Got enemy hp");
                enemyHP.TakeDamage(DomeDamge);
            }
            if (Energy >0)
            {
                Energy -= Shrink * Time.deltaTime;
                Debug.Log("Dome is shrinking");
            }
        }
    }
}
