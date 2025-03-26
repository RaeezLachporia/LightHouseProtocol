using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public Transform NavTarget;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (NavTarget != null)
        {
            agent.SetDestination(NavTarget.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (NavTarget != null)
        {
            agent.SetDestination(NavTarget.position);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        NavTarget = newTarget;
    }
}
