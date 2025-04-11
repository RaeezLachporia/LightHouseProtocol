using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public Transform LightHouse;
    private NavMeshAgent agent;
    public float detectionRaidus = 20f;
    public Transform NavTarget;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        LightHouse = GameObject.FindWithTag("LightHouseNav").transform;

        UpdateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();
    }

    /*public void SetTarget(Transform newTarget)
    {
        NavTarget = newTarget;
    }*/ //changed to allow for a new method that tracks the nearest target, if no new target detected enemy will go for the tower.

    void UpdateTarget()//checks if there is a target wheter it be a defense tower or a lighthouse
    {
        Transform nearestTarget = FindClosestTarget();
        if (nearestTarget != null)
        {
            NavTarget = nearestTarget;

        }
        else if(LightHouse != null)
        {
            NavTarget = LightHouse;
        }

        if (NavTarget != null || agent != null)
        {
            agent.SetDestination(NavTarget.position);
        }
    }

    public Transform FindClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRaidus);
        Transform closestTarget = null;
        float closeDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Tower") || col.CompareTag("LaserBeam") || col.CompareTag("LightHouseNav")) 
            {
                float distanceToTarget = Vector3.Distance(transform.position, col.transform.position);
                if (distanceToTarget<closeDistance)
                {
                    closeDistance = distanceToTarget;
                    closestTarget = col.transform;
                }
            }
        }
        return closestTarget;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRaidus);
    }

    public void SetTarget(Transform newTarget)
    {
        NavTarget = newTarget;
        if (agent != null || NavTarget!= null)
        {
            agent.SetDestination(NavTarget.position);
        }
    }
}
