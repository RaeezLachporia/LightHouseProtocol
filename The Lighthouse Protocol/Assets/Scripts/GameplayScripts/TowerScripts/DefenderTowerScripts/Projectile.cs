using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    public float lifetime = 5f;
    private Transform target;
    public void SetTarget(Transform enemy)
    {
        target = enemy;
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            lifetime = distance / speed;
            Destroy(gameObject, lifetime + 0.5f);
        }
        else
        {
            Destroy(gameObject, 5f);
        }
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

    private void TargetHit()
    {
        if (target.TryGetComponent(out EnemyHealth enemyHealth))  
        {
            enemyHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
