using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LightHouseDetectionRingForNAV : MonoBehaviour
{
    public float DetectionRing = 50f;
    public float TwrHealth = 100;//Health of the tower
    public float currentTwrHealth;
    public Slider TwrHealthSlider;
    private GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.FindWithTag("LightHouse");
        currentTwrHealth = TwrHealth;
        TwrHealthSlider.maxValue = TwrHealth;
        TwrHealthSlider.value = currentTwrHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRing);
    }

    public void OnTriggerStay(Collider other)
    {
       
            if (other.CompareTag("Enemy"))
            {
                new WaitForSeconds(10f);
                MainTowerTakeDamage(10f * Time.deltaTime);
                Debug.Log("The LightHouse is taking damage");
            }
       
    }
    public void MainTowerTakeDamage(float amount)
    {
        currentTwrHealth -= amount;
        TwrHealthSlider.value = currentTwrHealth;
        if (currentTwrHealth <= 0)
        {
            Destroy(go);
            new WaitForSeconds(5f);
            Time.timeScale = 0;
            SceneManager.LoadScene(5);
        }
    }
}
