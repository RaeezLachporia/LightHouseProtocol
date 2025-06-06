using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerShooting : MonoBehaviour
{
    public float twrRange = 10f;//range till tower detects enemy
    public float fireRate = 1f;//rate of fire for the tower
    public float TwrHealth = 100;//Health of the tower
    public float currentTwrHealth;
    public Slider TwrHealthSlider;
    public GameObject GatlingProjectile;//prefab of the projectile for the specific tower
    public Transform shootPoint;//point where the projectile is fired from 
    //public LayerMask enemyLayer; // only detects objects labeled as enemy
    public string enemyTag = "Enemy";
    private Transform enemyTarget;
    private float cooldown = 0f;
    private int upgradeLevel = 0;
    private static List<TowerShooting> upgradableTowers = new List<TowerShooting>();
    private static int UgrdIndex = 0;
    private Dictionary<int, Dictionary<string, int>> upgradeCost = new Dictionary<int, Dictionary<string, int>>();
    void Start()
    {
        currentTwrHealth = TwrHealth; ;
        TwrHealthSlider.maxValue = TwrHealth;
        TwrHealthSlider.value = currentTwrHealth;
        upgradableTowers.Add(this);
        UpgradeCosts();
    }

    // Update is called once per frame
    void Update()
    {
        enemyTarget = detectClosestEnemy();

        if (enemyTarget != null)
        {
            RotateToTarget(enemyTarget);
            if (cooldown<=0)
            {
                Shoot();
                cooldown = 1f/ fireRate;
            }
        }
        cooldown -= Time.deltaTime;
        
    }

    private Transform detectClosestEnemy()
    {
        GameObject[] enemiesInRange = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= twrRange) // Check if within range
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

    private void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (shootPoint != null)
        {
            shootPoint.LookAt(target.position);
        }
    }

    private void Shoot()
    {
        if (GatlingProjectile == null || shootPoint == null) return;

        GameObject bullet = Instantiate(GatlingProjectile, shootPoint.position, shootPoint.rotation);
        Projectile projectilescript = bullet.GetComponent < Projectile >();
        if (projectilescript != null)
        {
            projectilescript.SetTarget(enemyTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, twrRange);
    }

    private void UpgradeCosts()
    {
        upgradeCost[0] = new Dictionary<string, int> { { "Wood", 10 }, { "Metal", 5 } };
        upgradeCost[1] = new Dictionary<string, int> { { "Wood", 20 }, { "Metal", 10 } };
        upgradeCost[2] = new Dictionary<string, int> { { "Wood", 40 }, { "Metal", 20 } };
    }
    
    public Dictionary<string, int> GetupgradeCost()
    {
        if (upgradeCost.ContainsKey(upgradeLevel))
        {
            return upgradeCost[upgradeLevel];
        }
        return null;
    }

    public bool UpgradeTower()
    {
        if (!upgradeCost.ContainsKey(upgradeLevel)) return false;
        Dictionary<string, int> cost = upgradeCost[upgradeLevel];
        //Debug.Log("Current Inventory: " + InventoryManager);
        if (InventoryManager.HasRequiredResources(cost))
        {
            InventoryManager.UseResources(cost);
            ConfirmUpgrade();
            InventoryManager.Instance.UpdateInventoryUI();
            return true;
        }
        return false;
    }
    public static void UpgradeOneTower()
    {
        if (upgradableTowers.Count==0)
        {
            Debug.Log("No towers to Upgrade");
        }
        int Looped = 0;
        bool isUpgraded = false;
        while (Looped < upgradableTowers.Count && !isUpgraded)
        {
            if (UgrdIndex >= upgradableTowers.Count)
            {
                UgrdIndex = 0;
            }

            TowerShooting towers = upgradableTowers[UgrdIndex];
            UgrdIndex++;
            Looped++;

            
                if (towers.UpgradeTower())
                {
                    Debug.Log("Tower upgraded " + towers.name);
                    isUpgraded = true;
                }
                else
                {
                    Debug.Log("Not Enough Resources for " + towers.name);
                }
            
        }
        if (!isUpgraded)
        {
            Debug.Log("No towers upgraded");
        }
    }
    private void ConfirmUpgrade()
    {
        upgradeLevel++;
        twrRange += 20f;
        fireRate += 20f;
        TwrHealth += 100f;
        currentTwrHealth = TwrHealth;
        TwrHealthSlider.maxValue = TwrHealth;
        TwrHealthSlider.value = currentTwrHealth;
        Debug.Log("Tower upgrades" + upgradeLevel);
    }

    public void UpgradeGatlingTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            TowerShooting towerUpgradeScript = tower.GetComponent<TowerShooting>();
            if (towerUpgradeScript != null)
            {
                if (towerUpgradeScript.UpgradeTower())
                {
                    ConfirmUpgrade();
                    Debug.Log("Upgraded tower " + tower.name);
                }
                else
                {
                    Debug.Log("not enough resources to upgrade  " + tower.name);
                }
            }
        }
    }
    public void TowerTakeDamage(float amount)
    {
        currentTwrHealth -= amount;
        TwrHealthSlider.value = currentTwrHealth;
        if (currentTwrHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            new WaitForSeconds(10f);
            TowerTakeDamage(10f * Time.deltaTime);
            Debug.Log("Tower is taking damage");
        }
    }
}
