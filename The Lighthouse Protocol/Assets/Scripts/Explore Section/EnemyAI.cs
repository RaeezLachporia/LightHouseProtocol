using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform eyes;
    public float roamSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 10f;
    public float lostSightTime = 3f;
    public float obstacleAvoidanceRange = 2f;
    public float obstacleAvoidanceStrength = 2f;

    private Transform player;
    private List<Transform> waypoints;
    private List<Transform> seenWaypoints = new List<Transform>();
    private Transform currentWaypoint;
    private bool chasing = false;
    private float timeSinceLastSeen = 0f;


    public float attackRange = 2f;
    public float damage = 10f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new List<Transform>();
        foreach (GameObject obj in waypointObjects)
        {
            waypoints.Add(obj.transform);
        }

        PickNewWaypoint();
    }

    void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj) player = playerObj.transform;
        }

        if (chasing)
        {
            ChasePlayer();
        }
        else
        {
            Roam();
        }

        CheckForPlayer();
    }

    void Roam()
    {
        if (currentWaypoint == null) return;

        Vector3 targetDirection = (currentWaypoint.position - transform.position).normalized;
        targetDirection = AvoidObstacles(targetDirection);

        transform.position += targetDirection * roamSpeed * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, targetDirection, 0.1f);

        if (Vector3.Distance(transform.position, currentWaypoint.position) < 1f)
        {
            if (!seenWaypoints.Contains(currentWaypoint))
                seenWaypoints.Add(currentWaypoint);

            PickNewWaypoint();
        }
    }

    void PickNewWaypoint()
    {
        List<Transform> unseen = waypoints.FindAll(wp => !seenWaypoints.Contains(wp));
        if (unseen.Count == 0)
        {
            seenWaypoints.Clear(); // Reset once all are visited
            unseen = new List<Transform>(waypoints);
        }

        currentWaypoint = unseen[Random.Range(0, unseen.Count)];
    }

    void ChasePlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                FirstPersonController playerController = player.GetComponent<FirstPersonController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damage);
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            Vector3 targetDirection = (player.position - transform.position).normalized;
            targetDirection = AvoidObstacles(targetDirection);

            transform.position += targetDirection * chaseSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, targetDirection, 0.1f);
        }

        if (!CanSeePlayer())
        {
            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen >= lostSightTime)
            {
                chasing = false;
                timeSinceLastSeen = 0f;
                PickNewWaypoint(); // Resume exploring
            }
        }
    }

    void CheckForPlayer()
    {
        if (CanSeePlayer())
        {
            chasing = true;
            timeSinceLastSeen = 0f;
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        RaycastHit hit;
        if (Physics.Raycast(eyes.position, (player.position - eyes.position).normalized, out hit, detectionRange))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    Vector3 AvoidObstacles(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 right = transform.right;
        Vector3 left = -transform.right;

        if (Physics.Raycast(transform.position, direction, out hit, obstacleAvoidanceRange))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                if (!Physics.Raycast(transform.position, right, obstacleAvoidanceRange))
                    return (direction + right * obstacleAvoidanceStrength).normalized;
                else if (!Physics.Raycast(transform.position, left, obstacleAvoidanceRange))
                    return (direction + left * obstacleAvoidanceStrength).normalized;
            }
        }

        return direction;
    }
}