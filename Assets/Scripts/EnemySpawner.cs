using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int maximumAmountOfSpawns = 10;
    public GameObject EnemyAsset;
    public int EnemiesPerRow;
    public float DistanceBetweenEnemies;

    public IEnumerator SpawnGroup(int amount)
    {
        int amountToSpawn = amount;
        int amountPerWave = amount;

        if (amountPerWave > maximumAmountOfSpawns)
        {
            amountPerWave = maximumAmountOfSpawns;
            amountToSpawn -= amountPerWave;
        }

        while (amountToSpawn > 0)
        {
            if (amountPerWave > amountToSpawn)  // If the wave amount is higher than the amount left
            {
                amountPerWave = amountToSpawn;
            }

            for (int i = 0; i < amountPerWave; i++)
            {
                GameManager.instance.EnemiesLeft--;
                GameManager.AmountOfActiveEnemies++;
                amountToSpawn--;

                var enemy = Instantiate(EnemyAsset);
                enemy.transform.position = transform.position;

                Vector3 right = transform.right * DistanceBetweenEnemies * (i % EnemiesPerRow);
                Vector3 backward = -transform.forward * DistanceBetweenEnemies * Mathf.FloorToInt(i / EnemiesPerRow);

                enemy.transform.Translate(right + backward);
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
