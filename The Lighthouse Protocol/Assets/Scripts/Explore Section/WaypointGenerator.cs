using System.Collections.Generic;
using UnityEngine;

public class WaypointGenerator : MonoBehaviour
{
    public GameObject waypointPrefab;  // Prefab for the waypoint
    public int maxWaypoints = 20;  // Number of waypoints to place
    public float minSpacing = 5f;  // Minimum distance between waypoints
    public LayerMask caveLayer;  // Layer to check for valid positions
    public Vector3 caveBoundsMin;  // Bottom-left-back corner of the cave
    public Vector3 caveBoundsMax;  // Top-right-front corner of the cave

    private List<Vector3> waypointPositions = new List<Vector3>();

    void Start()
    {
        GenerateWaypoints();
    }

    void GenerateWaypoints()
    {
        int attempts = 0;
        while (waypointPositions.Count < maxWaypoints && attempts < maxWaypoints * 10)
        {
            attempts++;
            Vector3 randomPosition = GetRandomPositionInCave();

            if (IsValidWaypoint(randomPosition))
            {
                waypointPositions.Add(randomPosition);
                Instantiate(waypointPrefab, randomPosition, Quaternion.identity);
            }
        }
    }

    Vector3 GetRandomPositionInCave()
    {
        float x = Random.Range(caveBoundsMin.x, caveBoundsMax.x);
        float y = Random.Range(caveBoundsMin.y, caveBoundsMax.y);
        float z = Random.Range(caveBoundsMin.z, caveBoundsMax.z);
        return new Vector3(x, y, z);
    }

    bool IsValidWaypoint(Vector3 position)
    {
        // Ensure waypoints are not too close to each other
        foreach (Vector3 waypoint in waypointPositions)
        {
            if (Vector3.Distance(waypoint, position) < minSpacing)
                return false;
        }

        // Check if the position is inside the cave
        return Physics.CheckSphere(position, 1f, caveLayer);
    }
}