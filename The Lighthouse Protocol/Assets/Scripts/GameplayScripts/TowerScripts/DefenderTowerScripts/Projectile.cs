using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    private Transform target;
    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if(Vector3.Distance(transform.position, target.position)<0.5f)
        {
            Destroy(gameObject);
            EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
