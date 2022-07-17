using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyAsset;
    public int NrOfEnemiesToSpawn = 2;
    public int EnemyIncrement;
    public int EnemiesPerRow;
    public float DistanceBetweenEnemies;

    public bool spawnEnemies = false;
    
    public void SpawnAll()
    {
        for (int i = 0; i < NrOfEnemiesToSpawn; i++)
        {
            var enemy = GameObject.Instantiate(EnemyAsset);
            enemy.transform.position = transform.position;
            
            Vector3 right = transform.right * DistanceBetweenEnemies * (i % EnemiesPerRow);
            
            
            Vector3 backward = -transform.forward * DistanceBetweenEnemies * Mathf.FloorToInt(i / EnemiesPerRow);
            
            enemy.transform.Translate(right + backward);
        }
        
        NrOfEnemiesToSpawn += EnemyIncrement;
    }
    public IEnumerator SpawnConsecutively()
    {
        for (int i = 0; i < NrOfEnemiesToSpawn; i++)
        {
            var enemy = GameObject.Instantiate(EnemyAsset);
            enemy.transform.position = transform.position;
            yield return new WaitForSeconds(1.25f);
        }

        NrOfEnemiesToSpawn += EnemyIncrement;
    }
    private void OnValidate()
    {
        if (spawnEnemies)
        {
            spawnEnemies = false;
            StopAllCoroutines();
            _ = StartCoroutine(SpawnConsecutively());
        }
    }
}
