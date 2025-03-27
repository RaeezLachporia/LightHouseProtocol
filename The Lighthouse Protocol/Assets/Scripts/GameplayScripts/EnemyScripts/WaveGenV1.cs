using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WaveGenV1 : MonoBehaviour
{

    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float SpawnIntervals = 5f;
    //public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(SpawnIntervals);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length==null)
        {
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(spawnPoint.position);
        }
        Movement enemyScript = enemy.GetComponent<Movement>();
        if (enemyScript!= null)
        {
            Transform nearestTarget = enemyScript.FindClosestTarget();
            if (nearestTarget != null)
            {
                enemyScript.SetTarget(nearestTarget);
            }
        }
    }
}
