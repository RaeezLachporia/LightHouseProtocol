using System.Collections;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public Transform eyes; // Assign your "Eyes" child object here
    public float roamSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 10f;
    public float lostSightTime = 3f; // Time before enemy gives up searching

    private Transform player;
    private List<Transform> waypoints;
    private Transform currentWaypoint;
    private int waypointIndex = 0;
    private bool chasing = false;
    private float timeSinceLastSeen = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new List<Transform>();
        foreach (GameObject obj in waypointObjects)
        {
            waypoints.Add(obj.transform);
        }

        if (waypoints.Count > 0)
        {
            currentWaypoint = waypoints[0];
        }
    }

    void Update()
    {
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

        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, roamSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentWaypoint.position) < 1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Count;
            currentWaypoint = waypoints[waypointIndex];
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

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
}