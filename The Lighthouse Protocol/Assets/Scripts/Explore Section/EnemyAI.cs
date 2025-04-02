using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform eyes; // Assign your "Eyes" child object here
    public float roamSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 10f;
    public float lostSightTime = 3f; // Time before enemy gives up searching
    public float obstacleAvoidanceRange = 2f; // How far ahead to check for obstacles
    public float obstacleAvoidanceStrength = 2f; // How strong the avoidance force is

    private Transform player;
    private List<Transform> waypoints;
    private Transform currentWaypoint;
    private int waypointIndex = 0;
    private bool chasing = false;
    private float timeSinceLastSeen = 0f;
    private Vector3 moveDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new List<Transform>();
        foreach (GameObject obj in waypointObjects)
        {
            waypoints.Add(obj.transform);
        }

        if (waypoints.Count > 0)
        {
            currentWaypoint = waypoints[0];
            moveDirection = (currentWaypoint.position - transform.position).normalized;
        }
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
        transform.forward = Vector3.Lerp(transform.forward, targetDirection, 0.1f); // Smooth rotation

        if (Vector3.Distance(transform.position, currentWaypoint.position) < 1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Count;
            currentWaypoint = waypoints[waypointIndex];
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        Vector3 targetDirection = (player.position - transform.position).normalized;
        targetDirection = AvoidObstacles(targetDirection);

        transform.position += targetDirection * chaseSpeed * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, targetDirection, 0.1f);

        if (!CanSeePlayer())
        {
            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen >= lostSightTime)
            {
                chasing = false;
                timeSinceLastSeen = 0f;
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

        // Check forward
        if (Physics.Raycast(transform.position, direction, out hit, obstacleAvoidanceRange))
        {
            if (!hit.collider.CompareTag("Player")) // Ignore player
            {
                // Avoid by steering right or left
                if (!Physics.Raycast(transform.position, right, obstacleAvoidanceRange))
                    return (direction + right * obstacleAvoidanceStrength).normalized;
                else if (!Physics.Raycast(transform.position, left, obstacleAvoidanceRange))
                    return (direction + left * obstacleAvoidanceStrength).normalized;
            }
        }

        return direction; // No obstacles, move normally
    }
}