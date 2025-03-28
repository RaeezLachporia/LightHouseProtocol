using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenV2 : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    public Transform[] spawnPoints;
    // [SerializeField] private GameObject EnemyType2;
    // [SerializeField] private GameObject EnemyType3;

    [SerializeField] private float archerInterval = 3.5f;
   // [SerializeField] private float assassinInterval = 3.5f;
   // [SerializeField] private float bruteInterval = 3.5f;


    void Start()
    {
        StartCoroutine(spawnEnemy(archerInterval, enemyPrefab));
       // StartCoroutine(spawnEnemy(assassinInterval, EnemyType2));
       // StartCoroutine(spawnEnemy(bruteInterval, EnemyType3));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)// 
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);//spawns enemies 
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
