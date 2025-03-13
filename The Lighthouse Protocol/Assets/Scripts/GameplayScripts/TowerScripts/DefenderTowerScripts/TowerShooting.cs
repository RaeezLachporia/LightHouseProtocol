using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    public float twrRange = 10f;//range till tower detects enemy
    public float fireRate = 1f;//rate of fire for the tower
    public GameObject GatlingProjectile;//prefab of the projectile for the specific tower
    public Transform shootPoint;//point where the projectile is fired from 
    public LayerMask enemyLayer; // only detects objects labeled as enemy
    private Transform enemyTarget;
    private float cooldown = 0f;
    void Start()
    {
        
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
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, twrRange, enemyLayer);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
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
    }

    private void Shoot()
    {
        if (GatlingProjectile == null || shootPoint == null) return;

        GameObject bullet = Instantiate(GatlingProjectile, shootPoint.position, shootPoint.rotation);
       
    }
}
