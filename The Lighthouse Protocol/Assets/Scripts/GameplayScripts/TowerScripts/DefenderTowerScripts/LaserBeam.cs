using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{

    public float range = 10f;
    public float dps = 5f;
    public Transform shootPoint;
    public LineRenderer laserLine;
    private Transform target;
    public float rotationSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindTarget();
        if (target != null)
        {
            FireLaser();
            RotateToTarget();
        }
        else
        {
            DeactivateLaser();
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestTarget = range;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <closestTarget)
            {
                closestTarget = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }
        target = closestEnemy;
    }

    void FireLaser()
    {
        if (target == null)
        {
            DeactivateLaser();
            return;
        }
        laserLine.enabled = true;
        laserLine.SetPosition(0, shootPoint.position);
        laserLine.SetPosition(1, target.position);

        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(dps * Time.deltaTime);
        }
        
    }

    void DeactivateLaser()
    {
        laserLine.enabled = false;
        laserLine.SetPosition(1, shootPoint.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void RotateToTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        Quaternion rotationtoTarget = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationtoTarget, Time.deltaTime * rotationSpeed);

        if (shootPoint != null)
        {
            shootPoint.LookAt(target.position);
        }
    }
}
