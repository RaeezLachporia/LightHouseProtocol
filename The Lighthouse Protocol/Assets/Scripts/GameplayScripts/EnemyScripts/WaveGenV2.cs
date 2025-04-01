using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenV2 : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    public Transform[] spawnPoints;
    [SerializeField] private float archerInterval = 3.5f;

    void Start()
    {
        StartCoroutine(SpawnEnemyLoop(archerInterval, enemyPrefab));
    }

    private IEnumerator SpawnEnemyLoop(float interval, GameObject enemy)
    {
        while (true) //Keeps running without recursion
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(interval); //Wait before spawning again
        }
    }
}