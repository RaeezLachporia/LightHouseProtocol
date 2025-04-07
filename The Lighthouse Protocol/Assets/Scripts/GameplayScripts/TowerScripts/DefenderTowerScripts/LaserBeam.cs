using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LaserBeam : MonoBehaviour
{

    public float range = 10f;
    public float dps = 5f;
    public Transform shootPoint;
    public LineRenderer laserLine;
    private Transform target;
    public float rotationSpeed = 5f;
    public float TwrHealth = 100;//Health of the tower
    public float currentTwrHealth;
    public Slider TwrHealthSlider;
    private static List<LaserBeam> upgradableLaserTowers = new List<LaserBeam>();
    public Dictionary<int, Dictionary<string, int>> upgradeCosts = new Dictionary<int, Dictionary<string, int>>();
    // Start is called before the first frame update
    void Start()
    {
        currentTwrHealth = TwrHealth; ;
        TwrHealthSlider.maxValue = TwrHealth;
        TwrHealthSlider.value = currentTwrHealth;
        upgradableLaserTowers.Add(this);
        UpgradeCosts();
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

    private void UpgradeCosts()
    {
        upgradeCosts[0] = new Dictionary<string, int> { { "Wood", 10 }, { "Metal", 5 } };
        upgradeCosts[1] = new Dictionary<string, int> { { "Wood", 20 }, { "Metal", 10 } };
        upgradeCosts[2] = new Dictionary<string, int> { { "Wood", 40 }, { "Metal", 20 } };
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
