using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    public float twrRange = 10f;//range till tower detects enemy
    public float fireRate = 1f;//rate of fire for the tower
    public GameObject GatlingProjectile;//prefab of the projectile for the specific tower
    public Transform shootPoint;//point where the projectile is fired from 
    //public LayerMask enemyLayer; // only detects objects labeled as enemy
    public string enemyTag = "Enemy";
    private Transform enemyTarget;
    private float cooldown = 0f;
    private int upgradeLevel = 0;
    private Dictionary<int, Dictionary<string, int>> upgradeCost = new Dictionary<int, Dictionary<string, int>>();
    void Start()
    {
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
    }
    
    public Dictionary<string, int> GetUpgradeCost()
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
        if (InventoryManager.HasRequiredResources(cost))
        {
            InventoryManager.UseResources(cost);
            ConfirmUpgrade();
            return true;
        }
        return false;
    }
    private void ConfirmUpgrade()
    {
        upgradeLevel++;
        twrRange += 20f;
        fireRate += 20f;
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
                    Debug.Log("Upgraded tower " + tower.name);
                }
                else
                {
                    Debug.Log("not enough resources to upgrade  " + tower.name);
                }
            }
        }
    }
}
